#include "stdafx.h"
#include <string>
#include <sstream>
#include <algorithm>
#include "LibretroRenderer.h"
#include "LibretroSoundManager.h"
#include "LibretroKeyManager.h"
#include "LibretroMessageManager.h"
#include "libretro.h"
#include "../Core/Console.h"
#include "../Core/VideoDecoder.h"
#include "../Core/VideoRenderer.h"
#include "../Core/EmulationSettings.h"
#include "../Core/CheatManager.h"
#include "../Core/HdData.h"
#include "../Core/SaveStateManager.h"
#include "../Core/DebuggerTypes.h"
#include "../Core/GameDatabase.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/HexUtilities.h"

#define DEVICE_AUTO               RETRO_DEVICE_JOYPAD
#define DEVICE_GAMEPAD            RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_JOYPAD, 0)
#define DEVICE_POWERPAD           RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_JOYPAD, 1)
#define DEVICE_FAMILYTRAINER      RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_JOYPAD, 2)
#define DEVICE_PARTYTAP           RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_JOYPAD, 3)
#define DEVICE_PACHINKO           RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_JOYPAD, 4)
#define DEVICE_EXCITINGBOXING     RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_JOYPAD, 5)
#define DEVICE_KONAMIHYPERSHOT    RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_JOYPAD, 6)
#define DEVICE_SNESGAMEPAD        RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_JOYPAD, 7)
#define DEVICE_ZAPPER             RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_POINTER, 0)
#define DEVICE_OEKAKIDS           RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_POINTER, 1)
#define DEVICE_BANDAIHYPERSHOT    RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_POINTER, 2)
#define DEVICE_ARKANOID           RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_MOUSE, 0)
#define DEVICE_HORITRACK          RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_MOUSE, 1)
#define DEVICE_SNESMOUSE          RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_MOUSE, 2)
#define DEVICE_ASCIITURBOFILE     RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_NONE, 0)
#define DEVICE_BATTLEBOX          RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_NONE, 1)
#define DEVICE_FOURPLAYERADAPTER  RETRO_DEVICE_SUBCLASS(RETRO_DEVICE_NONE, 2)

static retro_log_printf_t logCallback = nullptr;
static retro_environment_t retroEnv = nullptr;
static unsigned _inputDevices[5] = { DEVICE_AUTO, DEVICE_AUTO, DEVICE_AUTO, DEVICE_AUTO, DEVICE_AUTO };
static bool _hdPacksEnabled = false;
static string _mesenVersion = "";
static int32_t _saveStateSize = -1;
static struct retro_memory_descriptor _descriptors[3];
static struct retro_memory_map _memoryMap;
static bool _shiftButtonsClockwise = false;
static int32_t _audioSampleRate = 44100;

//Include game database as a byte array (representing the MesenDB.txt file)
#include "MesenDB.inc"

static std::shared_ptr<Console> _console;
static std::unique_ptr<LibretroRenderer> _renderer;
static std::unique_ptr<LibretroSoundManager> _soundManager;
static std::unique_ptr<LibretroKeyManager> _keyManager;
static std::unique_ptr<LibretroMessageManager> _messageManager;

static constexpr const char* MesenNtscFilter = "mesen_ntsc_filter";
static constexpr const char* MesenPalette = "mesen_palette";
static constexpr const char* MesenNoSpriteLimit = "mesen_nospritelimit";
static constexpr const char* MesenOverclock = "mesen_overclock";
static constexpr const char* MesenOverclockType = "mesen_overclock_type";
static constexpr const char* MesenOverscanVertical = "mesen_overscan_vertical";
static constexpr const char* MesenOverscanHorizontal = "mesen_overscan_horizontal";
static constexpr const char* MesenAspectRatio = "mesen_aspect_ratio";
static constexpr const char* MesenRegion = "mesen_region";
static constexpr const char* MesenRamState = "mesen_ramstate";
static constexpr const char* MesenControllerTurboSpeed = "mesen_controllerturbospeed";
static constexpr const char* MesenFdsAutoSelectDisk = "mesen_fdsautoinsertdisk";
static constexpr const char* MesenFdsFastForwardLoad = "mesen_fdsfastforwardload";
static constexpr const char* MesenHdPacks = "mesen_hdpacks";
static constexpr const char* MesenScreenRotation = "mesen_screenrotation";
static constexpr const char* MesenFakeStereo = "mesen_fake_stereo";
static constexpr const char* MesenMuteTriangleUltrasonic = "mesen_mute_triangle_ultrasonic";
static constexpr const char* MesenReduceDmcPopping = "mesen_reduce_dmc_popping";
static constexpr const char* MesenSwapDutyCycle = "mesen_swap_duty_cycle";
static constexpr const char* MesenDisableNoiseModeFlag = "mesen_disable_noise_mode_flag";
static constexpr const char* MesenShiftButtonsClockwise = "mesen_shift_buttons_clockwise";
static constexpr const char* MesenAudioSampleRate = "mesen_audio_sample_rate";

uint32_t defaultPalette[0x40] { 0xFF666666, 0xFF002A88, 0xFF1412A7, 0xFF3B00A4, 0xFF5C007E, 0xFF6E0040, 0xFF6C0600, 0xFF561D00, 0xFF333500, 0xFF0B4800, 0xFF005200, 0xFF004F08, 0xFF00404D, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFADADAD, 0xFF155FD9, 0xFF4240FF, 0xFF7527FE, 0xFFA01ACC, 0xFFB71E7B, 0xFFB53120, 0xFF994E00, 0xFF6B6D00, 0xFF388700, 0xFF0C9300, 0xFF008F32, 0xFF007C8D, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFEFF, 0xFF64B0FF, 0xFF9290FF, 0xFFC676FF, 0xFFF36AFF, 0xFFFE6ECC, 0xFFFE8170, 0xFFEA9E22, 0xFFBCBE00, 0xFF88D800, 0xFF5CE430, 0xFF45E082, 0xFF48CDDE, 0xFF4F4F4F, 0xFF000000, 0xFF000000, 0xFFFFFEFF, 0xFFC0DFFF, 0xFFD3D2FF, 0xFFE8C8FF, 0xFFFBC2FF, 0xFFFEC4EA, 0xFFFECCC5, 0xFFF7D8A5, 0xFFE4E594, 0xFFCFEF96, 0xFFBDF4AB, 0xFFB3F3CC, 0xFFB5EBF2, 0xFFB8B8B8, 0xFF000000, 0xFF000000 };
uint32_t unsaturatedPalette[0x40] { 0xFF6B6B6B, 0xFF001E87, 0xFF1F0B96, 0xFF3B0C87, 0xFF590D61, 0xFF5E0528, 0xFF551100, 0xFF461B00, 0xFF303200, 0xFF0A4800, 0xFF004E00, 0xFF004619, 0xFF003A58, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFB2B2B2, 0xFF1A53D1, 0xFF4835EE, 0xFF7123EC, 0xFF9A1EB7, 0xFFA51E62, 0xFFA52D19, 0xFF874B00, 0xFF676900, 0xFF298400, 0xFF038B00, 0xFF008240, 0xFF007891, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF63ADFD, 0xFF908AFE, 0xFFB977FC, 0xFFE771FE, 0xFFF76FC9, 0xFFF5836A, 0xFFDD9C29, 0xFFBDB807, 0xFF84D107, 0xFF5BDC3B, 0xFF48D77D, 0xFF48CCCE, 0xFF555555, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFC4E3FE, 0xFFD7D5FE, 0xFFE6CDFE, 0xFFF9CAFE, 0xFFFEC9F0, 0xFFFED1C7, 0xFFF7DCAC, 0xFFE8E89C, 0xFFD1F29D, 0xFFBFF4B1, 0xFFB7F5CD, 0xFFB7F0EE, 0xFFBEBEBE, 0xFF000000, 0xFF000000 };
uint32_t yuvPalette[0x40] { 0xFF666666, 0xFF002A88, 0xFF1412A7, 0xFF3B00A4, 0xFF5C007E, 0xFF6E0040, 0xFF6C0700, 0xFF561D00, 0xFF333500, 0xFF0C4800, 0xFF005200, 0xFF004C18, 0xFF003E5B, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFADADAD, 0xFF155FD9, 0xFF4240FF, 0xFF7527FE, 0xFFA01ACC, 0xFFB71E7B, 0xFFB53120, 0xFF994E00, 0xFF6B6D00, 0xFF388700, 0xFF0D9300, 0xFF008C47, 0xFF007AA0, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF64B0FF, 0xFF9290FF, 0xFFC676FF, 0xFFF26AFF, 0xFFFF6ECC, 0xFFFF8170, 0xFFEA9E22, 0xFFBCBE00, 0xFF88D800, 0xFF5CE430, 0xFF45E082, 0xFF48CDDE, 0xFF4F4F4F, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFC0DFFF, 0xFFD3D2FF, 0xFFE8C8FF, 0xFFFAC2FF, 0xFFFFC4EA, 0xFFFFCCC5, 0xFFF7D8A5, 0xFFE4E594, 0xFFCFEF96, 0xFFBDF4AB, 0xFFB3F3CC, 0xFFB5EBF2, 0xFFB8B8B8, 0xFF000000, 0xFF000000 };
uint32_t nestopiaRgbPalette[0x40] { 0xFF6D6D6D, 0xFF002492, 0xFF0000DB, 0xFF6D49DB, 0xFF92006D, 0xFFB6006D, 0xFFB62400, 0xFF924900, 0xFF6D4900, 0xFF244900, 0xFF006D24, 0xFF009200, 0xFF004949, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFB6B6B6, 0xFF006DDB, 0xFF0049FF, 0xFF9200FF, 0xFFB600FF, 0xFFFF0092, 0xFFFF0000, 0xFFDB6D00, 0xFF926D00, 0xFF249200, 0xFF009200, 0xFF00B66D, 0xFF009292, 0xFF242424, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF6DB6FF, 0xFF9292FF, 0xFFDB6DFF, 0xFFFF00FF, 0xFFFF6DFF, 0xFFFF9200, 0xFFFFB600, 0xFFDBDB00, 0xFF6DDB00, 0xFF00FF00, 0xFF49FFDB, 0xFF00FFFF, 0xFF494949, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFB6DBFF, 0xFFDBB6FF, 0xFFFFB6FF, 0xFFFF92FF, 0xFFFFB6B6, 0xFFFFDB92, 0xFFFFFF49, 0xFFFFFF6D, 0xFFB6FF49, 0xFF92FF6D, 0xFF49FFDB, 0xFF92DBFF, 0xFF929292, 0xFF000000, 0xFF000000 };
uint32_t compositeDirectPalette[0x40] { 0xFF656565, 0xFF00127D, 0xFF18008E, 0xFF360082, 0xFF56005D, 0xFF5A0018, 0xFF4F0500, 0xFF381900, 0xFF1D3100, 0xFF003D00, 0xFF004100, 0xFF003B17, 0xFF002E55, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFAFAFAF, 0xFF194EC8, 0xFF472FE3, 0xFF6B1FD7, 0xFF931BAE, 0xFF9E1A5E, 0xFF993200, 0xFF7B4B00, 0xFF5B6700, 0xFF267A00, 0xFF008200, 0xFF007A3E, 0xFF006E8A, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF64A9FF, 0xFF8E89FF, 0xFFB676FF, 0xFFE06FFF, 0xFFEF6CC4, 0xFFF0806A, 0xFFD8982C, 0xFFB9B40A, 0xFF83CB0C, 0xFF5BD63F, 0xFF4AD17E, 0xFF4DC7CB, 0xFF4C4C4C, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFC7E5FF, 0xFFD9D9FF, 0xFFE9D1FF, 0xFFF9CEFF, 0xFFFFCCF1, 0xFFFFD4CB, 0xFFF8DFB1, 0xFFEDEAA4, 0xFFD6F4A4, 0xFFC5F8B8, 0xFFBEF6D3, 0xFFBFF1F1, 0xFFB9B9B9, 0xFF000000, 0xFF000000 };
uint32_t nesClassicPalette[0x40] { 0xFF60615F, 0xFF000083, 0xFF1D0195, 0xFF340875, 0xFF51055E, 0xFF56000F, 0xFF4C0700, 0xFF372308, 0xFF203A0B, 0xFF0F4B0E, 0xFF194C16, 0xFF02421E, 0xFF023154, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFA9AAA8, 0xFF104BBF, 0xFF4712D8, 0xFF6300CA, 0xFF8800A9, 0xFF930B46, 0xFF8A2D04, 0xFF6F5206, 0xFF5C7114, 0xFF1B8D12, 0xFF199509, 0xFF178448, 0xFF206B8E, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFBFBFB, 0xFF6699F8, 0xFF8974F9, 0xFFAB58F8, 0xFFD557EF, 0xFFDE5FA9, 0xFFDC7F59, 0xFFC7A224, 0xFFA7BE03, 0xFF75D703, 0xFF60E34F, 0xFF3CD68D, 0xFF56C9CC, 0xFF414240, 0xFF000000, 0xFF000000, 0xFFFBFBFB, 0xFFBED4FA, 0xFFC9C7F9, 0xFFD7BEFA, 0xFFE8B8F9, 0xFFF5BAE5, 0xFFF3CAC2, 0xFFDFCDA7, 0xFFD9E09C, 0xFFC9EB9E, 0xFFC0EDB8, 0xFFB5F4C7, 0xFFB9EAE9, 0xFFABABAB, 0xFF000000, 0xFF000000 };
uint32_t originalHardwarePalette[0x40] { 0xFF6A6D6A, 0xFF00127D, 0xFF1E008A, 0xFF3B007D, 0xFF56005D, 0xFF5A0018, 0xFF4F0D00, 0xFF381E00, 0xFF203100, 0xFF003D00, 0xFF004000, 0xFF003B1E, 0xFF002E55, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFB9BCB9, 0xFF194EC8, 0xFF472FE3, 0xFF751FD7, 0xFF931EAD, 0xFF9E245E, 0xFF963800, 0xFF7B5000, 0xFF5B6700, 0xFF267A00, 0xFF007F00, 0xFF007842, 0xFF006E8A, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF69AEFF, 0xFF9798FF, 0xFFB687FF, 0xFFE278FF, 0xFFF279C7, 0xFFF58F6F, 0xFFDDA932, 0xFFBCB70D, 0xFF88D015, 0xFF60DB49, 0xFF4FD687, 0xFF50CACE, 0xFF515451, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFCCEAFF, 0xFFDEE2FF, 0xFFEEDAFF, 0xFFFAD7FD, 0xFFFDD7F6, 0xFFFDDCD0, 0xFFFAE8B6, 0xFFF2F1A9, 0xFFDBFBA9, 0xFFCAFFBD, 0xFFC3FBD8, 0xFFC4F6F6, 0xFFBEC1BE, 0xFF000000, 0xFF000000 };
uint32_t pvmStylePalette[0x40] { 0xFF696964, 0xFF001774, 0xFF28007D, 0xFF3E006D, 0xFF560057, 0xFF5E0013, 0xFF531A00, 0xFF3B2400, 0xFF2A3000, 0xFF143A00, 0xFF003F00, 0xFF003B1E, 0xFF003050, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFB9B9B4, 0xFF1453B9, 0xFF4D2CDA, 0xFF7A1EC8, 0xFF98189C, 0xFF9D2344, 0xFFA03E00, 0xFF8D5500, 0xFF656D00, 0xFF2C7900, 0xFF008100, 0xFF007D42, 0xFF00788A, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF69A8FF, 0xFF9A96FF, 0xFFC28AFA, 0xFFEA7DFA, 0xFFF387B4, 0xFFF1986C, 0xFFE6B327, 0xFFD7C805, 0xFF90DF07, 0xFF64E53C, 0xFF45E27D, 0xFF48D5D9, 0xFF4B4B46, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFD2EAFF, 0xFFE2E2FF, 0xFFF2D8FF, 0xFFF8D2FF, 0xFFF8D9EA, 0xFFFADEB9, 0xFFF9E89B, 0xFFF3F28C, 0xFFD3FA91, 0xFFB8FCA8, 0xFFAEFACA, 0xFFCAF3F3, 0xFFBEBEB9, 0xFF000000, 0xFF000000 };
uint32_t sonyCxa2025AsPalette[0x40] { 0xFF585858, 0xFF00238C, 0xFF00139B, 0xFF2D0585, 0xFF5D0052, 0xFF7A0017, 0xFF7A0800, 0xFF5F1800, 0xFF352A00, 0xFF093900, 0xFF003F00, 0xFF003C22, 0xFF00325D, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFA1A1A1, 0xFF0053EE, 0xFF153CFE, 0xFF6028E4, 0xFFA91D98, 0xFFD41E41, 0xFFD22C00, 0xFFAA4400, 0xFF6C5E00, 0xFF2D7300, 0xFF007D06, 0xFF007852, 0xFF0069A9, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF1FA5FE, 0xFF5E89FE, 0xFFB572FE, 0xFFFE65F6, 0xFFFE6790, 0xFFFE773C, 0xFFFE9308, 0xFFC4B200, 0xFF79CA10, 0xFF3AD54A, 0xFF11D1A4, 0xFF06BFFE, 0xFF424242, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFA0D9FE, 0xFFBDCCFE, 0xFFE1C2FE, 0xFFFEBCFB, 0xFFFEBDD0, 0xFFFEC5A9, 0xFFFED18E, 0xFFE9DE86, 0xFFC7E992, 0xFFA8EEB0, 0xFF95ECD9, 0xFF91E4FE, 0xFFACACAC, 0xFF000000, 0xFF000000 };
uint32_t wavebeamPalette[0x40] { 0xFF6B6B6B, 0xFF001B88, 0xFF21009A, 0xFF40008C, 0xFF600067, 0xFF64001E, 0xFF590800, 0xFF481600, 0xFF283600, 0xFF004500, 0xFF004908, 0xFF00421D, 0xFF003659, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFB4B4B4, 0xFF1555D3, 0xFF4337EF, 0xFF7425DF, 0xFF9C19B9, 0xFFAC0F64, 0xFFAA2C00, 0xFF8A4B00, 0xFF666B00, 0xFF218300, 0xFF008A00, 0xFF008144, 0xFF007691, 0xFF000000, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFF63B2FF, 0xFF7C9CFF, 0xFFC07DFE, 0xFFE977FF, 0xFFF572CD, 0xFFF4886B, 0xFFDDA029, 0xFFBDBD0A, 0xFF89D20E, 0xFF5CDE3E, 0xFF4BD886, 0xFF4DCFD2, 0xFF525252, 0xFF000000, 0xFF000000, 0xFFFFFFFF, 0xFFBCDFFF, 0xFFD2D2FF, 0xFFE1C8FF, 0xFFEFC7FF, 0xFFFFC3E1, 0xFFFFCAC6, 0xFFF2DAAD, 0xFFEBE3A0, 0xFFD2EDA2, 0xFFBCF4B4, 0xFFB5F1CE, 0xFFB6ECF1, 0xFFBFBFBF, 0xFF000000, 0xFF000000 };

extern "C" {
	void logMessage(retro_log_level level, const char* message)
	{
		if(logCallback) {
			logCallback(level, message);
		}
	}

	RETRO_API unsigned retro_api_version()
	{
		return RETRO_API_VERSION;
	}

	RETRO_API void retro_init()
	{
		struct retro_log_callback log;
		if(retroEnv(RETRO_ENVIRONMENT_GET_LOG_INTERFACE, &log)) {
			logCallback = log.log;
		} else {
			logCallback = nullptr;
		}

		_console.reset(new Console());
		_console->Init();

		_renderer.reset(new LibretroRenderer(_console, retroEnv));
		_soundManager.reset(new LibretroSoundManager(_console));
		_keyManager.reset(new LibretroKeyManager(_console));
		_messageManager.reset(new LibretroMessageManager(logCallback, retroEnv));

		std::stringstream databaseData;
		databaseData.write((const char*)MesenDatabase, sizeof(MesenDatabase));
		GameDatabase::LoadGameDb(databaseData);

		_console->GetSettings()->SetFlags(EmulationFlags::FdsAutoLoadDisk);
		_console->GetSettings()->SetFlags(EmulationFlags::AutoConfigureInput);
		_console->GetSettings()->SetSampleRate(_audioSampleRate);
		_console->GetSettings()->SetAutoSaveOptions(0, false);
		_console->GetSettings()->SetRewindBufferSize(0);
	}

	RETRO_API void retro_deinit()
	{
		_renderer.reset();
		_soundManager.reset();
		_keyManager.reset();
		_messageManager.reset();

		_console->SaveBatteries();
		_console->Release(true);
		_console.reset();
	}

	RETRO_API void retro_set_environment(retro_environment_t env)
	{
		retroEnv = env;

		static constexpr struct retro_variable vars[] = {
			{ MesenNtscFilter, "NTSC filter; Disabled|Composite (Blargg)|S-Video (Blargg)|RGB (Blargg)|Monochrome (Blargg)|Bisqwit 2x|Bisqwit 4x|Bisqwit 8x" },
			{ MesenPalette, "Palette; Default|Composite Direct (by FirebrandX)|Nes Classic|Nestopia (RGB)|Original Hardware (by FirebrandX)|PVM Style (by FirebrandX)|Sony CXA2025AS|Unsaturated v6 (by FirebrandX)|YUV v3 (by FirebrandX)|Wavebeam (by nakedarthur)|Custom|Raw" },
			{ MesenOverclock, "Overclock; None|Low|Medium|High|Very High" },
			{ MesenOverclockType, "Overclock Type; Before NMI (Recommended)|After NMI" },
			{ MesenRegion, "Region; Auto|NTSC|PAL|Dendy" },
			{ MesenOverscanVertical, "Vertical Overscan; None|8px|16px" },
			{ MesenOverscanHorizontal, "Horizontal Overscan; None|8px|16px" },
			{ MesenAspectRatio, "Aspect Ratio; Auto|No Stretching|NTSC|PAL|4:3|16:9" },
			{ MesenControllerTurboSpeed, "Controller Turbo Speed; Fast|Very Fast|Disabled|Slow|Normal" },
			{ MesenShiftButtonsClockwise, u8"Shift A/B/X/Y clockwise; disabled|enabled" },
			{ MesenHdPacks, "Enable HD Packs; enabled|disabled" },
			{ MesenNoSpriteLimit, "Remove sprite limit; enabled|disabled" },
			{ MesenFakeStereo, u8"Enable fake stereo effect; disabled|enabled" },
			{ MesenMuteTriangleUltrasonic, u8"Reduce popping on Triangle channel; enabled|disabled" },
			{ MesenReduceDmcPopping, u8"Reduce popping on DMC channel; enabled|disabled" },
			{ MesenSwapDutyCycle, u8"Swap Square channel duty cycles; disabled|enabled" },
			{ MesenDisableNoiseModeFlag, u8"Disable Noise channel mode flag; disabled|enabled" },
			{ MesenScreenRotation, u8"Screen Rotation; None|90 degrees|180 degrees|270 degrees" },
			{ MesenRamState, "Default power-on state for RAM; All 0s (Default)|All 1s|Random Values" },
			{ MesenFdsAutoSelectDisk, "FDS: Automatically insert disks; disabled|enabled" },
			{ MesenFdsFastForwardLoad, "FDS: Fast forward while loading; disabled|enabled" },
			{ MesenAudioSampleRate, "Sound Output Sample Rate; 96000|192000|384000|11025|22050|44100|48000" },
			{ NULL, NULL },
		};

		static constexpr struct retro_controller_description pads1[] = {
			{ "Auto", DEVICE_AUTO },
			{ "Standard Controller", DEVICE_GAMEPAD },
			{ "Zapper", DEVICE_ZAPPER },
			{ "Power Pad", DEVICE_POWERPAD },
			{ "Arkanoid", DEVICE_ARKANOID },
			{ "SNES Controller", DEVICE_SNESGAMEPAD },
			{ "SNES Mouse", DEVICE_SNESMOUSE },
			{ NULL, 0 },
		};

		static constexpr struct retro_controller_description pads2[] = {
			{ "Auto", DEVICE_AUTO },
			{ "Standard Controller", DEVICE_GAMEPAD },
			{ "Zapper", DEVICE_ZAPPER },
			{ "Power Pad", DEVICE_POWERPAD },
			{ "Arkanoid", DEVICE_ARKANOID },
			{ "SNES Controller", DEVICE_SNESGAMEPAD },
			{ "SNES Mouse", DEVICE_SNESMOUSE },
			{ NULL, 0 },
		};

		static constexpr struct retro_controller_description pads3[] = {
			{ "Auto", DEVICE_AUTO },
			{ "Standard Controller", DEVICE_GAMEPAD },
			{ NULL, 0 },
		};

		static constexpr struct retro_controller_description pads4[] = {
			{ "Auto", DEVICE_AUTO },
			{ "Standard Controller", DEVICE_GAMEPAD },
			{ NULL, 0 },
		};
		
		static constexpr struct retro_controller_description pads5[] = {
			{ "Auto",     RETRO_DEVICE_JOYPAD },
			{ "Arkanoid", DEVICE_ARKANOID },
			{ "Ascii Turbo File", DEVICE_ASCIITURBOFILE },
			{ "Bandai Hypershot", DEVICE_BANDAIHYPERSHOT },
			{ "Battle Box", DEVICE_BATTLEBOX },
			{ "Exciting Boxing", DEVICE_EXCITINGBOXING },
			{ "Family Trainer", DEVICE_FAMILYTRAINER },
			{ "Four Player Adapter", DEVICE_FOURPLAYERADAPTER },
			{ "Hori Track", DEVICE_HORITRACK },
			{ "Konami Hypershot", DEVICE_KONAMIHYPERSHOT },
			{ "Pachinko", DEVICE_PACHINKO },
			{ "Partytap", DEVICE_PARTYTAP },
			{ "Oeka Kids Tablet", DEVICE_OEKAKIDS },			
			{ NULL, 0 },
		};
		
		static constexpr struct retro_controller_info ports[] = {
			{ pads1, 7 },
			{ pads2, 7 },
			{ pads3, 2 },
			{ pads4, 2 },
			{ pads5, 13 },
			{ 0 },
		};

		retroEnv(RETRO_ENVIRONMENT_SET_VARIABLES, (void*)vars);
		retroEnv(RETRO_ENVIRONMENT_SET_CONTROLLER_INFO, (void*)ports);
	}

	RETRO_API void retro_set_video_refresh(retro_video_refresh_t sendFrame)
	{
		_renderer->SetVideoCallback(sendFrame);
	}

	RETRO_API void retro_set_audio_sample(retro_audio_sample_t sendAudioSample)
	{
	}

	RETRO_API void retro_set_audio_sample_batch(retro_audio_sample_batch_t audioSampleBatch)
	{
		_soundManager->SetSendAudioSample(audioSampleBatch);
	}

	RETRO_API void retro_set_input_poll(retro_input_poll_t pollInput)
	{	
		_keyManager->SetPollInput(pollInput);
	}

	RETRO_API void retro_set_input_state(retro_input_state_t getInputState)
	{
		_keyManager->SetGetInputState(getInputState);
	}

	RETRO_API void retro_reset()
	{
		_console->Reset(true);
	}

	bool readVariable(const char* key, retro_variable &var)
	{
		var.key = key;
		var.value = nullptr;
		if(retroEnv(RETRO_ENVIRONMENT_GET_VARIABLE, &var) && var.value != nullptr) {
			return true;
		}
		return false;
	}

	void set_flag(const char* flagName, uint64_t flagValue)
	{
		struct retro_variable var = {};
		if(readVariable(flagName, var)) {
			string value = string(var.value);
			if(value == "disabled") {
				_console->GetSettings()->ClearFlags(flagValue);
			} else {
				_console->GetSettings()->SetFlags(flagValue);
			}
		}
	}

	void load_custom_palette()
	{
		//Setup default palette in case we can't load the custom one
		_console->GetSettings()->SetUserRgbPalette(defaultPalette);

		//Try to load the custom palette from the MesenPalette.pal file
		string palettePath = FolderUtilities::CombinePath(FolderUtilities::GetHomeFolder(), "MesenPalette.pal");
		uint8_t fileData[512 * 3] = {};
		ifstream palette(palettePath, ios::binary);
		if(palette) {
			palette.seekg(0, ios::end);
			std::streamoff fileSize = palette.tellg();
			palette.seekg(0, ios::beg);
			if((fileSize == 64 * 3) || (fileSize == 512 * 3)) {
				palette.read((char*)fileData, fileSize);
				uint32_t customPalette[512];
				for(int i = 0; i < fileSize / 3; i++) {
					customPalette[i] = 0xFF000000 | fileData[i * 3 + 2] | (fileData[i * 3 + 1] << 8) | (fileData[i * 3] << 16);
				}
				_console->GetSettings()->SetUserRgbPalette(customPalette, (uint32_t)fileSize / 3);
			}
		}
	}

	void update_settings()
	{
		struct retro_variable var = { };
		_console->GetSettings()->SetPictureSettings(0, 0, 0, 0, 0);

		_hdPacksEnabled = _console->GetSettings()->CheckFlag(EmulationFlags::UseHdPacks);

		set_flag(MesenNoSpriteLimit, EmulationFlags::RemoveSpriteLimit | EmulationFlags::AdaptiveSpriteLimit);
		set_flag(MesenHdPacks, EmulationFlags::UseHdPacks);
		set_flag(MesenMuteTriangleUltrasonic, EmulationFlags::SilenceTriangleHighFreq);
		set_flag(MesenReduceDmcPopping, EmulationFlags::ReduceDmcPopping);
		set_flag(MesenSwapDutyCycle, EmulationFlags::SwapDutyCycles);
		set_flag(MesenDisableNoiseModeFlag, EmulationFlags::DisableNoiseModeFlag);
		set_flag(MesenFdsAutoSelectDisk, EmulationFlags::FdsAutoInsertDisk);
		set_flag(MesenFdsFastForwardLoad, EmulationFlags::FdsFastForwardOnLoad);

		if(readVariable(MesenFakeStereo, var)) {
			string value = string(var.value);
			AudioFilterSettings settings;
			if(value == "enabled") {
				settings.Filter = StereoFilter::Delay;
				settings.Delay = 15;
				_console->GetSettings()->SetAudioFilterSettings(settings);
			} else {
				_console->GetSettings()->SetAudioFilterSettings(settings);
			}
		}
		
		if(readVariable(MesenNtscFilter, var)) {
			string value = string(var.value);
			if(value == "Disabled") {
				_console->GetSettings()->SetVideoFilterType(VideoFilterType::None);
			} else if(value == "Composite (Blargg)") {
				_console->GetSettings()->SetVideoFilterType(VideoFilterType::NTSC);
				_console->GetSettings()->SetNtscFilterSettings(0, 0, 0, 0, 0, 0, false, 0, 0, 0, false, true);
			} else if(value == "S-Video (Blargg)") {
				_console->GetSettings()->SetVideoFilterType(VideoFilterType::NTSC);
				_console->GetSettings()->SetNtscFilterSettings(-1.0, 0, -1.0, 0, 0.2, 0.2, false, 0, 0, 0, false, true);
			} else if(value == "RGB (Blargg)") {
				_console->GetSettings()->SetVideoFilterType(VideoFilterType::NTSC);
				_console->GetSettings()->SetPictureSettings(0, 0, 0, 0, 0);
				_console->GetSettings()->SetNtscFilterSettings(-1.0, -1.0, -1.0, 0, 0.7, 0.2, false, 0, 0, 0, false, true);
			} else if(value == "Monochrome (Blargg)") {
				_console->GetSettings()->SetVideoFilterType(VideoFilterType::NTSC);
				_console->GetSettings()->SetPictureSettings(0, 0, -1.0, 0, 0);
				_console->GetSettings()->SetNtscFilterSettings(-0.2, -0.1, -0.2, 0, 0.7, 0.2, false, 0, 0, 0, false, true);
			} else if(value == "Bisqwit 2x") {
				_console->GetSettings()->SetVideoFilterType(VideoFilterType::BisqwitNtscQuarterRes);
				_console->GetSettings()->SetNtscFilterSettings(0, 0, 0, 0, 0, 0, false, 0, 0, 0, false, true);
			} else if(value == "Bisqwit 4x") {
				_console->GetSettings()->SetVideoFilterType(VideoFilterType::BisqwitNtscHalfRes);
				_console->GetSettings()->SetNtscFilterSettings(0, 0, 0, 0, 0, 0, false, 0, 0, 0, false, true);
			} else if(value == "Bisqwit 8x") {
				_console->GetSettings()->SetVideoFilterType(VideoFilterType::BisqwitNtsc);
				_console->GetSettings()->SetNtscFilterSettings(0, 0, 0, 0, 0, 0, false, 0, 0, 0, false, true);
			}
		}

		if(readVariable(MesenPalette, var)) {
			string value = string(var.value);
			if(value == "Default") {
				_console->GetSettings()->SetUserRgbPalette(defaultPalette);
			} else if(value == "Composite Direct (by FirebrandX)") {
				_console->GetSettings()->SetUserRgbPalette(compositeDirectPalette);
			} else if(value == "Nes Classic") {
				_console->GetSettings()->SetUserRgbPalette(nesClassicPalette);
			} else if(value == "Nestopia (RGB)") {
				_console->GetSettings()->SetUserRgbPalette(nestopiaRgbPalette);
			} else if(value == "Original Hardware (by FirebrandX)") {
				_console->GetSettings()->SetUserRgbPalette(originalHardwarePalette);
			} else if(value == "PVM Style (by FirebrandX)") {
				_console->GetSettings()->SetUserRgbPalette(pvmStylePalette);
			} else if(value == "Sony CXA2025AS") {
				_console->GetSettings()->SetUserRgbPalette(sonyCxa2025AsPalette);
			} else if(value == "Unsaturated v6 (by FirebrandX)") {
				_console->GetSettings()->SetUserRgbPalette(unsaturatedPalette);
			} else if(value == "YUV v3 (by FirebrandX)") {
				_console->GetSettings()->SetUserRgbPalette(yuvPalette);
			} else if(value == "Wavebeam (by nakedarthur)") {
				_console->GetSettings()->SetUserRgbPalette(wavebeamPalette);
			} else if(value == "Custom") {
				load_custom_palette();
			} else if(value == "Raw") {
				//Using the raw palette replaces the NTSC filters, if one is selected
				_console->GetSettings()->SetVideoFilterType(VideoFilterType::Raw);
			}
		}

		bool beforeNmi = true;
		if(readVariable(MesenOverclockType, var)) {
			string value = string(var.value);
			if(value == "After NMI") {
				beforeNmi = false;
			}
		}

		if(readVariable(MesenOverclock, var)) {
			string value = string(var.value);
			int lineCount = 0;
			if(value == "None") {
				lineCount = 0;
			} else if(value == "Low") {
				lineCount = 100;
			} else if(value == "Medium") {
				lineCount = 250;
			} else if(value == "High") {
				lineCount = 500;
			} else if(value == "Very High") {
				lineCount = 1000;
			}

			if(beforeNmi) {
				_console->GetSettings()->SetPpuNmiConfig(lineCount, 0);
			} else {
				_console->GetSettings()->SetPpuNmiConfig(0, lineCount);
			}
		}

		int overscanHorizontal = 0;
		int overscanVertical = 0;		
		if(readVariable(MesenOverscanHorizontal, var)) {
			string value = string(var.value);
			if(value == "8px") {
				overscanHorizontal = 8;
			} else if(value == "16px") {
				overscanHorizontal = 16;
			}
		}

		if(readVariable(MesenOverscanVertical, var)) {
			string value = string(var.value);
			if(value == "8px") {
				overscanVertical = 8;
			} else if(value == "16px") {
				overscanVertical = 16;
			}
		}
		_console->GetSettings()->SetOverscanDimensions(overscanHorizontal, overscanHorizontal, overscanVertical, overscanVertical);

		if(readVariable(MesenAspectRatio, var)) {
			string value = string(var.value);
			if(value == "Auto") {
				_console->GetSettings()->SetVideoAspectRatio(VideoAspectRatio::Auto, 1.0);
			} else if(value == "No Stretching") {
				_console->GetSettings()->SetVideoAspectRatio(VideoAspectRatio::NoStretching, 1.0);
			} else if(value == "NTSC") {
				_console->GetSettings()->SetVideoAspectRatio(VideoAspectRatio::NTSC, 1.0);
			} else if(value == "PAL") {
				_console->GetSettings()->SetVideoAspectRatio(VideoAspectRatio::PAL, 1.0);
			} else if(value == "4:3") {
				_console->GetSettings()->SetVideoAspectRatio(VideoAspectRatio::Standard, 1.0);
			} else if(value == "16:9") {
				_console->GetSettings()->SetVideoAspectRatio(VideoAspectRatio::Widescreen, 1.0);
			}
		}

		if(readVariable(MesenRegion, var)) {
			string value = string(var.value);
			if(value == "Auto") {
				_console->GetSettings()->SetNesModel(NesModel::Auto);
			} else if(value == "NTSC") {
				_console->GetSettings()->SetNesModel(NesModel::NTSC);
			} else if(value == "PAL") {
				_console->GetSettings()->SetNesModel(NesModel::PAL);
			} else if(value == "Dendy") {
				_console->GetSettings()->SetNesModel(NesModel::Dendy);
			}
		}
		
		if(readVariable(MesenRamState, var)) {
			string value = string(var.value);
			if(value == "All 0s (Default)") {
				_console->GetSettings()->SetRamPowerOnState(RamPowerOnState::AllZeros);
			} else if(value == "All 1s") {
				_console->GetSettings()->SetRamPowerOnState(RamPowerOnState::AllOnes);
			} else if(value == "Random Values") {
				_console->GetSettings()->SetRamPowerOnState(RamPowerOnState::Random);
			}
		}

		if(readVariable(MesenScreenRotation, var)) {
			string value = string(var.value);
			if(value == "None") {
				_console->GetSettings()->SetScreenRotation(0);
			} else if(value == u8"90 degrees") {
				_console->GetSettings()->SetScreenRotation(90);
			} else if(value == u8"180 degrees") {
				_console->GetSettings()->SetScreenRotation(180);
			} else if(value == u8"270 degrees") {
				_console->GetSettings()->SetScreenRotation(270);
			}
		}

		int turboSpeed = 0;
		bool turboEnabled = true;
		if(readVariable(MesenControllerTurboSpeed, var)) {
			string value = string(var.value);
			if(value == "Slow") {
				turboSpeed = 0;
			} else if(value == "Normal") {
				turboSpeed = 1;
			} else if(value == "Fast") {
				turboSpeed = 2;
			} else if(value == "Very Fast") {
				turboSpeed = 3;
			} else if(value == "Disabled") {
				turboEnabled = false;
			}
		}

		_shiftButtonsClockwise = false;
		if(readVariable(MesenShiftButtonsClockwise, var)) {
			string value = string(var.value);
			if(value == "enabled") {
				_shiftButtonsClockwise = true;
 			}
		}

		if(readVariable(MesenAudioSampleRate, var)) {
			int old_value = _audioSampleRate;

			_audioSampleRate = atoi(var.value);

			if(old_value != _audioSampleRate) {
				_console->GetSettings()->SetSampleRate(_audioSampleRate);

				// switch when core actively running
				if(_saveStateSize != -1) {
					struct retro_system_av_info system_av_info;
					retro_get_system_av_info(&system_av_info);
					retroEnv(RETRO_ENVIRONMENT_SET_SYSTEM_AV_INFO, &system_av_info);
				}
			}
		}

		auto getKeyCode = [=](int port, int retroKey) {
			return (port << 8) | (retroKey + 1);
		};

		auto getKeyBindings = [=](int port) {
			KeyMappingSet keyMappings;
			keyMappings.TurboSpeed = turboSpeed;
			if(_console->GetSettings()->GetControllerType(port) == ControllerType::SnesController) {
				keyMappings.Mapping1.LButton = getKeyCode(port, RETRO_DEVICE_ID_JOYPAD_L);
				keyMappings.Mapping1.RButton = getKeyCode(port, RETRO_DEVICE_ID_JOYPAD_R);
				keyMappings.Mapping1.A = getKeyCode(port, RETRO_DEVICE_ID_JOYPAD_A);
				keyMappings.Mapping1.B = getKeyCode(port, RETRO_DEVICE_ID_JOYPAD_B);
				keyMappings.Mapping1.TurboA = getKeyCode(port, RETRO_DEVICE_ID_JOYPAD_X);
				keyMappings.Mapping1.TurboB = getKeyCode(port, RETRO_DEVICE_ID_JOYPAD_Y);
			} else {
				keyMappings.Mapping1.A = getKeyCode(port, _shiftButtonsClockwise ? RETRO_DEVICE_ID_JOYPAD_B : RETRO_DEVICE_ID_JOYPAD_A);
				keyMappings.Mapping1.B = getKeyCode(port, _shiftButtonsClockwise ? RETRO_DEVICE_ID_JOYPAD_Y : RETRO_DEVICE_ID_JOYPAD_B);
				if(turboEnabled) {
					keyMappings.Mapping1.TurboA = getKeyCode(port, _shiftButtonsClockwise ? RETRO_DEVICE_ID_JOYPAD_A : RETRO_DEVICE_ID_JOYPAD_X);
					keyMappings.Mapping1.TurboB = getKeyCode(port, _shiftButtonsClockwise ? RETRO_DEVICE_ID_JOYPAD_X : RETRO_DEVICE_ID_JOYPAD_Y);
				}
			}

			keyMappings.Mapping1.Start = getKeyCode(port, RETRO_DEVICE_ID_JOYPAD_START);
			keyMappings.Mapping1.Select = getKeyCode(port, RETRO_DEVICE_ID_JOYPAD_SELECT);

			keyMappings.Mapping1.Up = getKeyCode(port, RETRO_DEVICE_ID_JOYPAD_UP);
			keyMappings.Mapping1.Down = getKeyCode(port, RETRO_DEVICE_ID_JOYPAD_DOWN);
			keyMappings.Mapping1.Left = getKeyCode(port, RETRO_DEVICE_ID_JOYPAD_LEFT);
			keyMappings.Mapping1.Right = getKeyCode(port, RETRO_DEVICE_ID_JOYPAD_RIGHT);

			if(port == 0) {
				keyMappings.Mapping1.PartyTapButtons[0] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_B);
				keyMappings.Mapping1.PartyTapButtons[1] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_A);
				keyMappings.Mapping1.PartyTapButtons[2] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_Y);
				keyMappings.Mapping1.PartyTapButtons[3] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_X);
				keyMappings.Mapping1.PartyTapButtons[4] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_L);
				keyMappings.Mapping1.PartyTapButtons[5] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_R);

				unsigned powerPadPort = 0;
				if(_console->GetSettings()->GetExpansionDevice() == ExpansionPortDevice::FamilyTrainerMat) {
					powerPadPort = 4;
				}

				keyMappings.Mapping1.PowerPadButtons[0] = getKeyCode(powerPadPort, RETRO_DEVICE_ID_JOYPAD_B);
				keyMappings.Mapping1.PowerPadButtons[1] = getKeyCode(powerPadPort, RETRO_DEVICE_ID_JOYPAD_A);
				keyMappings.Mapping1.PowerPadButtons[2] = getKeyCode(powerPadPort, RETRO_DEVICE_ID_JOYPAD_Y);
				keyMappings.Mapping1.PowerPadButtons[3] = getKeyCode(powerPadPort, RETRO_DEVICE_ID_JOYPAD_X);
				keyMappings.Mapping1.PowerPadButtons[4] = getKeyCode(powerPadPort, RETRO_DEVICE_ID_JOYPAD_L);
				keyMappings.Mapping1.PowerPadButtons[5] = getKeyCode(powerPadPort, RETRO_DEVICE_ID_JOYPAD_R);
				keyMappings.Mapping1.PowerPadButtons[6] = getKeyCode(powerPadPort, RETRO_DEVICE_ID_JOYPAD_LEFT);
				keyMappings.Mapping1.PowerPadButtons[7] = getKeyCode(powerPadPort, RETRO_DEVICE_ID_JOYPAD_RIGHT);
				keyMappings.Mapping1.PowerPadButtons[8] = getKeyCode(powerPadPort, RETRO_DEVICE_ID_JOYPAD_UP);
				keyMappings.Mapping1.PowerPadButtons[9] = getKeyCode(powerPadPort, RETRO_DEVICE_ID_JOYPAD_DOWN);
				keyMappings.Mapping1.PowerPadButtons[10] = getKeyCode(powerPadPort, RETRO_DEVICE_ID_JOYPAD_SELECT);
				keyMappings.Mapping1.PowerPadButtons[11] = getKeyCode(powerPadPort, RETRO_DEVICE_ID_JOYPAD_START);

				keyMappings.Mapping1.PachinkoButtons[0] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_R);
				keyMappings.Mapping1.PachinkoButtons[1] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_L);

				keyMappings.Mapping1.ExcitingBoxingButtons[0] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_B); //left hook
				keyMappings.Mapping1.ExcitingBoxingButtons[1] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_RIGHT); //move right
				keyMappings.Mapping1.ExcitingBoxingButtons[2] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_LEFT); //move left
				keyMappings.Mapping1.ExcitingBoxingButtons[3] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_A); //right hook
				keyMappings.Mapping1.ExcitingBoxingButtons[4] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_Y); //left jab
				keyMappings.Mapping1.ExcitingBoxingButtons[5] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_L); //body
				keyMappings.Mapping1.ExcitingBoxingButtons[6] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_X); //right jab
				keyMappings.Mapping1.ExcitingBoxingButtons[7] = getKeyCode(4, RETRO_DEVICE_ID_JOYPAD_R); //straight
			} else if(port == 1) {
				keyMappings.Mapping1.Microphone = getKeyCode(0, RETRO_DEVICE_ID_JOYPAD_L3);
				keyMappings.Mapping1.PowerPadButtons[0] = getKeyCode(1, RETRO_DEVICE_ID_JOYPAD_B);
				keyMappings.Mapping1.PowerPadButtons[1] = getKeyCode(1, RETRO_DEVICE_ID_JOYPAD_A);
				keyMappings.Mapping1.PowerPadButtons[2] = getKeyCode(1, RETRO_DEVICE_ID_JOYPAD_Y);
				keyMappings.Mapping1.PowerPadButtons[3] = getKeyCode(1, RETRO_DEVICE_ID_JOYPAD_X);
				keyMappings.Mapping1.PowerPadButtons[4] = getKeyCode(1, RETRO_DEVICE_ID_JOYPAD_L);
				keyMappings.Mapping1.PowerPadButtons[5] = getKeyCode(1, RETRO_DEVICE_ID_JOYPAD_R);
				keyMappings.Mapping1.PowerPadButtons[6] = getKeyCode(1, RETRO_DEVICE_ID_JOYPAD_LEFT);
				keyMappings.Mapping1.PowerPadButtons[7] = getKeyCode(1, RETRO_DEVICE_ID_JOYPAD_RIGHT);
				keyMappings.Mapping1.PowerPadButtons[8] = getKeyCode(1, RETRO_DEVICE_ID_JOYPAD_UP);
				keyMappings.Mapping1.PowerPadButtons[9] = getKeyCode(1, RETRO_DEVICE_ID_JOYPAD_DOWN);
				keyMappings.Mapping1.PowerPadButtons[10] = getKeyCode(1, RETRO_DEVICE_ID_JOYPAD_SELECT);
				keyMappings.Mapping1.PowerPadButtons[11] = getKeyCode(1, RETRO_DEVICE_ID_JOYPAD_START);
			}
			return keyMappings;
		};

		_console->GetSettings()->SetControllerKeys(0, getKeyBindings(0));
		_console->GetSettings()->SetControllerKeys(1, getKeyBindings(1));
		_console->GetSettings()->SetControllerKeys(2, getKeyBindings(2));
		_console->GetSettings()->SetControllerKeys(3, getKeyBindings(3));

		retro_system_av_info avInfo = {};
		_renderer->GetSystemAudioVideoInfo(avInfo);
		retroEnv(RETRO_ENVIRONMENT_SET_GEOMETRY, &avInfo);
	}

	RETRO_API void retro_run()
	{
		if(_console->GetSettings()->CheckFlag(EmulationFlags::ForceMaxSpeed)) {
			//Skip frames to speed up emulation while still outputting at 50/60 fps (needed for FDS fast forward while loading)
			_renderer->SetSkipMode(true);
			_soundManager->SetSkipMode(true);
			for(int i = 0; i < 9; i++) {
				//Attempt to speed up to 1000% speed
				_console->RunSingleFrame();
			}
			_renderer->SetSkipMode(false);
			_soundManager->SetSkipMode(false);
		}

		bool updated = false;
		if(retroEnv(RETRO_ENVIRONMENT_GET_VARIABLE_UPDATE, &updated) && updated) {
			update_settings();

			bool hdPacksEnabled = _console->GetSettings()->CheckFlag(EmulationFlags::UseHdPacks);
			if(hdPacksEnabled != _hdPacksEnabled) {
				//Try to load/unload HD pack when the flag is toggled
				_console->UpdateHdPackMode();
				_hdPacksEnabled = hdPacksEnabled;
			}
		}

		_console->RunSingleFrame();

		if(updated) {
			//Update geometry after running the frame, in case the console's region changed (affects "auto" aspect ratio)
			retro_system_av_info avInfo = {};
			_renderer->GetSystemAudioVideoInfo(avInfo);
			retroEnv(RETRO_ENVIRONMENT_SET_GEOMETRY, &avInfo);
		}
	}

	RETRO_API size_t retro_serialize_size()
	{
		return _saveStateSize;
	}

	RETRO_API bool retro_serialize(void *data, size_t size)
	{
		std::stringstream ss;
		_console->GetSaveStateManager()->SaveState(ss);
		
		string saveStateData = ss.str();
		memset(data, 0, size);
		memcpy(data, saveStateData.c_str(), std::min(size, saveStateData.size()));

		return true;
	}

	RETRO_API bool retro_unserialize(const void *data, size_t size)
	{
		std::stringstream ss;
		ss.write((char*)data, size);

		bool result = _console->GetSaveStateManager()->LoadState(ss, false);
		if(result)
			_console->GetSettings()->SetSampleRate(_audioSampleRate);
		return result;
	}

	RETRO_API void retro_cheat_reset()
	{
		_console->GetCheatManager()->ClearCodes();
	}

	RETRO_API void retro_cheat_set(unsigned index, bool enabled, const char *codeStr)
	{
		static const string validGgLetters = "APZLGITYEOXUKSVN";
		static const string validParLetters = "0123456789ABCDEF";

		string code = codeStr;
		std::transform(code.begin(), code.end(), code.begin(), ::toupper);
		if(code.size() == 7 && code[4] == ':') {
			string address = code.substr(0, 4);
			string value = code.substr(5, 2);
			_console->GetCheatManager()->AddCustomCode(HexUtilities::FromHex(address), HexUtilities::FromHex(value));
		} else if(code.size() == 10 && code[4] == '?' && code[7] == ':') {
			string address = code.substr(0, 4);
			string comparison = code.substr(5, 2);
			string value = code.substr(8, 2);
			_console->GetCheatManager()->AddCustomCode(HexUtilities::FromHex(address), HexUtilities::FromHex(value), HexUtilities::FromHex(comparison));
		} else if(code.size() == 6 || code.size() == 8) {
			//This is either a GG or PAR code
			bool isValidGgCode = true;
			bool isValidParCode = code.size() == 8;
			for(size_t i = 0; i < code.size(); i++) {
				if(validGgLetters.find(code[i]) == string::npos) {
					isValidGgCode = false;
				}
				if(validParLetters.find(code[i]) == string::npos) {
					isValidParCode = false;
				}
			}

			if(isValidGgCode) {
				_console->GetCheatManager()->AddGameGenieCode(code);
			} else if(isValidParCode) {
				_console->GetCheatManager()->AddProActionRockyCode(HexUtilities::FromHex(code));
			}
		}

	}

	void update_input_descriptors()
	{
		vector<retro_input_descriptor> desc;

		auto addDesc = [&desc](unsigned port, unsigned button, const char* name) {
			retro_input_descriptor d = { port, RETRO_DEVICE_JOYPAD, 0, button, name };
			desc.push_back(d);
		};

		auto setupPlayerButtons = [addDesc](int port) {
			unsigned device = _inputDevices[port];
			if(device == DEVICE_AUTO) {
				if(port <= 3) {
					switch(_console->GetSettings()->GetControllerType(port)) {
						case ControllerType::StandardController: device = DEVICE_GAMEPAD; break;
						case ControllerType::PowerPad: device = DEVICE_POWERPAD; break;
						case ControllerType::SnesController: device = DEVICE_SNESGAMEPAD; break;
						case ControllerType::SnesMouse: device = DEVICE_SNESMOUSE; break;
						case ControllerType::Zapper: device = DEVICE_ZAPPER; break;
						case ControllerType::ArkanoidController: device = DEVICE_ARKANOID; break;
						default: return;
					}
				} else if(port == 4) {
					switch(_console->GetSettings()->GetExpansionDevice()) {
						case ExpansionPortDevice::ArkanoidController: device = DEVICE_ARKANOID; break;
						case ExpansionPortDevice::BandaiHyperShot: device = DEVICE_BANDAIHYPERSHOT; break;
						case ExpansionPortDevice::ExcitingBoxing: device = DEVICE_EXCITINGBOXING; break;
						case ExpansionPortDevice::FamilyTrainerMat: device = DEVICE_FAMILYTRAINER; break;
						case ExpansionPortDevice::HoriTrack: device = DEVICE_HORITRACK; break;
						case ExpansionPortDevice::KonamiHyperShot: device = DEVICE_KONAMIHYPERSHOT; break;
						case ExpansionPortDevice::OekaKidsTablet: device = DEVICE_OEKAKIDS; break;
						case ExpansionPortDevice::Pachinko: device = DEVICE_PACHINKO; break;
						case ExpansionPortDevice::PartyTap: device = DEVICE_PARTYTAP; break;
						case ExpansionPortDevice::Zapper: device = DEVICE_ZAPPER; break;
						case ExpansionPortDevice::BattleBox: device = DEVICE_BATTLEBOX; break;
						case ExpansionPortDevice::AsciiTurboFile: device = DEVICE_ASCIITURBOFILE; break;
						case ExpansionPortDevice::FourPlayerAdapter: device = DEVICE_FOURPLAYERADAPTER; break;
						default: return;
					}
				}
			}

			if(device == DEVICE_GAMEPAD || device == DEVICE_SNESGAMEPAD) {
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_LEFT, "D-Pad Left");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_UP, "D-Pad Up");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_DOWN, "D-Pad Down");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_RIGHT, "D-Pad Right");
				if(device == DEVICE_SNESGAMEPAD) {
					addDesc(port, RETRO_DEVICE_ID_JOYPAD_A, "A");
					addDesc(port, RETRO_DEVICE_ID_JOYPAD_B, "B");
					addDesc(port, RETRO_DEVICE_ID_JOYPAD_X, "X");
					addDesc(port, RETRO_DEVICE_ID_JOYPAD_Y, "Y");
					addDesc(port, RETRO_DEVICE_ID_JOYPAD_L, "L");
					addDesc(port, RETRO_DEVICE_ID_JOYPAD_R, "R");
				} else {
					if(_shiftButtonsClockwise) {
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_B, "A");
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_Y, "B");
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_A, "Turbo A");
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_X, "Turbo B");
					} else {
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_A, "A");
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_B, "B");
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_X, "Turbo A");
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_Y, "Turbo B");
					}

					if(port == 0) {
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_L, "(FDS) Insert Next Disk");
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_R, "(FDS) Switch Disk Side");
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_L2, "(VS) Insert Coin 1");
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_R2, "(VS) Insert Coin 2");
						addDesc(port, RETRO_DEVICE_ID_JOYPAD_L3, "(Famicom) Microphone (P2)");
					}
				}
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_START, "Start");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_SELECT, "Select");
			} else if(device == DEVICE_EXCITINGBOXING) {
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_B, "Left Hook");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_A, "Right Hook");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_Y, "Left Jab");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_X, "Right Jab");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_L, "Body");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_R, "Straight");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_LEFT, "Move Left");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_RIGHT, "Move Right");
			} else if(device == DEVICE_PARTYTAP) {
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_B, "Partytap P1");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_A, "Partytap P2");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_Y, "Partytap P3");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_X, "Partytap P4");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_L, "Partytap P5");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_R, "Partytap P6");
			} else if(device == DEVICE_FAMILYTRAINER || device == DEVICE_POWERPAD) {
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_B, "Powerpad B1");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_A, "Powerpad B2");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_Y, "Powerpad B3");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_X, "Powerpad B4");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_L, "Powerpad B5");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_R, "Powerpad B6");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_LEFT, "Powerpad B7");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_RIGHT, "Powerpad B8");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_UP, "Powerpad B9");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_DOWN, "Powerpad B10");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_SELECT, "Powerpad B11");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_START, "Powerpad B12");
			} else if(device == DEVICE_PACHINKO) {
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_L, "Release Trigger");
				addDesc(port, RETRO_DEVICE_ID_JOYPAD_R, "Press Trigger");
			}
		};

		setupPlayerButtons(0);
		setupPlayerButtons(1);
		setupPlayerButtons(2);
		setupPlayerButtons(3);
		setupPlayerButtons(4);

		retro_input_descriptor end = { 0 };
		desc.push_back(end);

		retroEnv(RETRO_ENVIRONMENT_SET_INPUT_DESCRIPTORS, desc.data());
	}

	void update_core_controllers()
	{
		//Setup all "auto" ports
		RomInfo romInfo = _console->GetRomInfo();
		if(romInfo.IsInDatabase || romInfo.IsNes20Header) {
			_console->GetSettings()->InitializeInputDevices(romInfo.InputType, romInfo.System, true);
		} else {
			_console->GetSettings()->InitializeInputDevices(GameInputType::StandardControllers, GameSystem::NesNtsc, true);
		}

		for(int port = 0; port < 5; port++) {
			if(_inputDevices[port] != DEVICE_AUTO) {
				if(port <= 3) {
					ControllerType type = ControllerType::StandardController;
					switch(_inputDevices[port]) {
						case RETRO_DEVICE_NONE: type = ControllerType::None; break;
						case DEVICE_GAMEPAD: type = ControllerType::StandardController; break;
						case DEVICE_ZAPPER: type = ControllerType::Zapper; break;
						case DEVICE_POWERPAD: type = ControllerType::PowerPad; break;
						case DEVICE_ARKANOID: type = ControllerType::ArkanoidController; break;
						case DEVICE_SNESGAMEPAD: type = ControllerType::SnesController; break;
						case DEVICE_SNESMOUSE: type = ControllerType::SnesMouse; break;
					}
					_console->GetSettings()->SetControllerType(port, type);
				} else {
					ExpansionPortDevice type = ExpansionPortDevice::None;
					switch(_inputDevices[port]) {
						case RETRO_DEVICE_NONE: type = ExpansionPortDevice::None; break;
						case DEVICE_FAMILYTRAINER: type = ExpansionPortDevice::FamilyTrainerMat; break;
						case DEVICE_PARTYTAP: type = ExpansionPortDevice::PartyTap; break;
						case DEVICE_PACHINKO: type = ExpansionPortDevice::Pachinko; break;
						case DEVICE_EXCITINGBOXING: type = ExpansionPortDevice::ExcitingBoxing; break;
						case DEVICE_KONAMIHYPERSHOT: type = ExpansionPortDevice::KonamiHyperShot; break;
						case DEVICE_OEKAKIDS: type = ExpansionPortDevice::OekaKidsTablet; break;
						case DEVICE_BANDAIHYPERSHOT: type = ExpansionPortDevice::BandaiHyperShot; break;
						case DEVICE_ARKANOID: type = ExpansionPortDevice::ArkanoidController; break;
						case DEVICE_HORITRACK: type = ExpansionPortDevice::HoriTrack; break;
						case DEVICE_ASCIITURBOFILE: type = ExpansionPortDevice::AsciiTurboFile; break;
						case DEVICE_BATTLEBOX: type = ExpansionPortDevice::BattleBox; break;
						case DEVICE_FOURPLAYERADAPTER: type = ExpansionPortDevice::FourPlayerAdapter; break;
					}
					_console->GetSettings()->SetExpansionDevice(type);
				}
			}
		}

		bool hasFourScore = false;
		bool isFamicom = (_console->GetSettings()->GetExpansionDevice() != ExpansionPortDevice::None || romInfo.System == GameSystem::Famicom || romInfo.System == GameSystem::FDS || romInfo.System == GameSystem::Dendy);
		if(isFamicom) {
			_console->GetSettings()->SetConsoleType(ConsoleType::Famicom);
			if(_console->GetSettings()->GetExpansionDevice() == ExpansionPortDevice::FourPlayerAdapter) {
				hasFourScore = true;
			}
		} else {
			_console->GetSettings()->SetConsoleType(ConsoleType::Nes);
			if(_console->GetSettings()->GetControllerType(2) != ControllerType::None || _console->GetSettings()->GetControllerType(3) != ControllerType::None) {
				hasFourScore = true;
			}
		}

		_console->GetSettings()->SetFlagState(EmulationFlags::HasFourScore, hasFourScore);
	}
	
	void retro_set_memory_maps()
	{
		//Expose internal RAM and work/save RAM for retroachievements
		memset(_descriptors, 0, sizeof(_descriptors));
		memset(&_memoryMap, 0, sizeof(_memoryMap));

		uint32_t i = 0;
		uint32_t size = 0;
		int32_t startAddr = 0;
		uint8_t* internalRam = _console->GetRamBuffer(DebugMemoryType::InternalRam, size, startAddr);
		_descriptors[i].ptr = internalRam;
		_descriptors[i].start = startAddr;
		_descriptors[i].len = size;
		_descriptors[i].select = 0;
		i++;

		uint8_t* saveRam = _console->GetRamBuffer(DebugMemoryType::SaveRam, size, startAddr);
		if(size > 0 && startAddr > 0) {
			_descriptors[i].ptr = saveRam;
			_descriptors[i].start = startAddr;
			_descriptors[i].len = size;
			_descriptors[i].select = 0;
			i++;
		}

		uint8_t* workRam = _console->GetRamBuffer(DebugMemoryType::WorkRam, size, startAddr);
		if(size > 0 && startAddr > 0) {
			_descriptors[i].ptr = workRam;
			_descriptors[i].start = startAddr;
			_descriptors[i].len = size;
			_descriptors[i].select = 0;
			i++;
		}

		_memoryMap.descriptors = _descriptors;
		_memoryMap.num_descriptors = i;
		retroEnv(RETRO_ENVIRONMENT_SET_MEMORY_MAPS, &_memoryMap);
	}

	RETRO_API void retro_set_controller_port_device(unsigned port, unsigned device)
	{
		if(port < 5 && _inputDevices[port] != device) {
			_inputDevices[port] = device;
			update_core_controllers();
			update_input_descriptors();
		}
	}

	RETRO_API bool retro_load_game(const struct retro_game_info *game)
	{
		char *systemFolder;
		if(!retroEnv(RETRO_ENVIRONMENT_GET_SYSTEM_DIRECTORY, &systemFolder) || !systemFolder) {
			return false;
		}

		char *saveFolder;
		if(!retroEnv(RETRO_ENVIRONMENT_GET_SAVE_DIRECTORY, &saveFolder)) {
			logMessage(RETRO_LOG_ERROR, "Could not find save directory.\n");
		}

		enum retro_pixel_format fmt = RETRO_PIXEL_FORMAT_XRGB8888;
		if(!retroEnv(RETRO_ENVIRONMENT_SET_PIXEL_FORMAT, &fmt)) {
			logMessage(RETRO_LOG_ERROR, "XRGB8888 is not supported.\n");
			return false;
		}

		//Expect the following structure:
		// /system/disksys.rom
		// /system/HdPacks/*
		// /saves/*.sav
		FolderUtilities::SetHomeFolder(systemFolder);
		FolderUtilities::SetFolderOverrides(saveFolder, "", "");

		update_settings();

		//Plug in 2 standard controllers by default, game database will switch the controller types for recognized games
		_console->GetSettings()->SetMasterVolume(10.0);
		_console->GetSettings()->SetControllerType(0, ControllerType::StandardController);
		_console->GetSettings()->SetControllerType(1, ControllerType::StandardController);
		_console->GetSettings()->SetControllerType(2, ControllerType::None);
		_console->GetSettings()->SetControllerType(3, ControllerType::None);

		VirtualFile romData(game->data, game->size, game->path);
		bool result = _console->Initialize(romData);

		if(result) {
			//Set default dipswitches for some VS System games
			switch(_console->GetRomInfo().Hash.PrgCrc32) {
				case 0x8850924B: _console->GetSettings()->SetDipSwitches(32); break; //VS Tetris
				case 0xE1AA8214: _console->GetSettings()->SetDipSwitches(32); break; //StarLuster
				default: _console->GetSettings()->SetDipSwitches(0); break;
			}

			update_core_controllers();
			update_input_descriptors();

			//Savestates in Mesen may change size over time
			//Retroarch doesn't like this for netplay or rewinding - it requires the states to always be the exact same size
			//So we need to send a large enough size to Retroarch to ensure Mesen's state will always fit within that buffer.
			std::stringstream ss;
			_console->GetSaveStateManager()->SaveState(ss);

			//Round up to the next 1kb multiple
			_saveStateSize = ((ss.str().size() * 2) + 0x400) & ~0x3FF;
			retro_set_memory_maps();
		}

		return result;
	}

	RETRO_API bool retro_load_game_special(unsigned game_type, const struct retro_game_info *info, size_t num_info)
	{
		return false;
	}

	RETRO_API void retro_unload_game()
	{
		_console->Stop();
	}

	RETRO_API unsigned retro_get_region()
	{
		NesModel model = _console->GetModel();
		return model == NesModel::NTSC ? RETRO_REGION_NTSC : RETRO_REGION_PAL;
	}

	RETRO_API void retro_get_system_info(struct retro_system_info *info)
	{
		_mesenVersion = EmulationSettings::GetMesenVersionString();

		info->library_name = "Mesen";
		info->library_version = _mesenVersion.c_str();
		info->need_fullpath = false;
		info->valid_extensions = "nes|fds|unf|unif";
		info->block_extract = false;
	}

	RETRO_API void retro_get_system_av_info(struct retro_system_av_info *info)
	{
		uint32_t hscale = 1;
		uint32_t vscale = 1;
		switch(_console->GetSettings()->GetVideoFilterType()) {
			case VideoFilterType::NTSC: hscale = 2; break;
			case VideoFilterType::BisqwitNtscQuarterRes: hscale = 2; break;
			case VideoFilterType::BisqwitNtscHalfRes: hscale = 4; break;
			case VideoFilterType::BisqwitNtsc: hscale = 8; break;
			default: hscale = 1; break;
		}
		
		shared_ptr<HdPackData> hdData = _console->GetHdData();
		if(hdData) {
			hscale = hdData->Scale;
			vscale = hdData->Scale;
		}

		if(hscale <= 2) {
			_renderer->GetSystemAudioVideoInfo(*info, NES_NTSC_OUT_WIDTH(256), 240 * vscale);
		} else {
			_renderer->GetSystemAudioVideoInfo(*info, 256 * hscale, 240 * vscale);
		}
	}

	RETRO_API void *retro_get_memory_data(unsigned id)
	{
		uint32_t size;
		int32_t startAddr;
		switch(id) {
			case RETRO_MEMORY_SAVE_RAM: return _console->GetRamBuffer(DebugMemoryType::SaveRam, size, startAddr);
			case RETRO_MEMORY_SYSTEM_RAM: return _console->GetRamBuffer(DebugMemoryType::InternalRam, size, startAddr);
		}
		return nullptr;
	}

	RETRO_API size_t retro_get_memory_size(unsigned id)
	{
		uint32_t size = 0;
		int32_t startAddr;
		switch(id) {
			case RETRO_MEMORY_SAVE_RAM: _console->GetRamBuffer(DebugMemoryType::SaveRam, size, startAddr); break;
			case RETRO_MEMORY_SYSTEM_RAM: _console->GetRamBuffer(DebugMemoryType::InternalRam, size, startAddr); break;
		}
		return size;
	}
}
