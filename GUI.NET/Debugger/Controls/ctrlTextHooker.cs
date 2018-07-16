using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Mesen.GUI.Config;
using Mesen.GUI.Controls;
using Mesen.GUI.Forms;
using System.Collections.Concurrent;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlTextHooker : BaseControl
	{
		private byte[][] _nametablePixelData = new byte[4][];
		private byte[][] _tileData = new byte[4][];
		private byte[][] _attributeData = new byte[4][];
		private byte[] _tmpTileData = new byte[16];

		private Bitmap _nametableImage = new Bitmap(512, 480);
		private Bitmap _outputImage = new Bitmap(512, 480);
		private int _xScroll = 0;
		private int _yScroll = 0;
		private DebugState _state = new DebugState();
		private ConcurrentDictionary<string, string> _charMappings;

		public ctrlTextHooker()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(!IsDesignMode) {
				DebugInfo debugInfo = ConfigManager.Config.DebugInfo;
				chkIgnoreMirroredNametables.Checked = debugInfo.TextHookerIgnoreMirroredNametables;
				chkUseScrollOffsets.Checked = debugInfo.TextHookerAdjustViewportScrolling;
				chkAutoCopyToClipboard.Checked = debugInfo.TextHookerAutoCopyToClipboard;

				BaseConfigForm.InitializeComboBox(cboDakutenMode, typeof(DakutenMode));
				cboDakutenMode.SetEnumValue(debugInfo.TextHookerDakutenMode);
			}
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);

			if(!IsDesignMode) {
				DebugInfo debugInfo = ConfigManager.Config.DebugInfo;
				debugInfo.TextHookerIgnoreMirroredNametables = chkIgnoreMirroredNametables.Checked;
				debugInfo.TextHookerAdjustViewportScrolling = chkUseScrollOffsets.Checked;
				debugInfo.TextHookerAutoCopyToClipboard = chkAutoCopyToClipboard.Checked;
				debugInfo.TextHookerDakutenMode = cboDakutenMode.GetEnumValue<DakutenMode>();
			}
		}

		public void GetData()
		{
			InteropEmu.DebugGetPpuScroll(out _xScroll, out _yScroll);
			InteropEmu.DebugGetState(ref _state);

			for(int i = 0; i < 4; i++) {
				InteropEmu.DebugGetNametable(i, ConfigManager.Config.DebugInfo.NtViewerUseGrayscalePalette, out _nametablePixelData[i], out _tileData[i], out _attributeData[i]);
			}

			InteropEmu.DebugGetPpuScroll(out _xScroll, out _yScroll);
			_xScroll &= 0xFFF8;
			_yScroll &= 0xFFF8;
		}

		public void RefreshViewer()
		{
			using(Graphics gNametable = Graphics.FromImage(_nametableImage)) {
				for(int i = 0; i < 4; i++) {
					GCHandle handle = GCHandle.Alloc(_nametablePixelData[i], GCHandleType.Pinned);
					Bitmap source = new Bitmap(256, 240, 4*256, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
					try {
						gNametable.DrawImage(source, new Rectangle(i % 2 == 0 ? 0 : 256, i <= 1 ? 0 : 240, 256, 240), new Rectangle(0, 0, 256, 240), GraphicsUnit.Pixel);
					} finally {
						handle.Free();
					}
				}
			}

			using(Graphics g = Graphics.FromImage(_outputImage)) {
				if(chkUseScrollOffsets.Checked) {
					g.DrawImage(_nametableImage, -_xScroll, -_yScroll);
					g.DrawImage(_nametableImage, -_xScroll + 512, -_yScroll + 480);
					g.DrawImage(_nametableImage, -_xScroll + 512, -_yScroll);
					g.DrawImage(_nametableImage, -_xScroll, -_yScroll + 480);
				} else {
					g.DrawImage(_nametableImage, 0, 0);
				}
			}
			picNametable.Image = _outputImage;

			DakutenMode dakutenMode = cboDakutenMode.GetEnumValue<DakutenMode>();
			StringBuilder output = new StringBuilder();
			DakutenType[] previousLineDakutenType = new DakutenType[32];
			for(int nt = 0; nt < 4; nt++) {
				for(int y = 0; y < 30; y++) {
					StringBuilder lineOutput = new StringBuilder();
					for(int x = 0; x < 32; x++) {
						int outNt, outY, outX;
						GetIndexes(nt, y, x, out outNt, out outY, out outX);
						if(IgnoreTile(outNt)) {
							continue;
						}

						string key = GetTileKey(outNt, (outY << 5) + outX);
						string value = GetMappedCharacter(key);

						DakutenType dakutenType = GetDakutenType(value);
						if(dakutenType != DakutenType.None) {
							previousLineDakutenType[x] = dakutenType;
						} else {
							DakutenType effectiveDakuten = dakutenMode == DakutenMode.OnTop ? previousLineDakutenType[x] : DakutenType.None;
							previousLineDakutenType[x] = DakutenType.None;

							if(effectiveDakuten == DakutenType.None && dakutenMode == DakutenMode.OnTheRight) {
								GetIndexes(nt, y, x + 1, out outNt, out outY, out outX);
								string nextTileKey = GetTileKey(outNt, (outY << 5) + outX);
								string nextTileValue = GetMappedCharacter(nextTileKey);
								effectiveDakuten = GetDakutenType(nextTileValue);
							}

							bool isKana = (
								(value[0] >= '\x3041' && value[0] <= '\x3096') || //hiragana
								(value[0] >= '\x30A1' && value[0] <= '\x30FA') //katakana
							);

							if(isKana && effectiveDakuten == DakutenType.Dakuten) {
								lineOutput.Append((char)(value[0] + 1));
							} else if(isKana && effectiveDakuten == DakutenType.Handakuten) {
								lineOutput.Append((char)(value[0] + 2));
							} else {
								lineOutput.Append(value);
							}
						}
					}

					string rowString = lineOutput.ToString().Trim();
					if(rowString.Length > 0) {
						output.AppendLine(rowString);
					}
				}
			}

			txtSelectedText.Text = output.ToString();
			if(chkAutoCopyToClipboard.Checked && !string.IsNullOrWhiteSpace(txtSelectedText.Text)) {
				Clipboard.SetText(txtSelectedText.Text);
			}
		}

		private string GetMappedCharacter(string key)
		{
			string value;
			if(this._charMappings.TryGetValue(key, out value)) {
				return value;
			} else {
				return " ";
			}
		}

		private DakutenType GetDakutenType(string value)
		{
			if(value == "daku" || value == "ﾞ") {
				return DakutenType.Dakuten;
			} else if(value == "han" || value == "ﾟ") {
				return DakutenType.Handakuten;
			} else {
				return DakutenType.None;
			}
		}

		private string GetTileKey(int nametableIndex, int index)
		{
			byte tileIndex = _tileData[nametableIndex][index];

			for(int i = 0; i < 16; i++) {
				_tmpTileData[i] = InteropEmu.DebugGetMemoryValue(DebugMemoryType.PpuMemory, (UInt32)(_state.PPU.ControlFlags.BackgroundPatternAddr + tileIndex * 16 + i));
			}

			return ctrlCharacterMapping.GetColorIndependentKey(_tmpTileData);
		}

		private bool IgnoreTile(int nametableIndex)
		{
			if(chkIgnoreMirroredNametables.Checked) {
				switch(_state.Cartridge.Mirroring) {
					case MirroringType.ScreenAOnly:
					case MirroringType.ScreenBOnly:
						if(nametableIndex > 0) {
							return true;
						}
						break;

					case MirroringType.Horizontal:
						if((nametableIndex & 0x01) == 0x01) {
							return true;
						}
						break;

					case MirroringType.Vertical:
						if((nametableIndex & 0x02) == 0x02) {
							return true;
						}
						break;
				}
			}
			return false;
		}

		public void SetCharacterMappings(ConcurrentDictionary<string, string> charMappings)
		{
			_charMappings = charMappings;
		}

		private void GetIndexes(int inNt, int inY, int inX, out int outNt, out int outY, out int outX)
		{
			outX = inX;
			outY = inY;
			outNt = inNt & 0x03;
			
			if(chkUseScrollOffsets.Checked) {
				outY += _yScroll / 8;
				outX += _xScroll / 8;
			}

			while(outX < 0) {
				outX += 32;
				outNt ^= 1;
			}
			
			while(outX >= 32) {
				outX -= 32;
				outNt ^= 1;
			}

			while(outY >= 30) {
				outY -= 30;
				outNt ^= 2;
			}

			while(outY < 0) {
				outY += 30;
				outNt ^= 2;
			}

			outNt &= 0x03;
		}
	}

	enum DakutenType {
		None = 0,
		Dakuten = 1,
		Handakuten = 2
	}
}
