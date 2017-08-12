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
using Mesen.GUI.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmEditHeader : BaseConfigForm
	{
		public frmEditHeader()
		{
			InitializeComponent();

			NesHeader header = NesHeader.FromBytes(InteropEmu.DebugGetNesHeader());
			Entity = header;

			AddBinding("MapperId", txtMapperId, eNumberFormat.Decimal);
			AddBinding("SubmapperId", txtSubmapperId, eNumberFormat.Decimal);

			AddBinding("Mirroring", cboMirroringType);
			AddBinding("System", cboSystem);
			AddBinding("VsPpu", cboVsPpuType);

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
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if(this.DialogResult == DialogResult.OK) {
				using(SaveFileDialog sfd = new SaveFileDialog()) {
					sfd.Filter = "NES roms (*.nes)|*.nes";
					sfd.FileName = InteropEmu.GetRomInfo().GetRomName() + ".nes";
					if(sfd.ShowDialog() == DialogResult.OK) {
						InteropEmu.DebugSaveRomToDisk(sfd.FileName, ((NesHeader)Entity).ToBytes());
					} else {
						e.Cancel = true;
					}
				}
			}
			base.OnFormClosing(e);
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
			if((header.PrgRom % 16) != 0) {
				lblError.Text = "Error: PRG ROM size must be a multiple of 16 KB";
				lblError.Visible = true;
				return false;
			}
			if((header.ChrRom % 8) != 0) {
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
				if(header.ChrRom >= 32768) {
					lblError.Text = "Error: CHR ROM size must be lower than 32768 KB";
					lblError.Visible = true;
					return false;
				}
				if(header.PrgRom >= 65536) {
					lblError.Text = "Error: PRG ROM size must be lower than 65536 KB";
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
			cboVsPpuType.Visible = isVsSystem;
			lblVsPpuType.Visible = isVsSystem;
			if(!isVsSystem) {
				cboVsPpuType.SelectedIndex = 0;
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
			cboVsPpuType.Enabled = radNes2.Checked;
			txtSubmapperId.Enabled = radNes2.Checked;

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
			if(!cboVsPpuType.Enabled) {
				cboVsPpuType.SelectedIndex = 0;
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

			public uint PrgRom;
			public uint ChrRom;

			public MirroringType Mirroring;

			public TvSystem System;
			public bool HasTrainer;
			public bool HasBattery;
			public VsPpuType VsPpu;

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

				uint prgRomValue = PrgRom / 16;
				uint chrRomValue = ChrRom / 8;

				if(IsNes20) {
					//NES 2.0
					header[4] = (byte)(prgRomValue);
					header[5] = (byte)(chrRomValue);

					header[6] = (byte)(
						((byte)(MapperId & 0x0F) << 4) |
						(byte)Mirroring | (HasTrainer ? 0x04 : 0x00) | (HasBattery ? 0x02 : 0x00)
					);

					header[7] =  (byte)(
						((byte)MapperId & 0xF0) |
						(byte)(System == TvSystem.VsSystem ? 0x01 : 0x00) | (System == TvSystem.Playchoice ? 0x02 : 0x00) |
						0x08 //Enable NES 2.0 header
					);

					header[8] = (byte)(((SubmapperId & 0x0F) << 4) | ((MapperId & 0xF00) >> 8));
					header[9] = (byte)(((prgRomValue & 0xF00) >> 8) | ((chrRomValue & 0xF00) >> 4));

					header[10] = (byte)((byte)WorkRam | ((byte)SaveRam) << 4);
					header[11] = (byte)((byte)ChrRam | ((byte)ChrRamBattery) << 4);

					switch(System) {
						default:
						case TvSystem.Ntsc:
							header[12] = 0;
							break;
						case TvSystem.Pal:
							header[12] = 1;
							break;
						case TvSystem.NtscAndPal:
							header[12] = 2;
							break;
					}

					header[13] = (byte)VsPpu;
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
					header[9] = (byte)(System == TvSystem.Pal ? 0x01 : 0x00);
					header[10] = 0;
					header[11] = 0;
					header[12] = 0;
					header[13] = 0;
				}

				//Reserved bytes
				header[14] = 0;
				header[15] = 0;

				return header;
			}

			public static NesHeader FromBytes(byte[] bytes)
			{
				BinaryHeader binHeader = new BinaryHeader(bytes);

				NesHeader header = new NesHeader();
				header.IsNes20 = binHeader.GetRomHeaderVersion() == RomHeaderVersion.Nes2_0;
				header.PrgRom = (uint)(binHeader.GetPrgSize() * 16);
				header.ChrRom = (uint)(binHeader.GetChrSize() * 8);
				header.HasTrainer = binHeader.HasTrainer();
				header.HasBattery = binHeader.HasBattery();
				if(binHeader.IsVsSystem()) {
					header.System = TvSystem.VsSystem;
				} else if(binHeader.IsPlaychoice()) {
					header.System = TvSystem.Playchoice;
				} else if(binHeader.IsPalRom()) {
					header.System = TvSystem.Pal;
				} else {
					header.System = TvSystem.Ntsc;
				}

				header.Mirroring = binHeader.GetMirroringType();
				header.MapperId = (uint)binHeader.GetMapperID();
				header.SubmapperId = (uint)binHeader.GetSubMapper();
				header.WorkRam = (MemorySizes)binHeader.GetWorkRamSize();
				header.SaveRam = (MemorySizes)binHeader.GetSaveRamSize();
				header.ChrRam = (MemorySizes)binHeader.GetChrRamSize();
				header.ChrRamBattery = (MemorySizes)binHeader.GetSaveChrRamSize();
				header.VsPpu = (VsPpuType)bytes[13];

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
						return ((_bytes[8] & 0x0F) << 4) | (_bytes[7] & 0xF0) | (_bytes[6] >> 4);
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

			public bool IsPalRom()
			{
				switch(GetRomHeaderVersion()) {
					case RomHeaderVersion.Nes2_0:
						return (_bytes[12] & 0x01) == 0x01;
					case RomHeaderVersion.iNes:
						return (_bytes[9] & 0x01) == 0x01;
					default:
						return false;
				}
			}

			public bool IsPlaychoice()
			{
				switch(GetRomHeaderVersion()) {
					case RomHeaderVersion.Nes2_0:
					case RomHeaderVersion.iNes:
						return (_bytes[7] & 0x02) == 0x02;
					default:
						return false;
				}
			}

			public bool IsVsSystem()
			{
				switch(GetRomHeaderVersion()) {
					case RomHeaderVersion.Nes2_0:
					case RomHeaderVersion.iNes:
						return (_bytes[7] & 0x01) == 0x01;
					default:
						return false;
				}
			}

			public int GetPrgSize()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					return (((_bytes[9] & 0x0F) << 8) | PrgCount);
				} else {
					if(PrgCount == 0) {
						return 256; //0 is a special value and means 256
					} else {
						return PrgCount;
					}
				}
			}

			public int GetChrSize()
			{
				if(GetRomHeaderVersion() == RomHeaderVersion.Nes2_0) {
					return (((_bytes[9] & 0xF0) << 4) | ChrCount);
				} else {
					return ChrCount;
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

			public MirroringType GetMirroringType()
			{
				if((_bytes[6] & 0x08) != 0) {
					return MirroringType.FourScreens;
				} else {
					return (_bytes[6] & 0x01) != 0 ? MirroringType.Vertical : MirroringType.Horizontal;
				}
			}
		}

		private enum RomHeaderVersion
		{
			iNes = 0,
			Nes2_0 = 1,
			OldiNes = 2
		}

		private enum MirroringType
		{
			Horizontal = 0,
			Vertical = 1,
			FourScreens = 8
		}

		private enum TvSystem
		{
			Ntsc = 0,
			Pal = 1,
			NtscAndPal = 2,
			VsSystem = 3,
			Playchoice = 4,
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
	}
}
