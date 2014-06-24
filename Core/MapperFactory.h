#include "stdafx.h"
#include "BaseMapper.h"
#include "MMC1.h"
#include "ROMLoader.h"

class MapperFactory
{
	public:
		static shared_ptr<BaseMapper> InitializeFromFile(wstring filename)
		{
			ROMLoader loader(filename);

			NESHeader header = loader.GetHeader();
			
			uint8_t mapperID = header.GetMapperID();
			BaseMapper* mapper = nullptr;
			switch(mapperID) {
				case 0: mapper = new DefaultMapper(); break;
				case 1: mapper = new MMC1(); break;
				case 2: mapper = new Mapper2(); break;
			}			

			if(!mapper) {
				throw std::exception("Unsupported mapper");
			}

			mapper->Initialize(header.GetMirroringType(), loader);
			return shared_ptr<BaseMapper>(mapper);
		}
};
