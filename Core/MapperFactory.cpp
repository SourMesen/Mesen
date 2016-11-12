#include "stdafx.h"
#include "MessageManager.h"
#include "MapperFactory.h"
#include "RomLoader.h"
#include "UnifBoards.h"

#include "A65AS.h"
#include "Ac08.h"
#include "Action53.h"
#include "ActionEnterprises.h"
#include "Ax5705.h"
#include "AXROM.h"
#include "Bandai74161_7432.h"
#include "BandaiFcg.h"
#include "BandaiKaraoke.h"
#include "Bb.h"
#include "BF909x.h"
#include "BF9096.h"
#include "Bmc11160.h"
#include "Bmc12in1.h"
#include "Bmc51.h"
#include "Bmc63.h"
#include "Bmc64in1NoRepeat.h"
#include "Bmc70in1.h"
#include "Bmc190in1.h"
#include "Bmc235.h"
#include "Bmc255.h"
#include "Bmc810544CA1.h"
#include "BmcG146.h"
#include "BmcNtd03.h"
#include "BnRom.h"
#include "Bs5.h"
#include "Caltron41.h"
#include "Cc21.h"
#include "CNROM.h"
#include "CpRom.h"
#include "ColorDreams.h"
#include "ColorDreams46.h"
#include "DaouInfosys.h"
#include "DreamTech01.h"
#include "Edu2000.h"
#include "FDS.h"
#include "FrontFareast.h"
#include "Ghostbusters63in1.h"
#include "Gs2004.h"
#include "Gs2013.h"
#include "GxRom.h"
#include "Henggedianzi177.h"
#include "Henggedianzi179.h"
#include "Hp898f.h"
#include "IremG101.h"
#include "IremH3001.h"
#include "IremLrog017.h"
#include "IremTamS1.h"
#include "JalecoJf11_14.h"
#include "JalecoJf13.h"
#include "JalecoJf16.h"
#include "JalecoJf17_19.h"
#include "JalecoJfxx.h"
#include "JalecoSs88006.h"
#include "JyCompany.h"
#include "Kaiser202.h"
#include "Kaiser7012.h"
#include "Kaiser7013B.h"
#include "Kaiser7016.h"
#include "Kaiser7022.h"
#include "Kaiser7037.h"
#include "Kaiser7057.h"
#include "Kaiser7058.h"
#include "Lh10.h"
#include "Lh32.h"
#include "Malee.h"
#include "Mapper15.h"
#include "Mapper35.h"
#include "Mapper40.h"
#include "Mapper42.h"
#include "Mapper43.h"
#include "Mapper50.h"
#include "Mapper57.h"
#include "Mapper58.h"
#include "Mapper60.h"
#include "Mapper61.h"
#include "Mapper62.h"
#include "Mapper83.h"
#include "Mapper91.h"
#include "Mapper103.h"
#include "Mapper106.h"
#include "Mapper107.h"
#include "Mapper108.h"
#include "Mapper112.h"
#include "Mapper117.h"
#include "Mapper120.h"
#include "Mapper170.h"
#include "Mapper183.h"
#include "Mapper200.h"
#include "Mapper201.h"
#include "Mapper202.h"
#include "Mapper203.h"
#include "Mapper204.h"
#include "Mapper212.h"
#include "Mapper213.h"
#include "Mapper214.h"
#include "Mapper216.h"
#include "Mapper218.h"
#include "Mapper220.h"
#include "Mapper221.h"
#include "Mapper222.h"
#include "Mapper225.h"
#include "Mapper226.h"
#include "Mapper227.h"
#include "Mapper229.h"
#include "Mapper230.h"
#include "Mapper231.h"
#include "Mapper233.h"
#include "Mapper234.h"
#include "Mapper240.h"
#include "Mapper241.h"
#include "Mapper242.h"
#include "Mapper244.h"
#include "Mapper246.h"
#include "Mapper253.h"
#include "MMC1.h"
#include "MMC1_105.h"
#include "MMC1_155.h"
#include "MMC2.h"
#include "MMC3.h"
#include "MMC3_12.h"
#include "MMC3_14.h"
#include "MMC3_37.h"
#include "MMC3_44.h"
#include "MMC3_45.h"
#include "MMC3_47.h"
#include "MMC3_49.h"
#include "MMC3_52.h"
#include "MMC3_114.h"
#include "MMC3_115.h"
#include "MMC3_121.h"
#include "MMC3_123.h"
#include "MMC3_126.h"
#include "MMC3_134.h"
#include "MMC3_165.h"
#include "MMC3_182.h"
#include "MMC3_187.h"
#include "MMC3_189.h"
#include "MMC3_196.h"
#include "MMC3_197.h"
#include "MMC3_199.h"
#include "MMC3_205.h"
#include "MMC3_215.h"
#include "MMC3_217.h"
#include "MMC3_219.h"
#include "MMC3_238.h"
#include "MMC3_245.h"
#include "MMC3_249.h"
#include "MMC3_250.h"
#include "MMC3_254.h"
#include "MMC3_Bmc411120C.h"
#include "MMC3_BmcF15.h"
#include "MMC3_ChrRam.h"
#include "MMC3_Coolboy.h"
#include "MMC3_Kof97.h"
#include "MMC3_MaliSB.h"
#include "MMC3_StreetHeroes.h"
#include "MMC3_Super24in1Sc03.h"
#include "MMC4.h"
#include "MMC5.h"
#include "Namco108.h"
#include "Namco108_76.h"
#include "Namco108_88.h"
#include "Namco108_95.h"
#include "Namco108_154.h"
#include "Namco163.h"
#include "Nanjing.h"
#include "Nina01.h"
#include "Nina03_06.h"
#include "NovelDiamond.h"
#include "NROM.h"
#include "NsfCart31.h"
#include "NsfMapper.h"
#include "NtdecTc112.h"
#include "OekaKids.h"
#include "Racermate.h"
#include "Rambo1.h"
#include "Rt01.h"
#include "Sachen_133.h"
#include "Sachen_136.h"
#include "Sachen_143.h"
#include "Sachen_145.h"
#include "Sachen_147.h"
#include "Sachen_148.h"
#include "Sachen_149.h"
#include "Sachen74LS374N.h"
#include "Sachen74LS374NB.h"
#include "Sachen8259.h"
#include "Smb2j.h"
#include "StudyBox.h"
#include "Subor166.h"
#include "Sunsoft3.h"
#include "Sunsoft4.h"
#include "Sunsoft89.h"
#include "Sunsoft93.h"
#include "Sunsoft184.h"
#include "SunsoftFme7.h"
#include "Supervision.h"
#include "Super40in1Ws.h"
#include "T262.h"
#include "TaitoTc0190.h"
#include "TaitoTc0690.h"
#include "TaitoX1005.h"
#include "TaitoX1017.h"
#include "Tf1201.h"
#include "Txc22000.h"
#include "Txc22211A.h"
#include "Txc22211B.h"
#include "Txc22211C.h"
#include "TxSRom.h"
#include "Unl43272.h"
#include "UnlPci556.h"
#include "UNROM.h"
#include "UnRom_94.h"
#include "UnRom_180.h"
#include "VRC1.h"
#include "VRC2_4.h"
#include "VRC3.h"
#include "VRC6.h"
#include "VRC7.h"
#include "VsSystem.h"
#include "Waixing162.h"
#include "Waixing164.h"
#include "Waixing176.h"
#include "Waixing178.h"
#include "Waixing252.h"

/*
Supported mappers:  
... : bad mappers
--- : potentially bad mappers
=== : not supported by both Nestopia & FCEUX
??? : No known roms
-----------------------------------------------------------------
| 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10| 11| 12| 13| 14| 15|
| 16| 17| 18| 19|...| 21| 22| 23| 24| 25| 26| 27| 28|   |   | 31|
| 32| 33| 34| 35| 36| 37| 38|---| 40| 41| 42| 43| 44| 45| 46| 47|
| 48| 49| 50| 51| 52| 53|???|???| 56| 57| 58|===| 60| 61| 62| 63|
| 64| 65| 66| 67| 68| 69| 70| 71| 72| 73| 74| 75| 76| 77| 78| 79|
| 80|===| 82| 83|===| 85| 86| 87| 88| 89| 90| 91| 92| 93| 94| 95|
| 96| 97|===| 99|...|101|===|103|???|105|106|107|108|===|===|===|
|112|113|114|115|   |117|118|119|120|121|===|123|===|???|126|===|
|===|===|===|===|132|133|134|===|136|137|138|139|140|141|142|143|
|144|145|146|147|148|149|150|151|152|153|154|155|156|157|???|159|
|---|===|162|163|164|165|166|167|168|===|170|171|172|173|===|175|
|176|177|178|179|180|---|182|183|184|185|186|187|188|189|===|191|
|192|193|194|195|196|197|   |199|200|201|202|203|204|205|206|207|
|???|209|210|211|212|213|214|215|216|217|218|219|220|221|222|???|
|???|225|226|227|228|229|230|231|232|233|234|235|236|===|238|===|
|240|241|242|243|244|245|246|===|===|249|250|===|252|253|254|255|
-----------------------------------------------------------------
*/

BaseMapper* MapperFactory::GetMapperFromID(RomData &romData)
{
#ifdef _DEBUG
	MessageManager::DisplayMessage("GameInfo", "Mapper", std::to_string(romData.MapperID), std::to_string(romData.SubMapperID));
#endif

	switch(romData.MapperID) {
		case 0: return new NROM();
		case 1: return new MMC1();
		case 2: return new UNROM();
		case 3: return new CNROM(false);
		case 4: return new MMC3();
		case 5: return new MMC5();
		case 6: return new FrontFareast();
		case 7: return new AXROM();
		case 8: return new FrontFareast();
		case 9: return new MMC2();
		case 10: return new MMC4();
		case 11: return new ColorDreams();
		case 12: return new MMC3_12();
		case 13: return new CpRom();
		case 14: return new MMC3_14();
		case 15: return new Mapper15();
		case 16: return new BandaiFcg();
		case 17: return new FrontFareast();
		case 18: return new JalecoSs88006();
		case 19: return new Namco163();
		case 21: return new VRC2_4();
		case 22: return new VRC2_4();
		case 23: return new VRC2_4();
		case 24: return new VRC6(VRCVariant::VRC6a);
		case 25: return new VRC2_4();
		case 26: return new VRC6(VRCVariant::VRC6b);
		case 27: return new VRC2_4();
		case 28: return new Action53();
		case 31: return new NsfCart31();
		case 32: return new IremG101();
		case 33: return new TaitoTc0190();
		case 34: 
			switch(romData.SubMapperID) {
				case 0: return (romData.ChrRom.size() > 0) ? (BaseMapper*)new Nina01() : (BaseMapper*)new BnRom(); //BnROM uses CHR RAM (so no CHR rom in the .NES file)
				case 1: return new Nina01();
				case 2: return new BnRom();
			}
		case 35: return new Mapper35();
		case 36: return new Txc22000();
		case 37: return new MMC3_37();
		case 38: return new UnlPci556();
		case 40: return new Mapper40();
		case 41: return new Caltron41();
		case 42: return new Mapper42();
		case 43: return new Mapper43();
		case 44: return new MMC3_44();
		case 45: return new MMC3_45();
		case 46: return new ColorDreams46();
		case 47: return new MMC3_47();
		case 48: return new TaitoTc0690();
		case 49: return new MMC3_49();
		case 50: return new Mapper50();
		case 51: return new Bmc51();
		case 52: return new MMC3_52();
		case 53: return new Supervision();
		case 56: return new Kaiser202();
		case 57: return new Mapper57();
		case 58: return new Mapper58();
		case 60: return new Mapper60();  //Partial support?
		case 61: return new Mapper61();
		case 62: return new Mapper62();
		case 63: return new Bmc63();
		case 64: return new Rambo1();
		case 65: return new IremH3001();
		case 66: return new GxRom();
		case 67: return new Sunsoft3();
		case 68: return new Sunsoft4();
		case 69: return new SunsoftFme7();
		case 70: return new Bandai74161_7432(false);
		case 71: return new BF909x();
		case 72: return new JalecoJf17_19(false);
		case 73: return new VRC3();
		case 74: return new MMC3_ChrRam(0x08, 0x09, 2);
		case 75: return new VRC1();
		case 76: return new Namco108_76();
		case 77: return new IremLrog017();
		case 78: return new JalecoJf16();
		case 79: return new Nina03_06(false);
		case 80: return new TaitoX1005(false);
		case 82: return new TaitoX1017();
		case 83: return new Mapper83();
		case 85: return new VRC7();
		case 86: return new JalecoJf13();
		case 87: return new JalecoJfxx(false);
		case 88: return new Namco108_88();
		case 89: return new Sunsoft89();
		case 90: return new JyCompany();
		case 91: return new Mapper91();
		case 92: return new JalecoJf17_19(true);
		case 93: return new Sunsoft93();
		case 94: return new UnRom_94();
		case 95: return new Namco108_95();
		case 96: return new OekaKids();
		case 97: return new IremTamS1();
		case 99: return new VsSystem();
		case 101: return new JalecoJfxx(true);
		case 103: return new Mapper103();
		case 105: return new MMC1_105(); break;
		case 106: return new Mapper106();
		case 107: return new Mapper107();
		case 108: return new Mapper108();
		case 112: return new Mapper112();
		case 113: return new Nina03_06(true);
		case 114: return new MMC3_114();
		case 115: return new MMC3_115();
		case 117: return new Mapper117();
		case 118: return new TxSRom();
		case 119: return new MMC3_ChrRam(0x40, 0x7F, 8);
		case 120: return new Mapper120();
		case 121: return new MMC3_121();
		case 123: return new MMC3_123();
		case 125: return new Lh32();
		case 126: return new MMC3_126();
		case 132: return new Txc22211A();
		case 133: return new Sachen_133();
		case 134: return new MMC3_134();
		case 136: return new Sachen_136();
		case 137: return new Sachen8259(Sachen8259Variant::Sachen8259D);
		case 138: return new Sachen8259(Sachen8259Variant::Sachen8259B);
		case 139: return new Sachen8259(Sachen8259Variant::Sachen8259C);
		case 140: return new JalecoJf11_14();
		case 141: return new Sachen8259(Sachen8259Variant::Sachen8259A);
		case 142: return new Kaiser202();
		case 143: return new Sachen_143();
		case 144: return new ColorDreams();
		case 145: return new Sachen_145();
		case 146: return new Nina03_06(false);
		case 147: return new Sachen_147();
		case 148: return new Sachen_148();
		case 149: return new Sachen_149();
		case 150: return new Sachen74LS374NB();
		case 151: return new VRC1();
		case 152: return new Bandai74161_7432(true);
		case 153: return new BandaiFcg();
		case 154: return new Namco108_154();
		case 155: return new MMC1_155();
		case 156: return new DaouInfosys();
		case 157: return new BandaiFcg();
		case 159: return new BandaiFcg();
		case 162: return new Waixing162();
		case 163: return new Nanjing();
		case 164: return new Waixing164();
		case 165: return new MMC3_165();
		case 166: return new Subor166();
		case 167: return new Subor166();
		case 168: return new Racermate();
		case 170: return new Mapper170();
		case 171: return new Kaiser7058();
		case 172: return new Txc22211B();
		case 173: return new Txc22211C();
		case 175: return new Kaiser7022();
		case 176: return new Waixing176();
		case 177: return new Henggedianzi177();
		case 178: return new Waixing178();
		case 179: return new Henggedianzi179();
		case 180: return new UnRom_180();
		case 182: return new MMC3_182();
		case 183: return new Mapper183();
		case 184: return new Sunsoft184();
		case 185: return new CNROM(true);
		case 186: return new StudyBox();
		case 187: return new MMC3_187();
		case 188: return new BandaiKaraoke();
		case 189: return new MMC3_189();
		case 191: return new MMC3_ChrRam(0x80, 0xFF, 2);
		case 192: return new MMC3_ChrRam(0x08, 0x0B, 4);
		case 193: return new NtdecTc112();
		case 194: return new MMC3_ChrRam(0x00, 0x01, 2);
		case 195: return new MMC3_ChrRam(0x00, 0x03, 4);
		case 196: return new MMC3_196();
		case 197: return new MMC3_197();
		case 199: return new MMC3_199();
		case 200: return new Mapper200();
		case 201: return new Mapper201();
		case 202: return new Mapper202();
		case 203: return new Mapper203();
		case 204: return new Mapper204();
		case 205: return new MMC3_205();
		case 206: return new Namco108();
		case 207: return new TaitoX1005(true);
		case 209: return new JyCompany();
		case 210: return new Namco163();
		case 211: return new JyCompany();
		case 212: return new Mapper212();
		case 213: return new Mapper213();
		case 214: return new Mapper214();
		case 215: return new MMC3_215();
		case 216: return new Mapper216();
		case 217: return new MMC3_217();
		case 218: return new Mapper218();
		case 219: return new MMC3_219();
		case 220: return new Mapper220();
		case 221: return new Mapper221();
		case 222: return new Mapper222();
		case 225: return new Mapper225();
		case 226: return new Mapper226();
		case 227: return new Mapper227();
		case 228: return new ActionEnterprises();
		case 229: return new Mapper229();
		case 230: return new Mapper230();
		case 231: return new Mapper231();
		case 232: return new BF9096();
		case 233: return new Mapper233();
		case 234: return new Mapper234();
		case 235: return new Bmc235();
		case 236: return new Bmc70in1();
		case 238: return new MMC3_238();
		case 240: return new Mapper240();
		case 241: return new Mapper241();
		case 242: return new Mapper242();
		case 243: return new Sachen74LS374N();
		case 244: return new Mapper244();
		case 245: return new MMC3_245();
		case 246: return new Mapper246();
		case 249: return new MMC3_249();
		case 250: return new MMC3_250();
		case 252: return new Waixing252();
		case 253: return new Mapper253();
		case 254: return new MMC3_254();
		case 255: return new Bmc255();

		case UnifBoards::A65AS: return new A65AS();
		case UnifBoards::Ac08: return new Ac08();
		case UnifBoards::Ax5705: return new Ax5705();
		case UnifBoards::Bb: return new Bb();
		case UnifBoards::Bmc11160: return new Bmc11160();
		case UnifBoards::Bmc12in1: return new Bmc12in1();
		case UnifBoards::Bmc411120C: return new MMC3_Bmc411120C();
		case UnifBoards::Bmc64in1NoRepeat: return new Bmc64in1NoRepeat();
		case UnifBoards::Bmc70in1: return new Bmc70in1();
		case UnifBoards::Bmc70in1B: return new Bmc70in1();
		case UnifBoards::Bmc190in1: return new Bmc190in1();
		case UnifBoards::Bmc810544CA1: return new Bmc810544CA1();
		case UnifBoards::BmcF15: return new MMC3_BmcF15();
		case UnifBoards::BmcG146: return new BmcG146();
		case UnifBoards::BmdNtd03: return new BmcNtd03();
		case UnifBoards::Bs5: return new Bs5();
		case UnifBoards::Cc21: return new Cc21();
		case UnifBoards::Coolboy: return new MMC3_Coolboy();
		case UnifBoards::DreamTech01: return new DreamTech01();
		case UnifBoards::Edu2000: return new Edu2000();
		case UnifBoards::Ghostbusters63in1: return new Ghostbusters63in1();			
		case UnifBoards::Gs2004: return new Gs2004();
		case UnifBoards::Gs2013: return new Gs2013();
		case UnifBoards::Hp898f: return new Hp898f();
		case UnifBoards::Kof97: return new MMC3_Kof97();
		case UnifBoards::Ks7012: return new Kaiser7012();
		case UnifBoards::Ks7013B: return new Kaiser7013B();
		case UnifBoards::Ks7016: return new Kaiser7016();
		case UnifBoards::Ks7037: return new Kaiser7037();
		case UnifBoards::Ks7057: return new Kaiser7057();
		case UnifBoards::Lh10: return new Lh10();
		case UnifBoards::Malee: return new Malee();
		case UnifBoards::MaliSB: return new MMC3_MaliSB();
		case UnifBoards::NovelDiamond: return new NovelDiamond();
		case UnifBoards::Rt01: return new Rt01();
		case UnifBoards::Smb2j: return new Smb2j();
		case UnifBoards::StreetHeroes: return new MMC3_StreetHeroes();
		case UnifBoards::Super24in1Sc03: return new MMC3_Super24in1Sc03();
		case UnifBoards::Super40in1Ws: return new Super40in1Ws();
		case UnifBoards::T262: return new T262();
		case UnifBoards::Tf1201: return new Tf1201();
		case UnifBoards::Unl43272: return new Unl43272();

		case MapperFactory::NsfMapperID: return new NsfMapper();
		case MapperFactory::FdsMapperID: return new FDS();
	}

	MessageManager::DisplayMessage("Error", "UnsupportedMapper", "iNES #" + std::to_string(romData.MapperID));
	return nullptr;
}

shared_ptr<BaseMapper> MapperFactory::InitializeFromFile(string romFilename, stringstream *filestream, string ipsFilename, int32_t archiveFileIndex)
{
	RomLoader loader;

	if(loader.LoadFile(romFilename, filestream, ipsFilename, archiveFileIndex)) {
		RomData romData = loader.GetRomData();
		shared_ptr<BaseMapper> mapper(GetMapperFromID(romData));

		if(mapper) {
			mapper->Initialize(romData);
			return mapper;
		}
	}
	return nullptr;
}

