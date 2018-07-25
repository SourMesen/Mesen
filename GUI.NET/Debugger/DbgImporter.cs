using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public class Ld65DbgImporter
	{
		private const int iNesHeaderSize = 16;

		private Dictionary<int, SegmentInfo> _segments = new Dictionary<int, SegmentInfo>();
		private Dictionary<int, FileInfo> _files = new Dictionary<int, FileInfo>();
		private Dictionary<int, LineInfo> _lines = new Dictionary<int, LineInfo>();
		private Dictionary<int, SpanInfo> _spans = new Dictionary<int, SpanInfo>();
		private Dictionary<int, SymbolInfo> _symbols = new Dictionary<int, SymbolInfo>();
		private Dictionary<int, CSymbolInfo> _cSymbols = new Dictionary<int, CSymbolInfo>();

		private HashSet<int> _usedFileIds = new HashSet<int>();
		private HashSet<string> _usedLabels = new HashSet<string>();
		private Dictionary<int, CodeLabel> _ramLabels = new Dictionary<int, CodeLabel>();
		private Dictionary<int, CodeLabel> _romLabels = new Dictionary<int, CodeLabel>();

		private HashSet<string> _filesNotFound = new HashSet<string>();
		private int _errorCount = 0;

		private static Regex _segmentRegex = new Regex("^seg\tid=([0-9]+),.*start=0x([0-9a-fA-F]+),.*size=0x([0-9A-Fa-f]+)", RegexOptions.Compiled);
		private static Regex _segmentPrgRomRegex = new Regex("^seg\tid=([0-9]+),.*start=0x([0-9a-fA-F]+),.*size=0x([0-9A-Fa-f]+),.*ooffs=([0-9]+)", RegexOptions.Compiled);
		private static Regex _lineRegex = new Regex("^line\tid=([0-9]+),.*file=([0-9]+),.*line=([0-9]+)(,.*type=([0-9]+)){0,1}(,.*span=([0-9]+)){0,1}", RegexOptions.Compiled);
		private static Regex _fileRegex = new Regex("^file\tid=([0-9]+),.*name=\"([^\"]+)\"", RegexOptions.Compiled);
		private static Regex _spanRegex = new Regex("^span\tid=([0-9]+),.*seg=([0-9]+),.*start=([0-9]+),.*size=([0-9]+)(,.*type=([0-9]+)){0,1}", RegexOptions.Compiled);
		private static Regex _symbolRegex = new Regex("^sym\tid=([0-9]+).*name=\"([0-9a-zA-Z@_-]+)\"(,.*def=([0-9+]+)){0,1}(,.*ref=([0-9+]+)){0,1}(,.*val=0x([0-9a-fA-F]+)){0,1}(,.*seg=([0-9]+)){0,1}(,.*exp=([0-9]+)){0,1}", RegexOptions.Compiled);
		private static Regex _cSymbolRegex = new Regex("^csym\tid=([0-9]+).*name=\"([0-9a-zA-Z@_-]+)\"(,.*sym=([0-9+]+)){0,1}", RegexOptions.Compiled);

		private static Regex _asmFirstLineRegex = new Regex(";(.*)", RegexOptions.Compiled);
		private static Regex _asmPreviousLinesRegex = new Regex("^\\s*;(.*)", RegexOptions.Compiled);
		private static Regex _cFirstLineRegex = new Regex("(.*)", RegexOptions.Compiled);
		private static Regex _cPreviousLinesRegex = new Regex("^\\s*//(.*)", RegexOptions.Compiled);

		private Dictionary<int, LineInfo> _linesByPrgAddress = new Dictionary<int, LineInfo>();
		private Dictionary<string, int> _prgAddressByLine = new Dictionary<string, int>();

		public Dictionary<int, FileInfo> Files { get { return _files; } }

		public int GetPrgAddress(int fileID, int lineIndex)
		{
			int prgAddress;
			if(_prgAddressByLine.TryGetValue(fileID.ToString() + "_" + lineIndex.ToString(), out prgAddress)) {
				return prgAddress;
			}
			return -1;
		}

		public LineInfo GetSourceCodeLineInfo(int prgRomAddress)
		{
			LineInfo line;
			if(_linesByPrgAddress.TryGetValue(prgRomAddress, out line)) {
				return line;
			}
			return null;
		}

		public string GetSourceCodeLine(int prgRomAddress)
		{
			if(prgRomAddress >= 0) {
				try {
					LineInfo line;
					if(_linesByPrgAddress.TryGetValue(prgRomAddress, out line)) {
						string output = "";
						FileInfo file = _files[line.FileID];
						if(file.Data == null) {
							return string.Empty;
						}
						
						output += file.Data[line.LineNumber];
						return output;
					}
				} catch { }
			}
			return null;
		}

		private SymbolInfo GetMatchingSymbol(SymbolInfo symbol, int rangeStart, int rangeEnd)
		{
			foreach(int reference in symbol.References) {
				LineInfo line = _lines[reference];
				if(line.SpanID == null) {
					continue;
				}

				SpanInfo span = _spans[line.SpanID.Value];
				SegmentInfo seg = _segments[span.SegmentID];

				if(!seg.IsRam) {
					int spanPrgOffset = seg.FileOffset - iNesHeaderSize + span.Offset;
					if(rangeStart < spanPrgOffset + span.Size && rangeEnd >= spanPrgOffset) {
						if(symbol.ExportSymbolID != null && symbol.Address == null) {
							return _symbols[symbol.ExportSymbolID.Value];
						} else {
							return symbol;
						}
					}
				}
			}									
			return null;
		}

		internal SymbolInfo GetSymbol(string word, int prgStartAddress, int prgEndAddress)
		{
			try {
				foreach(CSymbolInfo symbol in _cSymbols.Values) {
					if(symbol.Name == word && symbol.SymbolID.HasValue) {
						SymbolInfo matchingSymbol = GetMatchingSymbol(_symbols[symbol.SymbolID.Value], prgStartAddress, prgEndAddress);
						if(matchingSymbol != null) {
							return matchingSymbol;
						}
					}
				}

				foreach(SymbolInfo symbol in _symbols.Values) {
					if(symbol.Name == word) {
						SymbolInfo matchingSymbol = GetMatchingSymbol(symbol, prgStartAddress, prgEndAddress);
						if(matchingSymbol != null) {
							return matchingSymbol;
						}
					}
				}
			} catch { }

			return null;
		}

		public AddressTypeInfo GetSymbolAddressInfo(SymbolInfo symbol)
		{
			if(symbol.SegmentID == null || symbol.Address == null) {
				return null;
			}

			SegmentInfo segment = _segments[symbol.SegmentID.Value];
			if(segment.IsRam) {
				return new AddressTypeInfo() { Address = symbol.Address.Value, Type = AddressType.Register };
			} else {
				return new AddressTypeInfo() { Address = symbol.Address.Value - segment.Start + segment.FileOffset - iNesHeaderSize, Type = AddressType.PrgRom };
			}
		}

		private bool LoadSegments(string row)
		{
			Match match = _segmentRegex.Match(row);
			if(match.Success) {
				SegmentInfo segment = new SegmentInfo() {
					ID = Int32.Parse(match.Groups[1].Value),
					Start = Int32.Parse(match.Groups[2].Value, NumberStyles.HexNumber),
					Size = Int32.Parse(match.Groups[3].Value, NumberStyles.HexNumber),
					IsRam = true
				};

				match = _segmentPrgRomRegex.Match(row);
				if(match.Success && !row.Contains("type=rw")) {
					segment.FileOffset = Int32.Parse(match.Groups[4].Value);
					segment.IsRam = false;
				}

				_segments.Add(segment.ID, segment);
				return true;
			} else if(row.StartsWith("seg")) {
				System.Diagnostics.Debug.Fail("Regex doesn't match seg");
			}

			return false;
		}

		private bool LoadFiles(string row, string basePath)
		{
			Match match = _fileRegex.Match(row);
			if(match.Success) {
				FileInfo file = new FileInfo() {
					ID = Int32.Parse(match.Groups[1].Value),
					Name = Path.GetFullPath(Path.Combine(basePath, match.Groups[2].Value.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar))).Replace(basePath + Path.DirectorySeparatorChar, "")
				};

				_files.Add(file.ID, file);
				return true;
			} else if(row.StartsWith("file")) {
				System.Diagnostics.Debug.Fail("Regex doesn't match file");
			}

			return false;
		}

		private bool LoadLines(string row)
		{
			Match match = _lineRegex.Match(row);
			if(match.Success) {
				LineInfo line = new LineInfo() {
					ID = Int32.Parse(match.Groups[1].Value),
					FileID = Int32.Parse(match.Groups[2].Value),
					LineNumber = Int32.Parse(match.Groups[3].Value) - 1,
					Type = match.Groups[5].Success ? (LineType)Int32.Parse(match.Groups[5].Value) : LineType.Assembly,
					SpanID = match.Groups[7].Success ? (int?)Int32.Parse(match.Groups[7].Value) : null
				};

				_usedFileIds.Add(line.FileID);
				_lines.Add(line.ID, line);
				return true;
			} else if(row.StartsWith("line")) {
				System.Diagnostics.Debug.Fail("Regex doesn't match line");
			}

			return false;
		}

		private bool LoadSpans(string row)
		{
			Match match = _spanRegex.Match(row);
			if(match.Success) {
				SpanInfo span = new SpanInfo() {
					ID = Int32.Parse(match.Groups[1].Value),
					SegmentID = Int32.Parse(match.Groups[2].Value),
					Offset = Int32.Parse(match.Groups[3].Value),
					Size = Int32.Parse(match.Groups[4].Value)
				};
				span.IsData = match.Groups[6].Success;

				_spans.Add(span.ID, span);
				return true;
			} else if(row.StartsWith("span")) {
				System.Diagnostics.Debug.Fail("Regex doesn't match span");
			}

			return false;
		}
		
		private bool LoadCSymbols(string row)
		{
			Match match = _cSymbolRegex.Match(row);
			if(match.Success) {
				CSymbolInfo span = new CSymbolInfo() {
					ID = Int32.Parse(match.Groups[1].Value),
					Name = match.Groups[2].Value,
					SymbolID = match.Groups[4].Success ? (int?)Int32.Parse(match.Groups[4].Value) : null,
				};
				_cSymbols.Add(span.ID, span);
				return true;
			} else if(row.StartsWith("csym")) {
				System.Diagnostics.Debug.Fail("Regex doesn't match csym");
			}

			return false;
		}

		private bool LoadSymbols(string row)
		{
			Match match = _symbolRegex.Match(row);
			if(match.Success) {
				SymbolInfo symbol = new SymbolInfo() {
					ID = Int32.Parse(match.Groups[1].Value),
					Name = match.Groups[2].Value,
					Address = match.Groups[8].Success ? (int?)Int32.Parse(match.Groups[8].Value, NumberStyles.HexNumber) : null,
					SegmentID = match.Groups[10].Success ? (int?)Int32.Parse(match.Groups[10].Value) : null,
					ExportSymbolID = match.Groups[12].Success ? (int?)Int32.Parse(match.Groups[12].Value) : null
				};

				if(match.Groups[4].Success) {
					symbol.Definitions = match.Groups[4].Value.Split('+').Select(o => Int32.Parse(o)).ToList();
				} else {
					symbol.Definitions = new List<int>();
				}

				if(match.Groups[6].Success) {
					symbol.References = match.Groups[6].Value.Split('+').Select(o => Int32.Parse(o)).ToList();
				} else {
					symbol.References = new List<int>();
				}

				_symbols.Add(symbol.ID, symbol);
				return true;
			} else if(row.StartsWith("sym")) {
				System.Diagnostics.Debug.Fail("Regex doesn't match sym");
			}

			return false;
		}

		private void LoadLabels()
		{
			foreach(KeyValuePair<int, SymbolInfo> kvp in _symbols) {
				try {
					SymbolInfo symbol = kvp.Value;
					if(symbol.SegmentID == null) {
						continue;
					}

					if(_segments.ContainsKey(symbol.SegmentID.Value)) {
						SegmentInfo segment = _segments[symbol.SegmentID.Value];

						int count = 2;
						string orgSymbolName = symbol.Name;
						string newName = symbol.Name;
						while(!_usedLabels.Add(newName)) {
							//Ensure labels are unique
							newName = orgSymbolName + "_" + count.ToString();
							count++;
						}

						int address = GetSymbolAddressInfo(symbol).Address;
						if(segment.IsRam) {
							_ramLabels[address] = new CodeLabel() { Label = newName, Address = (UInt32)address, AddressType = AddressType.InternalRam, Comment = string.Empty };
						} else {
							_romLabels[address] = new CodeLabel() { Label = newName, Address = (UInt32)address, AddressType = AddressType.PrgRom, Comment = string.Empty };
						}
					}
				} catch {
					_errorCount++;
				}
			}
		}

		private void LoadComments()
		{
			foreach(KeyValuePair<int, LineInfo> kvp in _lines) {
				try {
					LineInfo line = kvp.Value;
					if(line.SpanID == null) {
						continue;
					}

					SpanInfo span = _spans[line.SpanID.Value];
					SegmentInfo segment = _segments[span.SegmentID];

					if(_files[line.FileID].Data == null) {
						//File was not found.
						if(_filesNotFound.Add(_files[line.FileID].Name)) {
							_errorCount++;
						}
						continue;
					}

					string ext = Path.GetExtension(_files[line.FileID].Name).ToLower();
					bool isAsm = ext != ".c" && ext != ".h";

					string comment = "";
					for(int i = line.LineNumber; i >= 0; i--) {
						string sourceCodeLine = _files[line.FileID].Data[i];
						if(sourceCodeLine.Trim().Length == 0) {
							//Ignore empty lines
							continue;
						}

						Regex regex;
						if(i == line.LineNumber) {
							regex = isAsm ? _asmFirstLineRegex : _cFirstLineRegex;
						} else {
							regex = isAsm ? _asmPreviousLinesRegex : _cPreviousLinesRegex;
						}

						Match match = regex.Match(sourceCodeLine);
						if(match.Success) {
							string matchedComment = match.Groups[1].Value.Replace("\t", " ");
							if(string.IsNullOrWhiteSpace(comment)) {
								comment = matchedComment;
							} else {
								comment = matchedComment + Environment.NewLine + comment;
							}
						} else if(i != line.LineNumber) {
							break;
						}
					}

					if(comment.Length > 0) {
						CodeLabel label;
						if(segment.IsRam) {
							int address = span.Offset + segment.Start;
							if(!_ramLabels.TryGetValue(address, out label)) {
								label = new CodeLabel() { Address = (UInt32)address, AddressType = AddressType.InternalRam, Label = string.Empty };
								_ramLabels[span.Offset] = label;
							}
						} else {
							int address = span.Offset + segment.FileOffset - iNesHeaderSize;
							if(!_romLabels.TryGetValue(address, out label)) {
								label = new CodeLabel() { Address = (UInt32)address, AddressType = AddressType.PrgRom, Label = string.Empty };
								_romLabels[span.Offset] = label;
							}
						}
						label.Comment = comment;
					}
				} catch {
					_errorCount++;
				}
			}
		}

		private void LoadFileData(string path)
		{
			foreach(FileInfo file in _files.Values) {
				if(_usedFileIds.Contains(file.ID)) {
					try {
						string basePath = path;
						string sourceFile = Path.Combine(basePath, file.Name);
						while(!File.Exists(sourceFile)) {
							//Go back up folder structure to attempt to find the file
							string oldPath = basePath;
							basePath = Path.GetDirectoryName(basePath);
							if(basePath == null || basePath == oldPath) {
								break;
							}
							sourceFile = Path.Combine(basePath, file.Name);
						}

						if(File.Exists(sourceFile)) {
							file.Data = File.ReadAllLines(sourceFile);
						}
					} catch {
						_errorCount++;
					}
				}
			}
		}

		public void Import(string path, bool silent = false)
		{
			string[] fileRows = File.ReadAllLines(path);

			string basePath = Path.GetDirectoryName(path);
			foreach(string row in fileRows) {
				try {
					if(LoadLines(row) || LoadSpans(row) || LoadSymbols(row) || LoadCSymbols(row) || LoadFiles(row, basePath) || LoadSegments(row)) {
						continue;
					}
				} catch {
					_errorCount++;
				}
			}

			LoadFileData(basePath);

			int prgSize = InteropEmu.DebugGetMemorySize(DebugMemoryType.PrgRom);
			if(prgSize > 0) {
				byte[] cdlFile = new byte[prgSize];
				foreach(KeyValuePair<int, SpanInfo> kvp in _spans) {
					SegmentInfo segment;
					if(_segments.TryGetValue(kvp.Value.SegmentID, out segment)) {
						if(!segment.IsRam && kvp.Value.Size != segment.Size) {
							int prgAddress = kvp.Value.Offset + segment.FileOffset - iNesHeaderSize;

							if(prgAddress >= 0 && prgAddress < prgSize) {
								for(int i = 0; i < kvp.Value.Size; i++) {
									if(cdlFile[prgAddress + i] == 0 && !kvp.Value.IsData && kvp.Value.Size <= 3) {
										cdlFile[prgAddress + i] = (byte)0x01;
									} else if(kvp.Value.IsData) {
										cdlFile[prgAddress + i] = (byte)0x02;
									}
								}
							}
						}
					}
				}
				InteropEmu.DebugSetCdlData(cdlFile);
			}

			foreach(LineInfo line in _lines.Values) {
				if(line.SpanID == null) {
					continue;
				}

				FileInfo file = _files[line.FileID];
				SpanInfo span = _spans[line.SpanID.Value];
				SegmentInfo segment = _segments[span.SegmentID];
				if(!segment.IsRam) {
					for(int i = 0; i < span.Size; i++) {
						int prgAddress = segment.FileOffset - iNesHeaderSize + span.Offset + i;

						LineInfo existingLine;
						if(_linesByPrgAddress.TryGetValue(prgAddress, out existingLine) && existingLine.Type == LineType.External) {
							//Give priority to lines that come from C files
							continue;
						}

						_linesByPrgAddress[prgAddress] = line;
						if(i == 0) {
							_prgAddressByLine[file.ID.ToString() + "_" + line.LineNumber.ToString()] = prgAddress;
						}
					}
				}
			}

			LoadLabels();

			int labelCount = 0;

			DebugImportConfig config = ConfigManager.Config.DebugInfo.ImportConfig;
			if(config.DbgImportComments) {
				LoadComments();
			}
			List<CodeLabel> labels = new List<CodeLabel>(_romLabels.Count + _ramLabels.Count);
			if(config.DbgImportPrgRomLabels) {
				labels.AddRange(_romLabels.Values);
				labelCount += _romLabels.Count;
			}
			if(config.DbgImportRamLabels) {
				labels.AddRange(_ramLabels.Values);
				labelCount += _ramLabels.Count;
			}
			LabelManager.SetLabels(labels);
			
			if(!silent) {
				if(_errorCount > 0) {
					_errorCount -= _filesNotFound.Count;
					string message = $"Import completed with {labelCount} labels imported";
					if(_errorCount > 0) {
						message += $"and {_errorCount} errors - please file a bug report and attach the DBG file you tried to import.";
					}
					if(_filesNotFound.Count > 0) {
						message += Environment.NewLine + Environment.NewLine + "The following files could not be found:";
						foreach(string file in _filesNotFound) {
							message += Environment.NewLine + file;
						}
					}
					MessageBox.Show(message, "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				} else {
					MessageBox.Show($"Import completed with {labelCount} labels imported.", "Mesen", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private class SegmentInfo
		{
			public int ID;
			public int Start;
			public int Size;
			public int FileOffset;
			public bool IsRam;
		}

		public class FileInfo
		{
			public int ID;
			public string Name;
			public string[] Data;

			public override string ToString()
			{
				string folderName = Path.GetDirectoryName(Name);
				string fileName = Path.GetFileName(Name);
				if(string.IsNullOrWhiteSpace(folderName)) {
					return fileName;
				} else {
					return $"{fileName} ({folderName})";
				}
			}
		}

		public class LineInfo
		{
			public int ID;
			public int FileID;
			public int? SpanID;
			public LineType Type;

			public int LineNumber;
		}

		public enum LineType
		{
			Assembly = 0, 
			External = 1, //i.e C source file
			Macro = 2
		}

		private class SpanInfo
		{
			public int ID;
			public int SegmentID;
			public int Offset;
			public int Size;
			public bool IsData;
		}

		public class SymbolInfo
		{
			public int ID;
			public string Name;
			public int? Address;
			public int? SegmentID;
			public int? ExportSymbolID;
			public List<int> References;
			public List<int> Definitions;
		}

		public class CSymbolInfo
		{
			public int ID;
			public string Name;
			public int? SymbolID;
		}
	}
}
