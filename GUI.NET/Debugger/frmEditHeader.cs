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
using Be.Windows.Forms;
using Mesen.GUI.Controls;
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmEditHeader : BaseConfigForm
	{
		private static Dictionary<UInt64, int> _validSizeValues = new Dictionary<UInt64, int>();

		public frmEditHeader()
		{
			InitializeComponent();

			this.hexBox.Font = new Font(BaseControl.MonospaceFontFamily, 10, FontStyle.Regular);

			NesHeader header = NesHeader.FromBytes(InteropEmu.DebugGetNesHeader());
			Entity = header;

			AddBinding("MapperId", txtMapperId, eNumberFormat.Decimal);
			AddBinding("SubmapperId", txtSubmapperId, eNumberFormat.Decimal);

			AddBinding("Mirroring", cboMirroringType);
			AddBinding("Timing", cboFrameTiming);
			AddBinding("System", cboSystem);
			AddBinding("VsPpu", cboVsPpuType);
			AddBinding("VsSystem", cboVsSystemType);
			AddBinding("InputType", cboInputType);

			AddBinding("HasBattery", chkBattery);
			AddBinding("HasTrainer", chkTrainer);

			AddBinding("PrgRom", txtPrgRomSize, eNumberFormat.Decimal);
			AddBinding("ChrRom", txtChrRomSize, eNumberFormat.Decimal);

			AddBinding("WorkRam", cboWorkRam);
			AddBinding("SaveRam", cboSaveRam);
			AddBinding("ChrRam", cboChrRam);
			AddBinding("ChrRamBattery", cboChrRamBattery);

			AddBinding("IsNes20", radNes2, radiNes);

			UpdateUI();
			UpdateVsDropdown();

			_validSizeValues = new Dictionary<UInt64, int>();
			for(int i = 0; i < 256; i++) {
				int multiplier = (i & 0x03) * 2 + 1;
				UInt64 value = ((UInt64)1 << (i >> 2)) / 1024;
				_validSizeValues[(UInt64)multiplier * value] = i;
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if(this.DialogResult == DialogResult.OK) {
				using(SaveFileDialog sfd = new SaveFileDialog()) {
					sfd.SetFilter("NES roms (*.nes)|*.nes");
					sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".nes";
					if(sfd.ShowDialog() == DialogResult.OK) {
						InteropEmu.DebugSaveRomToDisk(sfd.FileName, false, ((NesHeader)Entity).ToBytes());
					} else {
						e.Cancel = true;
					}
				}
			}
			base.OnFormClosing(e);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
			DebugWindowManager.CleanupDebugger();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			btnOK.Text = "Save As";
			btnOK.Image = Mesen.GUI.Properties.Resources.Floppy;
			btnOK.TextImageRelation = TextImageRelation.ImageBeforeText;
			btnOK.AutoSize = true;
		}

		protected override bool ValidateInput()
		{
			UpdateObject();

			NesHeader header = Entity as NesHeader;

			bool isValidPrgSize = header.IsNes20 && _validSizeValues.ContainsKey(header.PrgRom);
			bool isValidChrSize = header.IsNes20 && _validSizeValues.ContainsKey(header.ChrRom);

			if(!isValidPrgSize && (header.PrgRom % 16) != 0) {
				lblError.Text = "Error: PRG ROM size must be a multiple of 16 KB";
				lblError.Visible = true;
				return false;
			}
			if(!isValidChrSize && (header.ChrRom % 8) != 0) {
				lblError.Text = "Error: CHR ROM size must be a multiple of 8 KB";
				lblError.Visible = true;
				return false;
			}

			if(header.IsNes20) {
				if(header.MapperId >= 4096) {
					lblError.Text = "Error: Mapper ID must be lower than 4096";
					lblError.Visible = true;
					return false;
				}
				if(header.SubmapperId >= 16) {
					lblError.Text = "Error: Submapper ID must be lower than 16";
					lblError.Visible = true;
					return false;
				}
				if(!isValidChrSize && header.ChrRom >= 16384) {
					lblError.Text = "Error: CHR ROM size must be lower than 16384 KB";
					lblError.Visible = true;
					return false;
				}
				if(!isValidPrgSize && header.PrgRom >= 32768) {
					lblError.Text = "Error: PRG ROM size must be lower than 32768 KB";
					lblError.Visible = true;
					return false;
				}
			} else {
				if(header.MapperId >= 256) {
					lblError.Text = "Error: Mapper ID must be lower than 256 ";
					lblError.Visible = true;
					return false;
				}
				if(header.ChrRom >= 2048) {
					lblError.Text = "Error: CHR ROM size must be lower than 2048 KB";
					lblError.Visible = true;
					return false;
				}
				if(header.PrgRom >= 4096) {
					lblError.Text = "Error: PRG ROM size must be lower than 4096 KB";
					lblError.Visible = true;
					return false;
				}
			}
			lblError.Visible = false;

			hexBox.ByteProvider = new StaticByteProvider(header.ToBytes());

			return base.ValidateInput();
		}

		private void cboSaveRam_SelectedIndexChanged(object sender, EventArgs e)
		{
			chkBattery.Enabled = cboSaveRam.SelectedIndex == 0 && cboChrRamBattery.SelectedIndex == 0;
			if(!chkBattery.Enabled) {
				chkBattery.Checked = true;
			}
		}

		private void UpdateVsDropdown()
		{
			bool isVsSystem = cboSystem.GetEnumValue<TvSystem>() == TvSystem.VsSystem;
			cboVsPpuType.Enabled = isVsSystem && radNes2.Checked;
			cboVsSystemType.Enabled = isVsSystem && radNes2.Checked;
			if(!cboVsPpuType.Enabled) {
				cboVsPpuType.SelectedIndex = 0;
				cboVsSystemType.SelectedIndex = 0;
			}
		}

		private void cboSystem_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateVsDropdown();
		}

		private void radVersion_CheckedChanged(object sender, EventArgs e)
		{
			cboChrRam.Enabled = radNes2.Checked;
			cboChrRamBattery.Enabled = radNes2.Checked;
			cboSaveRam.Enabled = radNes2.Checked;
			cboWorkRam.Enabled = radNes2.Checked;
			cboInputType.Enabled = radNes2.Checked;
			txtSubmapperId.Enabled = radNes2.Checked;
			UpdateVsDropdown();

			Enum[] hiddenSystems = null;
			Enum[] hiddenTimings = null;
			if(!radNes2.Checked) {
				hiddenSystems = new Enum[] {
					TvSystem.BitCorporationCreator, TvSystem.Vt01Mono, TvSystem.Vt01RedCyan,
					TvSystem.Vt02, TvSystem.Vt03, TvSystem.Vt09, TvSystem.Vt36x, TvSystem.Vt3x
				};
				hiddenTimings = new Enum[] {
					FrameTiming.Dendy
				};
			}
			BaseConfigForm.InitializeComboBox(cboSystem, typeof(TvSystem), hiddenSystems);
			if(cboSystem.SelectedIndex < 0) {
				cboSystem.SelectedIndex = 0;
			}

			BaseConfigForm.InitializeComboBox(cboFrameTiming, typeof(FrameTiming), hiddenTimings);
			if(cboFrameTiming.SelectedIndex < 0) {
				cboFrameTiming.SelectedIndex = 0;
			}

			if(!cboChrRam.Enabled) {
				cboChrRam.SelectedIndex = 0;
			}
			if(!cboChrRamBattery.Enabled) {
				cboChrRamBattery.SelectedIndex = 0;
			}
			if(!cboSaveRam.Enabled) {
				cboSaveRam.SelectedIndex = 0;
			}
			if(!cboWorkRam.Enabled) {
				cboWorkRam.SelectedIndex = 0;
			}
			if(!txtSubmapperId.Enabled) {
				txtSubmapperId.Text = "0";
			}
		}

		private class NesHeader
		{
			public bool IsNes20;

			public uint MapperId;
			public uint SubmapperId;

			public UInt64 PrgRom;
			public UInt64 ChrRom;

			public iNesMirroringType Mirroring;

			public FrameTiming Timing;
			public TvSystem System;
			public bool HasTrainer;
			public bool HasBattery;
			public VsPpuType VsPpu;
			public VsSystemType VsSystem;
			public GameInputType InputType;

			public MemorySizes WorkRam = MemorySizes.None;
			public MemorySizes SaveRam = MemorySizes.None;
			public MemorySizes ChrRam = MemorySizes.None;
			public MemorySizes ChrRamBattery = MemorySizes.None;

			public byte[] ToBytes()
			{
				byte[] header = new byte[16];
				header[0] = 0x4E;
				header[1] = 0x45;
				header[2] = 0x53;
				header[3] = 0x1A;

				UInt64 prgRomValue = PrgRom / 16;
				UInt64 chrRomValue = ChrRom / 8;

				if(IsNes20) {
					if((PrgRom % 16) != 0 || PrgRom >= 32768) {
						if(_validSizeValues.ContainsKey(PrgRom)) {
							//This value is a valid exponent+multiplier combo (NES 2.0 only)
							prgRomValue = ((uint)_validSizeValues[PrgRom] & 0xFF) | 0xF00;
						}
					}

					if((ChrRom % 8) != 0 || ChrRom >= 16384) {
						if(_validSizeValues.ContainsKey(ChrRom)) {
							//This value is a valid exponent+multiplier combo (NES 2.0 only)
							chrRomValue = ((uint)_validSizeValues[ChrRom] & 0xFF) | 0xF00;
						}
					}

					//NES 2.0
					header[4] = (byte)(prgRomValue);
					header[5] = (byte)(chrRomValue);

					header[6] = (byte)(
						((byte)(MapperId & 0x0F) << 4) |
						(byte)Mirroring | (HasTrainer ? 0x04 : 0x00) | (HasBattery ? 0x02 : 0x00)
					);

					header[7] = (byte)(MapperId & 0xF0);

					switch(System) {
						case TvSystem.NesFamicomDendy: header[7] |= 0x00; break;
						case TvSystem.VsSystem: header[7] |= 0x01; break;
						case TvSystem.Playchoice: header[7] |= 0x02; break;
						default: header[7] |= 0x03; break;
					}

					//Enable NES 2.0 header
					header[7] |= 0x08;

					header[8] = (byte)(((SubmapperId & 0x0F) << 4) | ((MapperId & 0xF00) >> 8));
					header[9] = (byte)(((prgRomValue & 0xF00) >> 8) | ((chrRomValue & 0xF00) >> 4));

					header[10] = (byte)((byte)WorkRam | ((byte)SaveRam) << 4);
					header[11] = (byte)((byte)ChrRam | ((byte)ChrRamBattery) << 4);

					switch(Timing) {
						default:
						case FrameTiming.Ntsc: header[12] = 0x00; break;
						case FrameTiming.Pal: header[12] = 0x01; break;
						case FrameTiming.NtscAndPal: header[12] = 0x02; break;
						case FrameTiming.Dendy: header[12] = 0x03; break;
					}

					if(System == TvSystem.VsSystem) {
						header[13] = (byte)(((byte)VsPpu & 0x0F) | ((((byte)VsSystem) & 0x0F) << 4));
					} else {
						switch(System) {
							default:
							case TvSystem.NesFamicomDendy: header[13] = 0x00; break;

							case TvSystem.Playchoice: header[13] = 0x02; break;
							case TvSystem.BitCorporationCreator: header[13] = 0x03; break;
							case TvSystem.Vt01Mono: header[13] = 0x04; break;
							case TvSystem.Vt01RedCyan: header[13] = 0x05; break;
							case TvSystem.Vt02: header[13] = 0x06; break;
							case TvSystem.Vt03: header[13] = 0x07; break;
							case TvSystem.Vt09: header[13] = 0x08; break;
							case TvSystem.Vt3x: header[13] = 0x09; break;
							case TvSystem.Vt36x: header[13] = 0x0A; break;
						}
					}
					header[14] = 0;
					header[15] = (byte)InputType;
				} else {
					//iNES
					if(prgRomValue == 0x100) {
						header[4] = 0;
					} else {
						header[4] = (byte)(prgRomValue);
					}
					header[5] = (byte)(chrRomValue);

					header[6] = (byte)(
						((byte)(MapperId & 0x0F) << 4) |
						(byte)Mirroring | (HasTrainer ? 0x04 : 0x00) | (HasBattery ? 0x02 : 0x00)
					);
					header[7] =  (byte)(
						((byte)MapperId & 0xF0) |
						(byte)(System == TvSystem.VsSystem ? 0x01 : 0x00) | (System == TvSystem.Playchoice ? 0x02 : 0x00)
					);

					header[8] = 0;
					header[9] = (byte)(Timing == FrameTiming.Pal ? 0x01 : 0x00);
					header[10] = 0;
					header[11] = 0;
					header[12] = 0;
					header[13] = 0;
					header[14] = 0;
					header[15] = 0;
				}

				return header;
			}

			public static NesHeader FromBytes(byte[] bytes)
			{
				BinaryHeader binHeader = new BinaryHeader(bytes);

				NesHeader header = new NesHeader();
				header.IsNes20 = binHeader.GetRomHeaderVersion() == RomHeaderVersion.Nes2_0;
				header.PrgRom = (uint)(binHeader.GetPrgSize());
				header.ChrRom = (uint)(binHeader.GetChrSize());
				header.HasTrainer = binHeader.HasTrainer();
				header.HasBattery = binHeader.HasBattery();

				header.System = binHeader.GetTvSystem();
				header.Timing = binHeader.GetFrameTiming(); 

				header.Mirroring = binHeader.GetMirroringType();
				header.MapperId = (uint)binHeader.GetMapperID();
				header.SubmapperId = (uint)binHeader.GetSubMapper();
				header.WorkRam = (MemorySizes)binHeader.GetWorkRamSize();
				header.SaveRam = (MemorySizes)binHeader.GetSaveRamSize();
				header.ChrRam = (MemorySizes)binHeader.GetChrRamSize();
				header.ChrRamBattery = (MemorySizes)binHeader.GetSaveChrRamSize();
				header.InputType = binHeader.GetInputType();
				header.VsPpu = (VsPpuType)bytes[13];
				header.VsSystem = binHeader.GetVsSystemType();

				return header;
			}
		}

		private class BinaryHeader
		{
			private byte[] _bytes;
			private byte PrgCount;
			private byte ChrCount;

			public BinaryHeader(byte[] bytes)
			{
				_bytes = bytes;
				PrgCount = bytes[4];
				ChrCount = bytes[5];
			}

			public RomHeaderVersion GetRomHeaderVersion()
			{
				if((_bytes[7] & 0x0C) == 0x08) {
					return RomHeaderVersion.Nes2_0;
				} else if((_bytes[7] & 0x0C) == 0x00) {
					return RomHeaderVersion.iNes;
				} else {
					return RomHeaderVersion.OldiNes;
				}
			}

			public int GetMapperID()
			{
				switch(GetRomHeaderVersion()) {
					case RomHeaderVersion.Nes2_0:
						return ((_bytes[8] & 0x0F) << 8) | (_bytes[7] & 0xF0) | (_bytes[6] >> 4);
					default:
					case RomHeaderVersion.iNes:
						return (_bytes[7] & 0xF0) | (_bytes[6] >> 4);
					case RomHeaderVersion.OldiNes:
						return (_bytes[6] >> 4);
				}
			}

			public bool HasBattery()
			{
				return (_bytes[6] & 0x02) == 0x02;
			}

			public bool HasTrainer()
			{
				return (_bytes[6] & 0x04) == 0x04;
			}

			public FrameTiming GetFrameTiming()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					switch(_bytes[12] & 0x03) {
						case 0: return FrameTiming.Ntsc;
						case 1: return FrameTiming.Pal;
						case 2: return FrameTiming.NtscAndPal;
						case 3: return FrameTiming.Dendy;
					}
				} else if(GetRomHeaderVersion() == RomHeaderVersion.iNes) {
					return (_bytes[9] & 0x01) == 0x01 ? FrameTiming.Pal : FrameTiming.Ntsc;
				}
				return FrameTiming.Ntsc;
			}

			public TvSystem GetTvSystem()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					switch(_bytes[7] & 0x03) {
						case 0: return TvSystem.NesFamicomDendy;
						case 1: return TvSystem.VsSystem;
						case 2: return TvSystem.Playchoice;
						case 3:
							switch(_bytes[13]) {
								case 0: return TvSystem.NesFamicomDendy;
								case 1: return TvSystem.VsSystem;
								case 2: return TvSystem.Playchoice;
								case 3: return TvSystem.BitCorporationCreator;
								case 4: return TvSystem.Vt01Mono;
								case 5: return TvSystem.Vt01RedCyan;
								case 6: return TvSystem.Vt02;
								case 7: return TvSystem.Vt03;
								case 8: return TvSystem.Vt09;
								case 9: return TvSystem.Vt3x;
								case 10: return TvSystem.Vt36x;
								default: return TvSystem.NesFamicomDendy;
							}
					}
				} else if(GetRomHeaderVersion() == RomHeaderVersion.iNes) {
					if((_bytes[7] & 0x01) == 0x01) {
						return TvSystem.VsSystem;
					} else if((_bytes[7] & 0x02) == 0x02) {
						return TvSystem.Playchoice;
					}
				}
				return TvSystem.NesFamicomDendy;
			}

			private UInt64 GetSizeValue(int exponent, int multiplier)
			{
				multiplier = multiplier * 2 + 1;
				return (UInt64)multiplier * (((UInt64)1 << exponent) / 1024);
			}

			public UInt64 GetPrgSize()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					if((_bytes[9] & 0x0F) == 0x0F) {
						return GetSizeValue(PrgCount >> 2, PrgCount & 0x03);
					} else {
						return (UInt64)(((_bytes[9] & 0x0F) << 8) | PrgCount) * 16;
					}
				} else {
					if(PrgCount == 0) {
						return 256 * 16; //0 is a special value and means 256
					} else {
						return (UInt64)PrgCount * 16;
					}
				}
			}

			public UInt64 GetChrSize()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					if((_bytes[9] & 0xF0) == 0xF0) {
						return GetSizeValue(ChrCount >> 2, ChrCount & 0x03);
					} else {
						return (UInt64)(((_bytes[9] & 0xF0) << 4) | ChrCount) * 8;
					}
				} else {
					return (UInt64)ChrCount * 8;
				}
			}

			public int GetWorkRamSize()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					return _bytes[10] & 0x0F;
				} else {
					return 0;
				}
			}

			public int GetSaveRamSize()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					return (_bytes[10] & 0xF0) >> 4;
				} else {
					return 0;
				}
			}

			public int GetChrRamSize()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					return _bytes[11] & 0x0F;
				} else {
					return 0;
				}
			}

			public int GetSaveChrRamSize()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					return (_bytes[11] & 0xF0) >> 4;
				} else {
					return 0;
				}
			}

			public int GetSubMapper()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					return (_bytes[8] & 0xF0) >> 4;
				} else {
					return 0;
				}
			}

			public iNesMirroringType GetMirroringType()
			{
				if((_bytes[6] & 0x08) != 0) {
					return iNesMirroringType.FourScreens;
				} else {
					return (_bytes[6] & 0x01) != 0 ? iNesMirroringType.Vertical : iNesMirroringType.Horizontal;
				}
			}

			public GameInputType GetInputType()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					if(_bytes[15] < Enum.GetValues(typeof(GameInputType)).Length) {
						return (GameInputType)_bytes[15];
					}
					return GameInputType.Default;
				} else {
					return GameInputType.Default;
				}
			}

			public VsSystemType GetVsSystemType()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					if((_bytes[13] >> 4) <= 0x06) {
						return (VsSystemType)(_bytes[13] >> 4);
					}
				}
				return VsSystemType.Default;
			}
		}

		private enum RomHeaderVersion
		{
			iNes = 0,
			Nes2_0 = 1,
			OldiNes = 2
		}

		private enum iNesMirroringType
		{
			Horizontal = 0,
			Vertical = 1,
			FourScreens = 8
		}

		private enum FrameTiming
		{
			Ntsc = 0,
			Pal = 1,
			NtscAndPal = 2,
			Dendy = 3
		}

		private enum TvSystem
		{
			NesFamicomDendy,
			VsSystem,
			Playchoice,
			BitCorporationCreator,
			Vt01Mono,
			Vt01RedCyan,
			Vt02,
			Vt03,
			Vt09,
			Vt3x,
			Vt36x
		}

		private enum MemorySizes
		{
			None = 0,
			_128Bytes = 1,
			_256Bytes = 2,
			_512Bytes = 3,
			_1KB = 4,
			_2KB = 5,
			_4KB = 6,
			_8KB = 7,
			_16KB = 8,
			_32KB = 9,
			_64KB = 10,
			_128KB = 11,
			_256KB = 12,
			_512KB = 13,
			_1024KB = 14,
			Reserved = 15
		}

		private enum VsPpuType
		{
			RP2C03B = 0,
			RP2C03G = 1,
			RP2C040001 = 2,
			RP2C040002 = 3,
			RP2C040003 = 4,
			RP2C040004 = 5,
			RC2C03B = 6,
			RC2C03C = 7,
			RC2C0501 = 8,
			RC2C0502 = 9,
			RC2C0503 = 10,
			RC2C0504 = 11,
			RC2C0505 = 12,
			Undefined = 13,
			Undefined2 = 14,
			Undefined3 = 15
		}
		
		private enum VsSystemType
		{
			Default = 0,
			RbiBaseballProtection = 1,
			TkoBoxingProtection = 2,
			SuperXeviousProtection = 3,
			IceClimberProtection = 4,
			VsDualSystem = 5,
			RaidOnBungelingBayProtection = 6,
		}

		private enum GameInputType
		{
			Default = 0,
			FamicomControllers = 1,
			FourScore = 2,
			FourPlayerAdapter = 3,
			VsSystem = 4,
			VsSystemSwapped = 5,
			VsSystemSwapAB = 6,
			VsZapper = 7,
			Zapper = 8,
			TwoZappers = 9,
			BandaiHypershot = 0x0A,
			PowerPadSideA = 0x0B,
			PowerPadSideB = 0x0C,
			FamilyTrainerSideA = 0x0D,
			FamilyTrainerSideB = 0x0E,
			ArkanoidControllerNes = 0x0F,
			ArkanoidControllerFamicom = 0x10,
			DoubleArkanoidController = 0x11,
			KonamiHyperShot = 0x12,
			PachinkoController = 0x13,
			ExcitingBoxing = 0x14,
			JissenMahjong = 0x15,
			PartyTap = 0x16,
			OekaKidsTablet = 0x17,
			BarcodeBattler = 0x18,
			MiraclePiano = 0x19, //not supported yet
			PokkunMoguraa = 0x1A, //not supported yet
			TopRider = 0x1B, //not supported yet
			DoubleFisted = 0x1C, //not supported yet
			Famicom3dSystem = 0x1D, //not supported yet
			DoremikkoKeyboard = 0x1E, //not supported yet
			ROB = 0x1F, //not supported yet
			FamicomDataRecorder = 0x20,
			TurboFile = 0x21,
			BattleBox = 0x22,
			FamilyBasicKeyboard = 0x23,
			Pec586Keyboard = 0x24, //not supported yet
			Bit79Keyboard = 0x25, //not supported yet
			SuborKeyboard = 0x26,
			SuborKeyboardMouse1 = 0x27,
			SuborKeyboardMouse2 = 0x28,
			SnesMouse = 0x29,
			GenericMulticart = 0x2A, //not supported yet
			SnesControllers = 0x2B,
		}
	}
}
