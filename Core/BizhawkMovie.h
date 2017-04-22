#include "../Utilities/ZipReader.h"
#include "../Utilities/StringUtilities.h"
#include "Console.h"
#include "MovieManager.h"
#include "PPU.h"

class BizhawkMovie : public IMovie, public INotificationListener
{
private:
	vector<uint32_t> _systemActionByFrame;
	vector<uint8_t> _dataByFrame[4];
	bool _isPlaying = false;
	RamPowerOnState _originalPowerOnState;
	GameSystem _gameSystem;

	bool InitializeGameData(ZipReader &reader);
	bool InitializeInputData(ZipReader &reader);

public:
	BizhawkMovie();
	virtual ~BizhawkMovie();

	void RecordState(uint8_t port, uint8_t value) override;
	void Record(string filename, bool reset);

	uint8_t GetState(uint8_t port) override;

	bool Play(stringstream &filestream, bool autoLoadRom) override;

	bool IsRecording() override;
	bool IsPlaying() override;

	void ProcessNotification(ConsoleNotificationType type, void* parameter);
};