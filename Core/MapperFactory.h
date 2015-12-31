#include "stdafx.h"
#include "BaseMapper.h"

class MapperFactory
{
	private:
		static BaseMapper* GetMapperFromID(ROMLoader &romLoader);

	public:
		static shared_ptr<BaseMapper> InitializeFromFile(string romFilename, stringstream *filestream, string ipsFilename);
};
