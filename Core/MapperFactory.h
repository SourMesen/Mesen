#pragma once
#include "stdafx.h"
#include "BaseMapper.h"
#include "RomData.h"

class MapperFactory
{
	private:
		static BaseMapper* GetMapperFromID(RomData &romData);

	public:
		static const uint16_t FdsMapperID = 65535;
		static const uint16_t NsfMapperID = 65534;
		static const uint16_t UnknownBoard = 65533;
		static const uint16_t UnifTf1201 = 65532;
		static const uint16_t UnifCoolboy = 65531;
		static const uint16_t UnifSmb2j = 65530;
		static const uint16_t UnifMalee = 65529;
		static const uint16_t UnifStreetHeroes = 65528;
		static const uint16_t UnifDreamTech01 = 65527;
		static const uint16_t UnifEdu2000 = 65526;
		static const uint16_t UnifGs2013 = 65525;
		static const uint16_t UnifGs2004 = 65524;
		static const uint16_t UnifNovelDiamond = 65523;
		static const uint16_t UnifKof97 = 65522;
		static const uint16_t UnifT262 = 65521;
		static const uint16_t UnifA65AS = 65520;
		static const uint16_t UnifBs5 = 65519;
		static const uint16_t UnifBmc190in1 = 65518;
		static const uint16_t UnifGhostbusters63in1 = 65517;
		static const uint16_t UnifBmc70in1 = 65516;
		static const uint16_t UnifBmc70in1B = 65515;
		static const uint16_t UnifAx5705 = 65514;
		static const uint16_t UnifSuper24in1Sc03 = 65513;
		static const uint16_t UnifSuper40in1Ws = 65512;
		static const uint16_t UnifCc21 = 65511;
		static const uint16_t UnifKs7016 = 65510;

		static shared_ptr<BaseMapper> InitializeFromFile(string romFilename, stringstream *filestream, string ipsFilename, int32_t archiveFileIndex);
};
