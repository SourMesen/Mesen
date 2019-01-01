using Mesen.GUI.Debugger.Controls;
using Mesen.GUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmGoToAll : BaseForm
	{
		private const int MaxResultCount = 30;

		private List<ctrlSearchResult> _results = new List<ctrlSearchResult>();
		private int _selectedResult = 0;
		private int _resultCount = 0;
		private Ld65DbgImporter _symbolProvider;
		private bool _allowOutOfScope;
		private bool _showFilesAndConstants;

		public GoToDestination Destination { get; private set; }

		public frmGoToAll(bool allowOutOfScope, bool showFilesAndConstants)
		{
			InitializeComponent();

			_symbolProvider = DebugWorkspaceManager.SymbolProvider;
			_allowOutOfScope = allowOutOfScope;
			_showFilesAndConstants = showFilesAndConstants;

			tlpResults.SuspendLayout();
			for(int i = 0; i < MaxResultCount; i++) {
				ctrlSearchResult searchResult = new ctrlSearchResult();
				searchResult.Dock = DockStyle.Top;
				searchResult.BackColor = i % 2 == 0 ? SystemColors.ControlLight : SystemColors.ControlLightLight;
				searchResult.Visible = false;
				searchResult.Click += SearchResult_Click;
				searchResult.DoubleClick += SearchResult_DoubleClick;
				tlpResults.Controls.Add(searchResult, 0, i);
				tlpResults.RowStyles.Add(new RowStyle(SizeType.AutoSize));

				_results.Add(searchResult);
			}
			tlpResults.ResumeLayout();

			UpdateResults();
		}

		private void SearchResult_Click(object sender, EventArgs e)
		{
			SelectedResult = _results.IndexOf(sender as ctrlSearchResult);
		}

		private void SearchResult_DoubleClick(object sender, EventArgs e)
		{
			SelectedResult = _results.IndexOf(sender as ctrlSearchResult);
			SelectAndClose();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(keyData == Keys.Up) {
				SelectedResult--;
				return true;
			} else if(keyData == Keys.Down) {
				SelectedResult++;
				return true;
			} else if(keyData == Keys.PageUp) {
				SelectedResult -= pnlResults.ClientSize.Height / _results[0].Height;
				return true;
			} else if(keyData == Keys.PageDown) {
				SelectedResult += pnlResults.ClientSize.Height / _results[0].Height;
				return true;
			} else if(keyData == Keys.Enter) {
				SelectAndClose();
			} else if(keyData == Keys.Escape) {
				Close();
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private int SelectedResult
		{
			get { return _selectedResult; }
			set
			{
				//Reset currently highlighted element's color
				_results[_selectedResult].BackColor = _selectedResult % 2 == 0 ? SystemColors.ControlLight : SystemColors.ControlLightLight;

				_selectedResult = Math.Max(0, Math.Min(_resultCount - 1, value));
				if(_resultCount == 0) {
					_results[0].BackColor = SystemColors.ControlLight;
				} else {
					_results[_selectedResult].BackColor = Color.LightBlue;
				}

				if(_resultCount > 0) {
					if(Program.IsMono) {
						//Use this logic to replace ScrollControlIntoView (which doesn't work properly on Mono)
						int startPos = (_results[0].Height + 1) * _selectedResult;
						int endPos = startPos + _results[0].Height + 1;

						int minVisiblePos = pnlResults.VerticalScroll.Value;
						int maxVisiblePos = pnlResults.Height + pnlResults.VerticalScroll.Value;

						if(startPos < minVisiblePos) {
							pnlResults.VerticalScroll.Value = startPos;
						} else if(endPos > maxVisiblePos) {
							pnlResults.VerticalScroll.Value = endPos - pnlResults.Height;
						}
					} else {
						pnlResults.ScrollControlIntoView(_results[_selectedResult]);
					}
				}
			}
		}

		private void UpdateResults()
		{
			string searchString = txtSearch.Text.ToLower();
			_resultCount = 0;

			HashSet<int> entryPoints = new HashSet<int>(InteropEmu.DebugGetFunctionEntryPoints());
			bool[] isJumpTargets = InteropEmu.DebugGetJumpTargets();

			List<SearchResultInfo> searchResults = new List<SearchResultInfo>();

			if(!string.IsNullOrWhiteSpace(searchString)) {
				if(_symbolProvider != null) {
					if(_showFilesAndConstants) {
						foreach(Ld65DbgImporter.FileInfo file in _symbolProvider.Files.Values) {
							if(file.Name.ToLower().Contains(searchString)) {
								searchResults.Add(new SearchResultInfo() {
									Caption = Path.GetFileName(file.Name),
									AbsoluteAddress = -1,
									MemoryType = AddressType.InternalRam,
									SearchResultType = SearchResultType.File,
									Filename = file.Name,
									FileLineNumber = 0,
									RelativeAddress = -1,
									CodeLabel = null
								});
							}
						}
					}

					foreach(Ld65DbgImporter.SymbolInfo symbol in _symbolProvider.GetSymbols()) {
						if(symbol.Name.ToLower().Contains(searchString)) {
							Ld65DbgImporter.DefinitionInfo def = _symbolProvider.GetSymbolDefinition(symbol);
							AddressTypeInfo addressInfo = _symbolProvider.GetSymbolAddressInfo(symbol);
							int value = 0;
							int relAddress = -1;
							bool isConstant = addressInfo == null;
							if(!_showFilesAndConstants && isConstant) {
								continue;
							}

							if(addressInfo != null) {
								value = InteropEmu.DebugGetMemoryValue(addressInfo.Type.ToMemoryType(), (uint)addressInfo.Address);
								relAddress = InteropEmu.DebugGetRelativeAddress((uint)addressInfo.Address, addressInfo.Type);
							} else {
								//For constants, the address field contains the constant's value
								value = symbol.Address ?? 0;
							}

							SearchResultType resultType = SearchResultType.Data;
							if(addressInfo?.Type == AddressType.PrgRom && entryPoints.Contains(addressInfo.Address)) {
								resultType = SearchResultType.Function;
							} else if(addressInfo?.Type == AddressType.PrgRom && addressInfo.Address < isJumpTargets.Length && isJumpTargets[addressInfo.Address]) {
								resultType = SearchResultType.JumpTarget;
							} else if(isConstant) {
								resultType = SearchResultType.Constant;
							}

							searchResults.Add(new SearchResultInfo() {
								Caption = symbol.Name,
								AbsoluteAddress = addressInfo?.Address ?? -1,
								MemoryType = addressInfo?.Type ?? AddressType.InternalRam,
								SearchResultType = resultType,
								Value = value,
								Filename = def?.FileName ?? "",
								FileLineNumber = def?.Line ?? 0,
								RelativeAddress = relAddress,
								CodeLabel = LabelManager.GetLabel(symbol.Name)
							});
						}
					}
				} else {
					foreach(CodeLabel label in LabelManager.GetLabels()) {
						if(label.Label.ToLower().Contains(searchString)) {
							SearchResultType resultType = SearchResultType.Data;
							if(label.AddressType == AddressType.PrgRom && entryPoints.Contains((int)label.Address)) {
								resultType = SearchResultType.Function;
							} else if(label.AddressType == AddressType.PrgRom && label.Address < isJumpTargets.Length && isJumpTargets[label.Address]) {
								resultType = SearchResultType.JumpTarget;
							}

							int relativeAddress = label.GetRelativeAddress();

							searchResults.Add(new SearchResultInfo() {
								Caption = label.Label,
								AbsoluteAddress = (int)label.Address,
								Value = label.GetValue(),
								MemoryType = label.AddressType,
								SearchResultType = resultType,
								Filename = "",
								Disabled = !_allowOutOfScope && relativeAddress < 0,
								RelativeAddress = relativeAddress,
								CodeLabel = label
							});
						}
					}
				}
			}

			searchResults.Sort((SearchResultInfo a, SearchResultInfo b) => {
				int comparison = a.Disabled.CompareTo(b.Disabled);

				if(comparison == 0) {
					bool aStartsWithSearch = a.Caption.StartsWith(searchString, StringComparison.InvariantCultureIgnoreCase);
					bool bStartsWithSearch = b.Caption.StartsWith(searchString, StringComparison.InvariantCultureIgnoreCase);

					comparison = bStartsWithSearch.CompareTo(aStartsWithSearch);
					if(comparison == 0) {
						comparison = a.Caption.CompareTo(b.Caption);
					}
				}
				return comparison;
			});

			_resultCount = Math.Min(searchResults.Count, MaxResultCount);
			SelectedResult = 0;

			if(searchResults.Count == 0) {
				searchResults.Add(new SearchResultInfo() { Caption = "No results found.", AbsoluteAddress = -1 });
				pnlResults.BackColor = SystemColors.ControlLight;
			} else {
				pnlResults.BackColor = SystemColors.ControlDarkDark;
			}

			if(Program.IsMono) {
				pnlResults.Visible = false;
			} else {
				//Suspend layout causes a crash on Mono
				tlpResults.SuspendLayout();
			}

			for(int i = 0; i < _resultCount; i++) {
				_results[i].Initialize(searchResults[i]);
				_results[i].Tag = searchResults[i];
				_results[i].Visible = true;
			}

			for(int i = searchResults.Count; i < MaxResultCount; i++) {
				_results[i].Visible = false;
			}

			pnlResults.VerticalScroll.Value = 0;
			tlpResults.Height = (_results[0].Height + 1) * _resultCount;
			
			pnlResults.ResumeLayout();
			if(Program.IsMono) {
				pnlResults.Visible = true;
				tlpResults.Width = pnlResults.ClientSize.Width - 17;
			} else {
				tlpResults.ResumeLayout();
				tlpResults.Width = pnlResults.ClientSize.Width - 1;
			}
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			UpdateResults();
		}

		private void SelectAndClose()
		{
			if(_resultCount > 0) {
				SearchResultInfo searchResult = _results[_selectedResult].Tag as SearchResultInfo;
				if(!searchResult.Disabled) {
					AddressTypeInfo addressInfo = new AddressTypeInfo() { Address = searchResult.AbsoluteAddress, Type = searchResult.MemoryType };
					Destination = new GoToDestination() {
						AddressInfo = addressInfo,
						CpuAddress = addressInfo.Address >= 0 ? InteropEmu.DebugGetRelativeAddress((UInt32)addressInfo.Address, addressInfo.Type) : -1,
						Label = searchResult.CodeLabel,
						File = searchResult.Filename,
						Line = searchResult.FileLineNumber
					};
					DialogResult = DialogResult.OK;
					Close();
				}
			}
		}
	}

	public struct GoToDestination
	{
		public CodeLabel Label;
		public AddressTypeInfo AddressInfo;
		public int CpuAddress;
		public string File;
		public int Line;
	}
}
