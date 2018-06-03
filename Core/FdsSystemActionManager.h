#pragma once
#include "stdafx.h"
#include "SystemActionManager.h"
#include "MessageManager.h"
#include "FDS.h"

class FdsSystemActionManager : public SystemActionManager
{
private:
	const uint8_t ReinsertDiskFrameDelay = 120;

	std::weak_ptr<FDS> _mapper;

	bool _needEjectDisk = false;
	uint8_t _insertDiskNumber = 0;
	uint8_t _insertDiskDelay = 0;
	uint32_t _sideCount;

protected:
	string GetKeyNames() override
	{
		return string("RPE0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ").substr(0, _sideCount + 3);
	}

	void StreamState(bool saving) override
	{
		SystemActionManager::StreamState(saving);
		Stream(_needEjectDisk, _insertDiskNumber, _insertDiskDelay);
	}

public:
	enum FdsButtons { EjectDiskButton = 2, InsertDisk1 };

	FdsSystemActionManager(shared_ptr<Console> console, shared_ptr<BaseMapper> mapper) : SystemActionManager(console)
	{
		_mapper = std::dynamic_pointer_cast<FDS>(mapper);
		_sideCount = std::dynamic_pointer_cast<FDS>(mapper)->GetSideCount();

		if(EmulationSettings::CheckFlag(EmulationFlags::FdsAutoLoadDisk)) {
			InsertDisk(0);
		}
	}

	void OnAfterSetState() override
	{
		SystemActionManager::OnAfterSetState();
		if(_needEjectDisk) {
			SetBit(FdsSystemActionManager::FdsButtons::EjectDiskButton);
			_needEjectDisk = false;
		}
		if(_insertDiskDelay > 0) {
			_insertDiskDelay--;
			if(_insertDiskDelay == 0) {
				SetBit(FdsSystemActionManager::FdsButtons::InsertDisk1 + _insertDiskNumber);
			}
		}

		bool needEject = IsPressed(FdsSystemActionManager::FdsButtons::EjectDiskButton);
		int diskToInsert = -1;
		for(int i = 0; i < 16; i++) {
			if(IsPressed(FdsSystemActionManager::FdsButtons::InsertDisk1 + i)) {
				diskToInsert = i;
				break;
			}
		}

		if(needEject || diskToInsert >= 0) {
			shared_ptr<FDS> mapper = _mapper.lock();
			if(needEject) {
				mapper->EjectDisk();
			}

			if(diskToInsert >= 0) {
				mapper->InsertDisk(diskToInsert);
				
			}
		}
	}
	
	void EjectDisk()
	{
		_needEjectDisk = true;
	}

	void InsertDisk(uint8_t diskNumber)
	{
		shared_ptr<FDS> mapper = _mapper.lock();
		if(mapper) {
			if(mapper->IsDiskInserted()) {
				//Eject disk on next frame, then insert new disk 2 seconds later
				_needEjectDisk = true;
				_insertDiskNumber = diskNumber;
				_insertDiskDelay = FdsSystemActionManager::ReinsertDiskFrameDelay;
			} else {
				//Insert disk on next frame
				_insertDiskNumber = diskNumber;
				_insertDiskDelay = 1;
			}
			MessageManager::DisplayMessage("FDS", "FdsDiskInserted", std::to_string(diskNumber / 2 + 1), diskNumber & 0x01 ? "B" : "A");
		}
	}

	void SwitchDiskSide()
	{
		if(!IsAutoInsertDiskEnabled()) {
			shared_ptr<FDS> mapper = _mapper.lock();
			if(mapper && mapper->IsDiskInserted()) {
				InsertDisk((mapper->GetCurrentDisk() ^ 0x01) % mapper->GetSideCount());
			}
		}
	}

	void InsertNextDisk()
	{
		if(!IsAutoInsertDiskEnabled()) {
			shared_ptr<FDS> mapper = _mapper.lock();
			if(mapper) {
				InsertDisk(((mapper->GetCurrentDisk() & 0xFE) + 2) % mapper->GetSideCount());
			}
		}
	}

	uint32_t GetSideCount()
	{
		shared_ptr<FDS> mapper = _mapper.lock();
		if(mapper) {
			return mapper->GetSideCount();
		} else {
			return 0;
		}
	}

	bool IsAutoInsertDiskEnabled()
	{
		shared_ptr<FDS> mapper = _mapper.lock();
		if(mapper) {
			return mapper->IsAutoInsertDiskEnabled();
		} else {
			return false;
		}
	}
};
