using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	class Ld65DbgImporter
	{
		private const int iNesHeaderSize = 16;

		private Dictionary<int, SegmentInfo> _segments = new Dictionary<int, SegmentInfo>();
		private Dictionary<int, FileInfo> _files = new Dictionary<int, FileInfo>();
		private Dictionary<int, LineInfo> _lines = new Dictionary<int, LineInfo>();
		private Dictionary<int, SpanInfo> _spans = new Dictionary<int, SpanInfo>();
		private Dictionary<int, SymbolInfo> _symbols = new Dictionary<int, SymbolInfo>();

		private HashSet<string> _usedLabels = new HashSet<string>();
		private Dictionary<int, CodeLabel> _ramLabels = new Dictionary<int, CodeLabel>();
		private Dictionary<int, CodeLabel> _romLabels = new Dictionary<int, CodeLabel>();

		private HashSet<string> _filesNotFound = new HashSet<string>();
		private int _errorCount = 0;

		private static Regex _segmentRegex = new Regex("seg\tid=([0-9]+),.*start=0x([0-9a-fA-F]+)", RegexOptions.Compiled);
		private static Regex _segmentPrgRomRegex = new Regex("seg\tid=([0-9]+),.*start=0x([0-9a-fA-F]+),.*ooffs=([0-9]+)", RegexOptions.Compiled);
		private static Regex _lineRegex = new Regex("line\tid=([0-9]+),.*file=([0-9]+),.*line=([0-9]+),.*span=([0-9]+)", RegexOptions.Compiled);
		private static Regex _fileRegex = new Regex("file\tid=([0-9]+),.*name=\"([0-9a-zA-Z()_.-:/\\\\ ]+)\"", RegexOptions.Compiled);
		private static Regex _spanRegex = new Regex("span\tid=([0-9]+),.*seg=([0-9]+),.*start=([0-9]+)", RegexOptions.Compiled);
		private static Regex _symbolRegex = new Regex("sym\tid=([0-9]+).*name=\"([0-9a-zA-Z@_]+)\",.*val=0x([0-9a-fA-F]+),.*seg=([0-9a-zA-Z]+)", RegexOptions.Compiled);

		private static Regex _asmFirstLineRegex = new Regex(";(.*)", RegexOptions.Compiled);
		private static Regex _asmPreviousLinesRegex = new Regex("^\\s*;(.*)", RegexOptions.Compiled);
		private static Regex _cFirstLineRegex = new Regex("(.*)", RegexOptions.Compiled);
		private static Regex _cPreviousLinesRegex = new Regex("^\\s*//(.*)", RegexOptions.Compiled);

		private bool LoadSegments(string row)
		{
			Match match = _segmentRegex.Match(row);
			if(match.Success) {
				SegmentInfo segment = new SegmentInfo() {
					ID = Int32.Parse(match.Groups[1].Value),
					Start = Int32.Parse(match.Groups[2].Value, System.Globalization.NumberStyles.HexNumber),
					IsRam = true
				};

				match = _segmentPrgRomRegex.Match(row);
				if(match.Success) {
					segment.FileOffset = Int32.Parse(match.Groups[3].Value);
					segment.IsRam = false;
				}

				_segments.Add(segment.ID, segment);
				return true;
			}

			return false;
		}

		private bool LoadFiles(string row, string basePath)
		{
			Match match = _fileRegex.Match(row);
			if(match.Success) {
				FileInfo file = new FileInfo() {
					ID = Int32.Parse(match.Groups[1].Value),
					Name = match.Groups[2].Value.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar)
				};
				
				string sourceFile = Path.Combine(basePath, file.Name);
				if(File.Exists(sourceFile)) {
					file.Data = File.ReadAllLines(sourceFile);
				}

				_files.Add(file.ID, file);
				return true;
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
					SpanID = Int32.Parse(match.Groups[4].Value)
				};

				_lines.Add(line.ID, line);
				return true;
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
					Offset = Int32.Parse(match.Groups[3].Value)
				};

				_spans.Add(span.ID, span);
				return true;
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
					Address = Int32.Parse(match.Groups[3].Value, NumberStyles.HexNumber),
					SegmentID = Int32.Parse(match.Groups[4].Value)					
				};

				_symbols.Add(symbol.ID, symbol);
				return true;
			}

			return false;
		}

		private void LoadLabels()
		{
			foreach(KeyValuePair<int, SymbolInfo> kvp in _symbols) {
				try {
					SymbolInfo symbol = kvp.Value;
					if(_segments.ContainsKey(symbol.SegmentID)) {
						SegmentInfo segment = _segments[symbol.SegmentID];

						while(!_usedLabels.Add(symbol.Name)) {
							//Ensure labels are unique
							symbol.Name += "_";
						}
						
						if(segment.IsRam) {
							int address = symbol.Address;
							_ramLabels[address] = new CodeLabel() { Label = symbol.Name, Address = (UInt32)address, AddressType = AddressType.InternalRam, Comment = string.Empty };
						} else {
							int address = symbol.Address - segment.Start + segment.FileOffset - iNesHeaderSize;
							_romLabels[address] = new CodeLabel() { Label = symbol.Name, Address = (UInt32)address, AddressType = AddressType.PrgRom, Comment = string.Empty };
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
					SpanInfo span = _spans[line.SpanID];
					SegmentInfo segment = _segments[span.SegmentID];

					if(_files[line.FileID].Data == null) {
						//File was not found.
						if(_filesNotFound.Add(_files[line.FileID].Name)) {
							_errorCount++;
						}
						continue;
					}

					bool isAsm = Path.GetExtension(_files[line.FileID].Name).StartsWith(".s");

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

		public void Import(string path)
		{
			string[] fileRows = File.ReadAllLines(path);

			string basePath = Path.GetDirectoryName(path);
			foreach(string row in fileRows) {
				try {
					if(LoadSegments(row) || LoadLines(row) || LoadSpans(row) || LoadFiles(row, basePath) || LoadSymbols(row)) {
						continue;
					}
				} catch {
					_errorCount++;
				}
			}

			LoadLabels();
			LoadComments();

			LabelManager.SetLabels(_romLabels.Values);
			LabelManager.SetLabels(_ramLabels.Values);

			int labelCount = _romLabels.Count + _ramLabels.Count;
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

		private class SegmentInfo
		{
			public int ID;
			public int Start;
			public int FileOffset;
			public bool IsRam;
		}

		private class FileInfo
		{
			public int ID;
			public string Name;
			public string[] Data;
		}

		private class LineInfo
		{
			public int ID;
			public int FileID;
			public int SpanID;

			public int LineNumber;
		}

		private class SpanInfo
		{
			public int ID;
			public int SegmentID;
			public int Offset;
		}

		private class SymbolInfo
		{
			public int ID;
			public string Name;
			public int Address;
			public int SegmentID;
		}
	}
}
