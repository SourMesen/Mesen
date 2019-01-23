#include "stdafx.h"
#include "Debugger.h"
#include "MemoryManager.h"
#include "PPU.h"
#include "CodeDataLogger.h"
#include "BaseMapper.h"
#include "MemoryDumper.h"
#include "VideoDecoder.h"
#include "Disassembler.h"
#include "MMC5.h"
#include "DebugBreakHelper.h"

MemoryDumper::MemoryDumper(shared_ptr<PPU> ppu, shared_ptr<MemoryManager> memoryManager, shared_ptr<BaseMapper> mapper, shared_ptr<CodeDataLogger> codeDataLogger, Debugger* debugger, shared_ptr<Disassembler> disassembler)
{
	_debugger = debugger;
	_ppu = ppu;
	_memoryManager = memoryManager;
	_mapper = mapper;
	_codeDataLogger = codeDataLogger;
	_disassembler = disassembler;
}

void MemoryDumper::SetMemoryState(DebugMemoryType type, uint8_t *buffer, int32_t length)
{
	switch(type) {
		case DebugMemoryType::ChrRom:
		case DebugMemoryType::PrgRom:
		case DebugMemoryType::CpuMemory:
		case DebugMemoryType::PpuMemory:
			break;

		case DebugMemoryType::InternalRam:
			for(int i = 0; i < 0x800 && i < length; i++) {
				_memoryManager->DebugWrite(i, buffer[i]);
			}
			break;

		case DebugMemoryType::PaletteMemory:
			for(int i = 0; i < 0x20 && i < length; i++) {
				_ppu->WritePaletteRAM(i, buffer[i]);
			}
			break;

		case DebugMemoryType::SpriteMemory: memcpy(_ppu->GetSpriteRam(), buffer, std::min(length, 0x100)); break;
		case DebugMemoryType::SecondarySpriteMemory: memcpy(_ppu->GetSecondarySpriteRam(), buffer, std::min(length, 0x20)); break;

		case DebugMemoryType::ChrRam:
		case DebugMemoryType::WorkRam:
		case DebugMemoryType::SaveRam:
		case DebugMemoryType::NametableRam:
			_mapper->WriteMemory(type, buffer, length);
			break;
	}
}

bool MemoryDumper::HasUndoHistory()
{
	return _undoHistory.size() > 0;
}

void MemoryDumper::PerformUndo()
{
	if(!_undoHistory.empty()) {
		DebugBreakHelper helper(_debugger);
		_mapper->RestorePrgChrBackup(_undoHistory.back());
		_undoHistory.pop_back();
		_debugger->UpdateCdlCache();
	}
}

void MemoryDumper::AddUndoHistory(vector<uint8_t> &originalRomData)
{
	vector<uint8_t> newData = _mapper->GetPrgChrCopy();
	if(memcmp(originalRomData.data(), newData.data(), originalRomData.size()) != 0) {
		//Add a step in the undo history
		_undoHistory.push_back(originalRomData);
		if(_undoHistory.size() > 100) {
			_undoHistory.pop_front();
		}
	}
}

uint32_t MemoryDumper::GetMemorySize(DebugMemoryType type)
{
	switch(type) {
		case DebugMemoryType::CpuMemory: return 0x10000;
		case DebugMemoryType::PpuMemory: return 0x4000;
		case DebugMemoryType::PaletteMemory: return 0x20;
		case DebugMemoryType::SpriteMemory: return 0x100;
		case DebugMemoryType::SecondarySpriteMemory: return 0x20;
		case DebugMemoryType::InternalRam: return 0x800;
		case DebugMemoryType::NametableRam:
		case DebugMemoryType::PrgRom:
		case DebugMemoryType::ChrRom:
		case DebugMemoryType::ChrRam:
		case DebugMemoryType::WorkRam:
		case DebugMemoryType::SaveRam:
			return _mapper->GetMemorySize(type);
	}
	return 0;
}

uint32_t MemoryDumper::GetMemoryState(DebugMemoryType type, uint8_t *buffer)
{
	switch(type) {
		case DebugMemoryType::CpuMemory:
			for(int i = 0; i <= 0xFFFF; i++) {
				buffer[i] = _memoryManager->DebugRead(i);
			}
			return 0x10000;

		case DebugMemoryType::PpuMemory:
			for(int i = 0; i < 0x3F00; i++) {
				buffer[i] = _mapper->DebugReadVRAM(i);
			}
			for(int i = 0x3F00; i <= 0x3FFF; i++) {
				buffer[i] = _ppu->ReadPaletteRAM(i);
			}
			return 0x4000;

		case DebugMemoryType::PaletteMemory:
			for(int i = 0; i <= 0x1F; i++) {
				buffer[i] = _ppu->ReadPaletteRAM(i);
			}
			return 0x20;

		case DebugMemoryType::SpriteMemory:
			memcpy(buffer, _ppu->GetSpriteRam(), 0x100);
			return 0x100;

		case DebugMemoryType::SecondarySpriteMemory:
			memcpy(buffer, _ppu->GetSecondarySpriteRam(), 0x20);
			return 0x20;

		case DebugMemoryType::PrgRom:
		case DebugMemoryType::ChrRom:
		case DebugMemoryType::ChrRam:
		case DebugMemoryType::WorkRam:
		case DebugMemoryType::SaveRam:
		case DebugMemoryType::NametableRam:
			return _mapper->CopyMemory(type, buffer);

		case DebugMemoryType::InternalRam:
			for(int i = 0; i < 0x800; i++) {
				buffer[i] = _memoryManager->DebugRead(i);
			}
			return 0x800;
	}
	return 0;
}

void MemoryDumper::SetMemoryValues(DebugMemoryType memoryType, uint32_t address, uint8_t* data, int32_t length)
{
	vector<uint8_t> originalRomData = _mapper->GetPrgChrCopy();
	for(int i = 0; i < length; i++) {
		SetMemoryValue(memoryType, address+i, data[i], true);
	}

	if(memoryType == DebugMemoryType::CpuMemory) {
		//Rebuild prg rom cache as needed after editing the code with the assembler/hex editor
		AddressTypeInfo infoStart, infoEnd;
		_debugger->GetAbsoluteAddressAndType(address, &infoStart);
		_debugger->GetAbsoluteAddressAndType(address+length, &infoEnd);

		if(infoStart.Type == AddressType::PrgRom && infoEnd.Type == AddressType::PrgRom && infoEnd.Address - infoStart.Address == length) {
			_disassembler->RebuildPrgRomCache(infoStart.Address, length);
		}
	}

	AddUndoHistory(originalRomData);
}

void MemoryDumper::SetMemoryValue(DebugMemoryType memoryType, uint32_t address, uint8_t value, bool preventRebuildCache, bool disableSideEffects)
{
	vector<uint8_t> originalRomData;
	if(!preventRebuildCache) {
		originalRomData = _mapper->GetPrgChrCopy();
	}
	
	switch(memoryType) {
		case DebugMemoryType::CpuMemory:
			if(disableSideEffects) {
				AddressTypeInfo info;
				_debugger->GetAbsoluteAddressAndType(address, &info);
				if(info.Address >= 0) {
					switch(info.Type) {
						case AddressType::Register: break; //not supported
						case AddressType::InternalRam: SetMemoryValue(DebugMemoryType::InternalRam, info.Address, value, preventRebuildCache, true); break;
						case AddressType::PrgRom: SetMemoryValue(DebugMemoryType::PrgRom, info.Address, value, preventRebuildCache, true); break;
						case AddressType::WorkRam: SetMemoryValue(DebugMemoryType::WorkRam, info.Address, value, preventRebuildCache, true); break;
						case AddressType::SaveRam: SetMemoryValue(DebugMemoryType::SaveRam, info.Address, value, preventRebuildCache, true); break;
					}
				}
			} else {
				_memoryManager->DebugWrite(address, value, false);
			}
			break;

		case DebugMemoryType::PpuMemory:
			if(address < 0x3F00) {
				_mapper->DebugWriteVRAM(address, value, disableSideEffects);
			} else {
				_ppu->WritePaletteRAM(address, value);
			}
			break;
		case DebugMemoryType::PaletteMemory: _ppu->WritePaletteRAM(address, value); break;
		case DebugMemoryType::SpriteMemory: _ppu->GetSpriteRam()[address % 0x100] = value; break;
		case DebugMemoryType::SecondarySpriteMemory: _ppu->GetSecondarySpriteRam()[address % 0x20] = value; break;

		case DebugMemoryType::PrgRom:
			_mapper->SetMemoryValue(memoryType, address, value);
			if(!preventRebuildCache) {
				_disassembler->RebuildPrgRomCache(address, 1);
			}
			break;

		case DebugMemoryType::ChrRom:
		case DebugMemoryType::ChrRam:
		case DebugMemoryType::WorkRam:
		case DebugMemoryType::SaveRam:
		case DebugMemoryType::NametableRam:
			_mapper->SetMemoryValue(memoryType, address, value);
			break;

		case DebugMemoryType::InternalRam: _memoryManager->DebugWrite(address, value); break;
	}

	if(!preventRebuildCache) {
		AddUndoHistory(originalRomData);
	}
}

uint16_t MemoryDumper::GetMemoryValueWord(DebugMemoryType memoryType, uint32_t address, bool disableSideEffects)
{
	return GetMemoryValue(memoryType, address, disableSideEffects) | (GetMemoryValue(memoryType, address + 1, disableSideEffects) << 8);
}

void MemoryDumper::SetMemoryValueWord(DebugMemoryType memoryType, uint32_t address, uint16_t value, bool preventRebuildCache, bool disableSideEffects)
{
	SetMemoryValue(memoryType, address, (uint8_t)value, preventRebuildCache, disableSideEffects);
	SetMemoryValue(memoryType, address + 1, (uint8_t)(value >> 8), preventRebuildCache, disableSideEffects);
}

uint8_t MemoryDumper::GetMemoryValue(DebugMemoryType memoryType, uint32_t address, bool disableSideEffects)
{
	switch(memoryType) {
		case DebugMemoryType::CpuMemory: return _memoryManager->DebugRead(address, disableSideEffects);

		case DebugMemoryType::PpuMemory: return _mapper->DebugReadVRAM(address, disableSideEffects);
		case DebugMemoryType::PaletteMemory: return _ppu->ReadPaletteRAM(address);
		case DebugMemoryType::SpriteMemory: return _ppu->GetSpriteRam()[address % 0x100];
		case DebugMemoryType::SecondarySpriteMemory: return _ppu->GetSecondarySpriteRam()[address % 0x20];

		case DebugMemoryType::PrgRom:
		case DebugMemoryType::ChrRom:
		case DebugMemoryType::ChrRam:
		case DebugMemoryType::WorkRam:
		case DebugMemoryType::SaveRam:
		case DebugMemoryType::NametableRam:
			return _mapper->GetMemoryValue(memoryType, address);

		case DebugMemoryType::InternalRam: return _memoryManager->DebugRead(address);
	}

	return 0;
}

void MemoryDumper::GetNametable(int nametableIndex, NametableDisplayMode mode, uint32_t* frameBuffer, uint8_t* tileData, uint8_t* paletteData)
{
	shared_ptr<MMC5> mmc5 = std::dynamic_pointer_cast<MMC5>(_mapper);
	uint32_t *rgbPalette = _debugger->GetConsole()->GetSettings()->GetRgbPalette();
	PPUDebugState state;
	_ppu->GetState(state);
	uint16_t bgAddr = state.ControlFlags.BackgroundPatternAddr;
	uint16_t baseAddr = 0x2000 + nametableIndex * 0x400;
	uint16_t baseAttributeAddr = baseAddr + 960;
	uint8_t grayscalePalette[4] = { 0x0F, 0x00, 0x10, 0x20 };
	for(uint8_t y = 0; y < 30; y++) {
		for(uint8_t x = 0; x < 32; x++) {
			uint16_t ntIndex = (y << 5) + x;
			uint8_t tileIndex = _mapper->DebugReadVRAM(baseAddr + ntIndex);
			uint8_t attribute = _mapper->DebugReadVRAM(baseAttributeAddr + ((y & 0xFC) << 1) + (x >> 2));
			tileData[y * 32 + x] = tileIndex;
			paletteData[y * 32 + x] = attribute;
			uint8_t shift = (x & 0x02) | ((y & 0x02) << 1);

			uint8_t paletteBaseAddr;
			if(mmc5 && mmc5->IsExtendedAttributes()) {
				paletteBaseAddr = mmc5->GetExAttributeNtPalette(ntIndex) << 2;
			} else {
				paletteBaseAddr = ((attribute >> shift) & 0x03) << 2;
			}
			uint16_t tileAddr = bgAddr + (tileIndex << 4);
			if(mode == NametableDisplayMode::AttributeView) {
				for(uint8_t i = 0; i < 8; i++) {
					for(uint8_t j = 0; j < 8; j++) {
						uint8_t color = ((j & 0x04) >> 2) + ((i & 0x04) >> 1);
						frameBuffer[(y << 11) + (x << 3) + (i << 8) + j] = rgbPalette[_ppu->ReadPaletteRAM(color == 0 ? 0 : (paletteBaseAddr + color))];
					}
				}
			} else {
				for(uint8_t i = 0; i < 8; i++) {
					uint8_t lowByte, highByte;
					if(mmc5 && mmc5->IsExtendedAttributes()) {
						lowByte = mmc5->GetExAttributeTileData(ntIndex, tileAddr + i);
						highByte = mmc5->GetExAttributeTileData(ntIndex, tileAddr + i + 8);
					} else {
						lowByte = _mapper->DebugReadVRAM(tileAddr + i);
						highByte = _mapper->DebugReadVRAM(tileAddr + i + 8);
					}
					for(uint8_t j = 0; j < 8; j++) {
						uint8_t color = ((lowByte >> (7 - j)) & 0x01) | (((highByte >> (7 - j)) & 0x01) << 1);
						if(mode == NametableDisplayMode::Grayscale) {
							frameBuffer[(y << 11) + (x << 3) + (i << 8) + j] = rgbPalette[grayscalePalette[color]];
						} else {
							frameBuffer[(y << 11) + (x << 3) + (i << 8) + j] = rgbPalette[(color == 0 ? _ppu->ReadPaletteRAM(0) : _ppu->ReadPaletteRAM(paletteBaseAddr + color)) & 0x3F];
						}
					}
				}
			}
		}
	}
}

void MemoryDumper::GatherChrPaletteInfo()
{
	shared_ptr<MMC5> mmc5 = std::dynamic_pointer_cast<MMC5>(_mapper);
	PPUDebugState state;
	_ppu->GetState(state);

	uint16_t bgAddr = state.ControlFlags.BackgroundPatternAddr;

	uint32_t palettes[8];
	for(int i = 0; i < 8; i++) {
		palettes[i] =
			_ppu->ReadPaletteRAM(0) |
			(_ppu->ReadPaletteRAM(i * 4 + 1) << 8) |
			(_ppu->ReadPaletteRAM(i * 4 + 2) << 16) |
			(_ppu->ReadPaletteRAM(i * 4 + 3) << 24);
	}

	auto processTilePalette = [=](uint32_t tileAddr, uint8_t paletteIndex) {
		TileKey key;
		uint32_t absoluteAddr;
		if(!mmc5 || !mmc5->IsExtendedAttributes()) {
			absoluteAddr = _mapper->ToAbsoluteChrAddress(tileAddr);
		} else {
			absoluteAddr = tileAddr;
		}

		if(_mapper->HasChrRom()) {
			key.TileIndex = absoluteAddr / 16;
			key.IsChrRamTile = false;
		} else {
			_mapper->CopyChrTile(absoluteAddr, key.TileData);
			key.IsChrRamTile = true;
		}
		_paletteByTile[key] = palettes[paletteIndex];
	};

	if(state.ControlFlags.BackgroundEnabled) {
		//Nametables - Check all palette/tile combinations
		//Only process nametables when background is enabled (to avoid issues when booting with random RAM contents)
		for(int i = 0; i < 4; i++) {
			uint16_t baseAddr = 0x2000 + i * 0x400;
			uint16_t baseAttributeAddr = baseAddr + 960;
			for(uint8_t y = 0; y < 30; y++) {
				for(uint8_t x = 0; x < 32; x++) {
					uint16_t ntIndex = (y << 5) + x;
					uint8_t tileIndex = _mapper->DebugReadVRAM(baseAddr + ntIndex);

					uint16_t tileAddr;
					uint8_t paletteIndex;
					if(mmc5 && mmc5->IsExtendedAttributes()) {
						paletteIndex = mmc5->GetExAttributeNtPalette(ntIndex);
						tileAddr = mmc5->GetExAttributeAbsoluteTileAddr(ntIndex, tileIndex * 16);
					} else {
						uint8_t attribute = _mapper->DebugReadVRAM(baseAttributeAddr + ((y & 0xFC) << 1) + (x >> 2));
						uint8_t shift = (x & 0x02) | ((y & 0x02) << 1);
						paletteIndex = ((attribute >> shift) & 0x03);
						tileAddr = bgAddr + (tileIndex << 4);
					}

					processTilePalette(tileAddr, paletteIndex);
				}
			}
		}
	}

	if(state.ControlFlags.SpritesEnabled) {
		//Sprites - Check all sprites palettes
		//Only process sprites if sprites are enabled (to avoid issues when booting with random RAM contents)
		uint8_t *spriteRam = _ppu->GetSpriteRam();
		uint16_t spriteAddr = state.ControlFlags.SpritePatternAddr;
		bool largeSprites = state.ControlFlags.LargeSprites;
		for(uint8_t y = 0; y < 8; y++) {
			for(uint8_t x = 0; x < 8; x++) {
				uint8_t ramAddr = ((y << 3) + x) << 2;
				uint8_t tileIndex = spriteRam[ramAddr + 1];
				uint8_t attributes = spriteRam[ramAddr + 2];

				uint16_t tileAddr;
				if(largeSprites) {
					tileAddr = (tileIndex & 0x01 ? 0x1000 : 0x0000) + ((tileIndex & 0xFE) << 4);
				} else {
					tileAddr = spriteAddr + (tileIndex << 4);
				}

				uint8_t palette = (attributes & 0x03) | 0x04;
				processTilePalette(tileAddr, palette);
				if(largeSprites) {
					processTilePalette(tileAddr + 16, palette);
				}
			}
		}
	}
}

void MemoryDumper::GetChrBank(int bankIndex, uint32_t* frameBuffer, uint8_t palette, bool largeSprites, CdlHighlightType highlightType, bool useAutoPalette, bool showSingleColorTilesInGrayscale, uint32_t* paletteBuffer)
{
	uint8_t paletteBaseAddr = (palette & 0x07) << 2;
	uint32_t defaultPalette;
	if(palette & 0x08) {
		//Use grayscale palette
		defaultPalette = 0x2010000F;
	} else {
		defaultPalette = _ppu->ReadPaletteRAM(0) |
			(_ppu->ReadPaletteRAM(paletteBaseAddr + 1) << 8) |
			(_ppu->ReadPaletteRAM(paletteBaseAddr + 2) << 16) |
			(_ppu->ReadPaletteRAM(paletteBaseAddr + 3) << 24);
	}
	
	uint32_t *rgbPalette = _debugger->GetConsole()->GetSettings()->GetRgbPalette();
	uint8_t chrBuffer[0x1000];
	bool chrIsDrawn[0x1000];
	bool isChrRam = _mapper->GetMemorySize(DebugMemoryType::ChrRam) > 0;
	if(bankIndex == 0 || bankIndex == 1) {
		uint16_t baseAddr = bankIndex == 0 ? 0x0000 : 0x1000;
		for(int i = 0; i < 0x1000; i++) {
			chrBuffer[i] = _mapper->DebugReadVRAM(baseAddr + i);
			chrIsDrawn[i] = isChrRam ? true : _codeDataLogger->IsDrawn(_mapper->ToAbsoluteChrAddress(baseAddr + i));
		}
	} else {
		int bank = bankIndex - 2;
		uint32_t baseAddr = bank * 0x1000;
		uint32_t chrSize = _mapper->GetMemorySize(isChrRam ? DebugMemoryType::ChrRam : DebugMemoryType::ChrRom);
		if(baseAddr + 0xFFF >= chrSize) {
			//Out of range, return to prevent crash
			return;
		}

		vector<uint8_t> chrData(chrSize, 0);
		_mapper->CopyMemory(isChrRam ? DebugMemoryType::ChrRam : DebugMemoryType::ChrRom, chrData.data());

		for(int i = 0; i < 0x1000; i++) {
			chrBuffer[i] = chrData[baseAddr + i];
			chrIsDrawn[i] = isChrRam ? true : _codeDataLogger->IsDrawn(baseAddr + i);
		}
	}

	for(uint8_t y = 0; y < 16; y++) {
		for(uint8_t x = 0; x < 16; x++) {
			uint8_t tileIndex = y * 16 + x;
			uint32_t paletteData = defaultPalette;

			if(useAutoPalette) {
				TileKey key;
				uint32_t absoluteTileIndex = bankIndex <= 1 ? _mapper->ToAbsoluteChrAddress(bankIndex * 0x1000 + tileIndex * 16) / 16 : ((bankIndex - 2) * 256 + tileIndex);
				if(_mapper->HasChrRom()) {
					key.TileIndex = absoluteTileIndex;
					key.IsChrRamTile = false;
				} else {
					_mapper->CopyChrTile(absoluteTileIndex * 16, key.TileData);
					key.IsChrRamTile = true;
				}
				auto result = _paletteByTile.find(key);
				if(result != _paletteByTile.end()) {
					paletteData = result->second;
				}
			}

			auto outputTile = [=](uint32_t palette) {
				paletteBuffer[tileIndex] = palette;

				bool usesSingleColor = true;
				int64_t previousColor = -1;
				uint16_t tileAddr = tileIndex << 4;
				for(uint8_t i = 0; i < 8; i++) {
					uint8_t lowByte = chrBuffer[tileAddr + i];
					uint8_t highByte = chrBuffer[tileAddr + i + 8];
					bool isDrawn = chrIsDrawn[tileAddr + i];
					for(uint8_t j = 0; j < 8; j++) {
						uint8_t color = ((lowByte >> (7 - j)) & 0x01) | (((highByte >> (7 - j)) & 0x01) << 1);

						uint32_t position;
						if(largeSprites) {
							int tmpX = x / 2 + ((y & 0x01) ? 8 : 0);
							int tmpY = (y & 0xFE) + ((x & 0x01) ? 1 : 0);

							position = (tmpY << 10) + (tmpX << 3) + (i << 7) + j;
						} else {
							position = (y << 10) + (x << 3) + (i << 7) + j;
						}

						uint32_t pixelColor = rgbPalette[(palette >> (8 * color)) & 0x3F];
						frameBuffer[position] = pixelColor;
						if(previousColor >= 0 && previousColor != pixelColor) {
							usesSingleColor = false;
						}
						previousColor = pixelColor;

						if(highlightType != CdlHighlightType::None && isDrawn == (highlightType != CdlHighlightType::HighlightUsed)) {
							frameBuffer[position] &= 0x4FFFFFFF;
						}
					}
				}

				return !usesSingleColor;
			};

			if(!outputTile(paletteData) && showSingleColorTilesInGrayscale) {
				//Tile is a solid color, redraw it in grayscale
				outputTile(0x2010000F);
			}
		}
	}
}

void MemoryDumper::GetSprites(uint32_t* frameBuffer)
{
	memset(frameBuffer, 0, 64*128*sizeof(uint32_t));

	uint8_t *spriteRam = _ppu->GetSpriteRam();
	uint32_t *rgbPalette = _debugger->GetConsole()->GetSettings()->GetRgbPalette();

	PPUDebugState state;
	_ppu->GetState(state);

	uint16_t spriteAddr = state.ControlFlags.SpritePatternAddr;
	bool largeSprites = state.ControlFlags.LargeSprites;

	for(uint8_t y = 0; y < 8; y++) {
		for(uint8_t x = 0; x < 8; x++) {
			uint8_t ramAddr = ((y << 3) + x) << 2;
			uint8_t tileIndex = spriteRam[ramAddr + 1];
			uint8_t attributes = spriteRam[ramAddr + 2];

			bool verticalMirror = (attributes & 0x80) == 0x80;
			bool horizontalMirror = (attributes & 0x40) == 0x40;

			uint16_t tileAddr;
			if(largeSprites) {
				tileAddr = (tileIndex & 0x01 ? 0x1000 : 0x0000) + ((tileIndex & 0xFE) << 4);
			} else {
				tileAddr = spriteAddr + (tileIndex << 4);
			}

			uint8_t palette = 0x10 + ((attributes & 0x03) << 2);

			for(uint8_t i = 0, iMax = largeSprites ? 16 : 8; i < iMax; i++) {
				if(i == 8) {
					//Move to next tile for 2nd tile of 8x16 sprites
					tileAddr += 8;
				}

				uint8_t lowByte = _mapper->DebugReadVRAM(tileAddr + i);
				uint8_t highByte = _mapper->DebugReadVRAM(tileAddr + i + 8);
				for(uint8_t j = 0; j < 8; j++) {
					uint8_t color;
					if(horizontalMirror) {
						color = ((lowByte >> j) & 0x01) | (((highByte >> j) & 0x01) << 1);
					} else {
						color = ((lowByte >> (7 - j)) & 0x01) | (((highByte >> (7 - j)) & 0x01) << 1);
					}

					uint16_t destAddr;
					if(verticalMirror) {
						destAddr = (y << 10) + (x << 3) + (((largeSprites ? 15 : 7) - i) << 6) + j;
					} else {
						destAddr = (y << 10) + (x << 3) + (i << 6) + j;
					}

					if(color != 0) {
						frameBuffer[destAddr] = rgbPalette[_ppu->ReadPaletteRAM(palette + color) & 0x3F];
					}
				}
			}
		}
	}
}

void MemoryDumper::GetPalette(uint32_t* frameBuffer)
{
	uint32_t *rgbPalette = _debugger->GetConsole()->GetSettings()->GetRgbPalette();
	for(uint8_t i = 0; i < 32; i++) {
		frameBuffer[i] = rgbPalette[_ppu->ReadPaletteRAM(i) & 0x3F];
	}
}
