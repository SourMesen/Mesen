#include "stdafx.h"
#include "BaseMapper.h"

class MapperFactory
{
	private:
		static BaseMapper* GetMapperFromID(uint8_t mapperID);

	public:
		static shared_ptr<BaseMapper> InitializeFromFile(wstring filename);
};
