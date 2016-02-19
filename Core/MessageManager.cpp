#include "stdafx.h"

#include <algorithm>
#include "MessageManager.h"
#include "EmulationSettings.h"

std::unordered_map<string, string> MessageManager::_enResources = {
	{ "Cheats", u8"Cheats" },
	{ "Debug", u8"Debug" },
	{ "EmulationSpeed", u8"Emulation Speed" },
	{ "Error", u8"Error" },
	{ "GameInfo", u8"Game Info" },
	{ "GameLoaded", u8"Game loaded" },
	{ "Movies", u8"Movies" },
	{ "NetPlay", u8"Net Play" },
	{ "SaveStates", u8"Save States" },
	{ "ScreenshotSaved", u8"Screenshot Saved" },
	{ "Test", u8"Test" },
	
	{ "CheatApplied", u8"1 cheat applied." },
	{ "CheatsApplied", u8"%1 cheats applied." },
	{ "ConnectedToServer", u8"Connected to server." },
	{ "ConnectedAsPlayer", u8"Connected as player %1" },
	{ "ConnectedAsSpectator", u8"Connected as spectator." },
	{ "ConnectionLost", u8"Connection to server lost." },
	{ "CouldNotConnect", u8"Could not connect to the server." },
	{ "CouldNotIniitalizeAudioSystem", u8"Could not initialize audio system" },	
	{ "CouldNotFindRom", u8"Could not find matching game ROM." },
	{ "CouldNotLoadFile", u8"Could not load file: %1" },
	{ "EmulationMaximumSpeed", u8"Maximum speed" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "GameCrash", u8"Game has crashed (%1)" },
	{ "Mapper", u8"Mapper: %1" },
	{ "MovieEnded", u8"Movie ended." },
	{ "MovieInvalid", u8"Invalid movie file." },
	{ "MovieMissingRom", u8"Missing ROM required (%1) to play movie." },
	{ "MovieNewerVersion", u8"Cannot load movies created by a more recent version of Mesen. Please download the latest version." },
	{ "MovieIncompatibleVersion", u8"This movie is incompatible with this version of Mesen." },
	{ "MoviePlaying", u8"Playing movie: %1" },
	{ "MovieRecordingTo", u8"Recording to: %1" },
	{ "MovieSaved", u8"Movie saved to file: %1" },
	{ "NetplayVersionMismatch", u8"%1 is not running the same version of Mesen and has been disconnected." },
	{ "PrgSizeWarning", u8"PRG size is smaller than 32kb" },
	{ "SaveStateEmpty", u8"Slot is empty." },
	{ "SaveStateIncompatibleVersion", u8"State #%1 is incompatible with this version of Mesen." },
	{ "SaveStateInvalidFile", u8"Invalid save state file." },
	{ "SaveStateLoaded", u8"State #%1 loaded." },
	{ "SaveStateNewerVersion", u8"Cannot load save states created by a more recent version of Mesen. Please download the latest version." },
	{ "SaveStateSaved", u8"State #%1 saved." },
	{ "ServerStarted", u8"Server started (Port: %1)" },
	{ "ServerStopped", u8"Server stopped" },
	{ "TestFileSavedTo", u8"Test file saved to: %1" },
	{ "UnsupportedMapper", u8"Unsupported mapper, cannot load game." },
};

std::unordered_map<string, string> MessageManager::_frResources = {
	{ "Cheats", u8"Codes" },
	{ "Debug", u8"Debug" },
	{ "EmulationSpeed", u8"Vitesse" },
	{ "Error", u8"Erreur" },
	{ "GameInfo", u8"Info sur le ROM" },
	{ "GameLoaded", u8"Jeu chargé" },
	{ "Movies", u8"Films" },
	{ "NetPlay", u8"Jeu en ligne" },
	{ "SaveStates", u8"Sauvegardes" },
	{ "ScreenshotSaved", u8"Capture d'écran" },
	{ "Test", u8"Test" },

	{ "CheatApplied", u8"%1 code activé." },
	{ "CheatsApplied", u8"%1 codes activés." },
	{ "ConnectedToServer", u8"Connecté avec succès au serveur." },
	{ "ConnectedAsPlayer", u8"Connecté en tant que joueur #%1" },
	{ "ConnectedAsSpectator", u8"Connecté en tant que spectateur." },
	{ "ConnectionLost", u8"La connexion avec le serveur a été perdue." },
	{ "CouldNotConnect", u8"Impossible de se connecter au serveur." },
	{ "CouldNotIniitalizeAudioSystem", u8"L'initialisation du système de son à échoué" },
	{ "CouldNotFindRom", u8"Impossible de trouvé un rom correspondant." },
	{ "CouldNotLoadFile", u8"Impossible de charger le fichier : %1" },
	{ "EmulationMaximumSpeed", u8"Vitesse maximale" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "GameCrash", u8"Le jeu a planté (%1)" },
	{ "Mapper", u8"Mapper : %1" },
	{ "MovieEnded", u8"Fin du film." },
	{ "MovieInvalid", u8"Fichier de film invalide." },
	{ "MovieMissingRom", u8"Le rom (%1) correspondant au film sélectionné est introuvable." },
	{ "MovieNewerVersion", u8"Impossible de charger un film qui a été créé avec une version plus récente de Mesen. Veuillez mettre à jour Mesen pour jouer ce film." },
	{ "MovieIncompatibleVersion", u8"Ce film est incompatible avec votre version de Mesen." },
	{ "MoviePlaying", u8"Film démarré: %1" },
	{ "MovieRecordingTo", u8"En cours d'enregistrement : %1" },
	{ "MovieSaved", u8"Film sauvegardé : %1" },
	{ "NetplayVersionMismatch", u8"%1 ne roule pas la même version de Mesen que vous et a été déconnecté automatiquement." },
	{ "PrgSizeWarning", u8"PRG size is smaller than 32kb" },
	{ "SaveStateEmpty", u8"Cette sauvegarde est vide." },
	{ "SaveStateIncompatibleVersion", u8"La sauvegarde #%1 est incompatible avec cette version de Mesen." },
	{ "SaveStateInvalidFile", u8"Fichier de sauvegarde invalide ou corrompu." },
	{ "SaveStateLoaded", u8"Sauvegarde #%1 chargée." },
	{ "SaveStateNewerVersion", u8"Impossible de charger une sauvegarde qui a été créée avec une version plus récente de Mesen. Veuillez mettre à jour Mesen." },
	{ "SaveStateSaved", u8"Sauvegarde #%1 sauvegardée." },
	{ "ServerStarted", u8"Le serveur a été démarré (Port: %1)" },
	{ "ServerStopped", u8"Le serveur a été arrêté" },
	{ "TestFileSavedTo", u8"Test sauvegardé: %1" },
	{ "UnsupportedMapper", u8"Ce mapper n'est pas encore supporté - le jeu ne peut pas être démarré." },
};

std::unordered_map<string, string> MessageManager::_jaResources = {
	{ "Cheats", u8"チート" },
	{ "Debug", u8"Debug" },
	{ "EmulationSpeed", u8"速度" },
	{ "Error", u8"エラー" },
	{ "GameInfo", u8"ゲーム情報" },
	{ "GameLoaded", u8"ゲーム開始" },
	{ "Movies", u8"動画" },
	{ "NetPlay", u8"ネットプレー" },
	{ "SaveStates", u8"クイックセーブ" },
	{ "ScreenshotSaved", u8"スクリーンショット" },
	{ "Test", u8"テスト" },

	{ "CheatApplied", u8"チートコード%1個を有効にしました。" },
	{ "CheatsApplied", u8"チートコード%1個を有効にしました。" },
	{ "ConnectedToServer", u8"サーバに接続しました。" },
	{ "ConnectedAsPlayer", u8"プレーヤー %1として接続しました。" },
	{ "ConnectedAsSpectator", u8"観客として接続しました。" },
	{ "ConnectionLost", u8"セーバから切断されました。" },
	{ "CouldNotConnect", u8"セーバに接続出来ませんでした。" },
	{ "CouldNotIniitalizeAudioSystem", u8"オーディオデバイスを初期化出来ませんでした。" },
	{ "CouldNotFindRom", u8"該当するゲームファイルは見つけられませんでした。" },
	{ "CouldNotLoadFile", u8"ファイルをロードできませんでした: %1" },
	{ "EmulationMaximumSpeed", u8"最高速度" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "GameCrash", u8"ゲームはクラッシュしました (%1)" },
	{ "Mapper", u8"Mapper: %1" },
	{ "MovieEnded", u8"動画が終わりました。" },
	{ "MovieInvalid", u8"動画のデータは読めませんでした。" },
	{ "MovieMissingRom", u8"動画に必要なゲームファイルは見つけられませんでした。(%1)" },
	{ "MovieNewerVersion", u8"この動画はもっと新しいMesenのバージョンで作られたため、読めませんでした。　Mesenのサイトで最新のバージョンをダウンロードしてください。" },
	{ "MovieIncompatibleVersion", u8"この動画は古いMesenのバージョンで作られたもので、ロードできませんでした。" },
	{ "MoviePlaying", u8"動画再生: %1" },
	{ "MovieRecordingTo", u8"%1に録画しています。" },
	{ "MovieSaved", u8"録画は終わりました: %1" },
	{ "NetplayVersionMismatch", u8"%1さんはMesenの別のバージョンを使っているため、接続はできませんでした。" },
	{ "PrgSizeWarning", u8"PRG size is smaller than 32kb" },
	{ "SaveStateEmpty", u8"セーブデータがありませんでした。" },
	{ "SaveStateIncompatibleVersion", u8"クイックセーブ%1は古いMesenのバージョンで作られたもので、ロードできませんでした。" },
	{ "SaveStateInvalidFile", u8"クイックセーブデータは読めませんでした。" },
	{ "SaveStateLoaded", u8"クイックセーブ%1をロードしました。" },
	{ "SaveStateNewerVersion", u8"クイックセーブデータはもっと新しいMesenのバージョンで作られたため、読めませんでした。　Mesenのサイトで最新のバージョンをダウンロードしてください。" },
	{ "SaveStateSaved", u8"クイックセーブ%1をセーブしました。" },
	{ "ServerStarted", u8"サーバは起動しました (ポート: %1)" },
	{ "ServerStopped", u8"セーバは停止しました。" },
	{ "TestFileSavedTo", u8"Test file saved to: %1" },
	{ "UnsupportedMapper", u8"このMapperを使うゲームはロードできません。" },
};

IMessageManager* MessageManager::_messageManager = nullptr;
vector<INotificationListener*> MessageManager::_notificationListeners;

void MessageManager::RegisterMessageManager(IMessageManager* messageManager)
{
	MessageManager::_messageManager = messageManager;
}

string MessageManager::Localize(string key)
{
	std::unordered_map<string, string> *resources = nullptr;
	switch(EmulationSettings::GetDisplayLanguage()) {
		case Language::English: resources = &_enResources; break;
		case Language::French: resources = &_frResources; break;
		case Language::Japanese: resources = &_jaResources; break;
	}
	if(resources) {
		if(resources->find(key) != resources->end()) {
			return (*resources)[key];
		}
	}

	return "";
}

void MessageManager::DisplayMessage(string title, string message, string param1)
{
	if(MessageManager::_messageManager) {
		std::unordered_map<string, string> *resources = nullptr;
		switch(EmulationSettings::GetDisplayLanguage()) {
			case Language::English: resources = &_enResources; break;
			case Language::French: resources = &_frResources; break;
			case Language::Japanese: resources = &_jaResources; break;
		}
		if(resources) {
			if(resources->find(title) != resources->end()) {
				title = (*resources)[title];
			}
			if(resources->find(message) != resources->end()) {
				message = (*resources)[message];

				size_t startPos = message.find(u8"%1");
				if(startPos != std::string::npos) {
					message.replace(startPos, 2, param1);
				}
			}
		}
		MessageManager::_messageManager->DisplayMessage(title, message);
	}
}

void MessageManager::DisplayToast(string title, string message, uint8_t* iconData, uint32_t iconSize)
{
	if(MessageManager::_messageManager) {
		MessageManager::_messageManager->DisplayToast(shared_ptr<ToastInfo>(new ToastInfo(title, message, 4000, iconData, iconSize)));
	}
}
void MessageManager::RegisterNotificationListener(INotificationListener* notificationListener)
{
	MessageManager::_notificationListeners.push_back(notificationListener);
}

void MessageManager::UnregisterNotificationListener(INotificationListener* notificationListener)
{
	MessageManager::_notificationListeners.erase(std::remove(MessageManager::_notificationListeners.begin(), MessageManager::_notificationListeners.end(), notificationListener), MessageManager::_notificationListeners.end());
}

void MessageManager::SendNotification(ConsoleNotificationType type, void* parameter)
{
	//Iterate on a copy to prevent issues if a notification causes a listener to unregister itself
	vector<INotificationListener*> listeners = MessageManager::_notificationListeners;
	vector<INotificationListener*> processedListeners;

	for(size_t i = 0, len = listeners.size(); i < len; i++) {
		INotificationListener* notificationListener = listeners[i];
		if(std::find(processedListeners.begin(), processedListeners.end(), notificationListener) == processedListeners.end()) {
			//Only send notification if it hasn't been processed already
			notificationListener->ProcessNotification(type, parameter);
		}
		processedListeners.push_back(notificationListener);

		if(len != MessageManager::_notificationListeners.size()) {
			//Vector changed, start from the beginning again (can occur when sending a notification caused listeners to unregister themselves)
			i = 0;
			len = MessageManager::_notificationListeners.size();
			listeners = MessageManager::_notificationListeners;
		}
	}
}