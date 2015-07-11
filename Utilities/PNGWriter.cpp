#include "stdafx.h"

#include "PNGWriter.h"
#include "miniz.h"

bool PNGWriter::WritePNG(string filename, uint8_t* buffer, uint32_t xSize, uint32_t ySize, uint32_t bitsPerPixel)
{
	size_t pngSize = 0;
	void *pngData = tdefl_write_image_to_png_file_in_memory_ex(buffer, xSize, ySize, bitsPerPixel/8, &pngSize, MZ_DEFAULT_LEVEL, MZ_FALSE);
	if(!pngData) {
		std::cout << "tdefl_write_image_to_png_file_in_memory_ex() failed!" << std::endl;
		return false;
	} else {
		ofstream file(filename, std::ios::out | std::ios::binary);
		file.write((char*)pngData, pngSize);
		file.close();
		mz_free(pngData);
		return true;
	}
}