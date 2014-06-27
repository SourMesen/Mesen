#include "stdafx.h"
#include "ROMLoader.h"
#include "AXROM.h"
#include "CNROM.h"
#include "MMC1.h"
#include "MMC3.h"
#include "NROM.h"
#include "UNROM.h"

class MapperFactory
{
	private:
		static BaseMapper* GetMapperFromID(uint8_t mapperID)
		{
			switch(mapperID) {
				case 0: return new NROM();
				case 1: return new MMC1();
				case 2: return new UNROM();
				case 3: return new CNROM();
				case 4: return new MMC3();
				case 7: return new AXROM();
			}

			throw std::exception("Unsupported mapper");
			return nullptr;
		}

	public:
		static shared_ptr<BaseMapper> InitializeFromFile(wstring filename)
		{
			ROMLoader loader(filename);

			uint8_t mapperID = loader.GetMapperID();
			
			BaseMapper* mapper = GetMapperFromID(mapperID);	
			mapper->Initialize(loader);
			return shared_ptr<BaseMapper>(mapper);
		}
};
