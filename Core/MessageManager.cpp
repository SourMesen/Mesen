#include "stdafx.h"

#include <algorithm>
#include "MessageManager.h"
#include "EmulationSettings.h"

std::unordered_map<string, string> MessageManager::_enResources = {
	{ "Cheats", u8"Cheats" },
	{ "Debug", u8"Debug" },
	{ "EmulationSpeed", u8"Emulation Speed" },
	{ "ClockRate", u8"Clock Rate" },
	{ "Error", u8"Error" },
	{ "GameInfo", u8"Game Info" },
	{ "GameLoaded", u8"Game loaded" },
	{ "IPS", u8"IPS" },
	{ "Movies", u8"Movies" },
	{ "NetPlay", u8"Net Play" },
	{ "Region", u8"Region" },
	{ "SaveStates", u8"Save States" },
	{ "ScreenshotSaved", u8"Screenshot Saved" },
	{ "SoundRecorder", u8"Sound Recorder" },
	{ "Test", u8"Test" },

	{ "ApplyingIps", u8"Applying patch: %1" },
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
	{ "Lag", u8"Lag" },
	{ "Mapper", u8"Mapper: %1, SubMapper: %2" },
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
	{ "SaveStateSlotSelected", u8"Slot #%1 selected." },
	{ "ScanlineTimingWarning", u8"PPU timing has been changed." },
	{ "ServerStarted", u8"Server started (Port: %1)" },
	{ "ServerStopped", u8"Server stopped" },
	{ "SoundRecorderStarted", u8"Recording to: %1" },
	{ "SoundRecorderStopped", u8"Recording saved to: %1" },
	{ "TestFileSavedTo", u8"Test file saved to: %1" },
	{ "UnsupportedMapper", u8"Unsupported mapper (%1), cannot load game." },

	{ "GoogleDrive", u8"Google Drive" },
	{ "SynchronizationStarted", u8"Synchronization started." },
	{ "SynchronizationFailed", u8"Synchronization failed." },
	{ "SynchronizationCompleted", u8"Synchronization completed." },
};

std::unordered_map<string, string> MessageManager::_frResources = {
	{ "Cheats", u8"Codes" },
	{ "Debug", u8"Debug" },
	{ "EmulationSpeed", u8"Vitesse" },
	{ "ClockRate", u8"Fréquence d'horloge" },
	{ "Error", u8"Erreur" },
	{ "GameInfo", u8"Info sur le ROM" },
	{ "GameLoaded", u8"Jeu chargé" },
	{ "IPS", u8"IPS" },
	{ "Movies", u8"Films" },
	{ "NetPlay", u8"Jeu en ligne" },
	{ "Region", u8"Région" },
	{ "SaveStates", u8"Sauvegardes" },
	{ "ScreenshotSaved", u8"Capture d'écran" },
	{ "SoundRecorder", u8"Enregistreur audio" },
	{ "Test", u8"Test" },

	{ "ApplyingIps", u8"Fichier IPS appliqué : %1" },
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
	{ "Lag", u8"Lag" },
	{ "Mapper", u8"Mapper : %1, SubMapper : %2" },
	{ "MovieEnded", u8"Fin du film." },
	{ "MovieInvalid", u8"Fichier de film invalide." },
	{ "MovieMissingRom", u8"Le rom (%1) correspondant au film sélectionné est introuvable." },
	{ "MovieNewerVersion", u8"Impossible de charger un film qui a été créé avec une version plus récente de Mesen. Veuillez mettre à jour Mesen pour jouer ce film." },
	{ "MovieIncompatibleVersion", u8"Ce film est incompatible avec votre version de Mesen." },
	{ "MoviePlaying", u8"Film démarré : %1" },
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
	{ "SaveStateSlotSelected", u8"Position de sauvegarde #%1 choisie." },
	{ "ScanlineTimingWarning", u8"Le timing du PPU a été modifié." },
	{ "ServerStarted", u8"Le serveur a été démarré (Port : %1)" },
	{ "ServerStopped", u8"Le serveur a été arrêté" },
	{ "SoundRecorderStarted", u8"En cours d'enregistrement : %1" },
	{ "SoundRecorderStopped", u8"Enregistrement audio sauvegardé : %1" },
	{ "TestFileSavedTo", u8"Test sauvegardé : %1" },
	{ "UnsupportedMapper", u8"Ce mapper (%1) n'est pas encore supporté - le jeu ne peut pas être démarré." },

	{ "GoogleDrive", u8"Google Drive" },
	{ "SynchronizationStarted", u8"Synchronisation en cours." },
	{ "SynchronizationFailed", u8"La synchronisation a échoué." },
	{ "SynchronizationCompleted", u8"Synchronisation terminée." },
};

std::unordered_map<string, string> MessageManager::_jaResources = {
	{ "Cheats", u8"チート" },
	{ "Debug", u8"Debug" },
	{ "EmulationSpeed", u8"速度" },
	{ "ClockRate", u8"クロックレート" },
	{ "Error", u8"エラー" },
	{ "GameInfo", u8"ゲーム情報" },
	{ "GameLoaded", u8"ゲーム開始" },
	{ "IPS", u8"IPS" },
	{ "Movies", u8"動画" },
	{ "NetPlay", u8"ネットプレー" },
	{ "Region", u8"地域" },
	{ "SaveStates", u8"クイックセーブ" },
	{ "ScreenshotSaved", u8"スクリーンショット" },
	{ "SoundRecorder", u8"サウンドレコーダー" },
	{ "Test", u8"テスト" },

	{ "ApplyingIps", u8"パッチファイルを適用しました:　%1" },
	{ "CheatApplied", u8"チートコード%1個を有効にしました。" },
	{ "CheatsApplied", u8"チートコード%1個を有効にしました。" },
	{ "ConnectedToServer", u8"サーバに接続しました。" },
	{ "ConnectedAsPlayer", u8"プレーヤー %1として接続しました。" },
	{ "ConnectedAsSpectator", u8"観客として接続しました。" },
	{ "ConnectionLost", u8"サーバから切断されました。" },
	{ "CouldNotConnect", u8"サーバに接続出来ませんでした。" },
	{ "CouldNotIniitalizeAudioSystem", u8"オーディオデバイスを初期化出来ませんでした。" },
	{ "CouldNotFindRom", u8"該当するゲームファイルは見つかりませんでした。" },
	{ "CouldNotLoadFile", u8"ファイルをロードできませんでした: %1" },
	{ "EmulationMaximumSpeed", u8"最高速度" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "GameCrash", u8"ゲームは停止しました (%1)" },
	{ "Lag", u8"ラグ" },
	{ "Mapper", u8"Mapper: %1, SubMapper: %2" },
	{ "MovieEnded", u8"動画の再生が終了しました。" },
	{ "MovieInvalid", u8"動画データの読み込みに失敗しました。" },
	{ "MovieMissingRom", u8"動画に必要なゲームファイルを見つかりませんでした。(%1)" },
	{ "MovieNewerVersion", u8"この動画は使用中のMesenより新しいバージョンで作られたため、ロードできません。　Mesenのサイトで最新のバージョンをダウンロードしてください。" },
	{ "MovieIncompatibleVersion", u8"この動画は古いMesenのバージョンで作られたもので、ロードできませんでした。" },
	{ "MoviePlaying", u8"動画再生: %1" },
	{ "MovieRecordingTo", u8"%1に録画しています。" },
	{ "MovieSaved", u8"録画を終了しました: %1" },
	{ "NetplayVersionMismatch", u8"%1さんはMesenの別のバージョンを使っているため、接続はできませんでした。" },
	{ "PrgSizeWarning", u8"PRG size is smaller than 32kb" },
	{ "SaveStateEmpty", u8"セーブデータがありませんでした。" },
	{ "SaveStateIncompatibleVersion", u8"クイックセーブ%1は古いMesenのバージョンで作られたもので、ロードできませんでした。" },
	{ "SaveStateInvalidFile", u8"クイックセーブデータを読めませんでした。" },
	{ "SaveStateLoaded", u8"クイックセーブ%1をロードしました。" },
	{ "SaveStateNewerVersion", u8"クイックセーブデータは使用中のMesenより新しいバージョンで作られたため、ロードできません。　Mesenのサイトで最新のバージョンをダウンロードしてください。" },
	{ "SaveStateSaved", u8"クイックセーブ%1をセーブしました。" },
	{ "SaveStateSlotSelected", u8"クイックセーブスロット%1。" },
	{ "ServerStarted", u8"サーバは起動しました (ポート: %1)" },
	{ "ServerStopped", u8"サーバは停止しました。" },
	{ "ScanlineTimingWarning", u8"PPUのタイミングは変更されました。" },
	{ "SoundRecorderStarted", u8"%1に録音しています。" },
	{ "SoundRecorderStopped", u8"録音を終了しました: %1" },
	{ "TestFileSavedTo", u8"Test file saved to: %1" },
	{ "UnsupportedMapper", u8"このMapper (%1)を使うゲームはロードできません。" },

	{ "GoogleDrive", u8"Google Drive" },
	{ "SynchronizationStarted", u8"同期中。" },
	{ "SynchronizationFailed", u8"同期は失敗しました。" },
	{ "SynchronizationCompleted", u8"同期完了。" },
};

std::unordered_map<string, string> MessageManager::_ruResources = {
	{ "Cheats", u8"Читы" },
	{ "Debug", u8"Отладка" },
	{ "EmulationSpeed", u8"Скорость эмуляции" },
	{ "ClockRate", u8"Тактовая частота" },
	{ "Error", u8"Ошибка" },
	{ "GameInfo", u8"Информация об игре" },
	{ "GameLoaded", u8"Игра загружена" },
	{ "IPS", u8"IPS" },
	{ "Movies", u8"Записи" },
	{ "NetPlay", u8"Игра по сети" },
	{ "Region", u8"Регион" },
	{ "SaveStates", u8"Сохранения" },
	{ "ScreenshotSaved", u8"Скриншот сохранён" },
	{ "SoundRecorder", u8"Запись звука" },
	{ "Test", u8"Тест" },

	{ "ApplyingIps", u8"Применён патч: %1" },
	{ "CheatApplied", u8"1 Чит применён." },
	{ "CheatsApplied", u8"Читов применено %1" },
	{ "ConnectedToServer", u8"Подключение к серверу." },
	{ "ConnectedAsPlayer", u8"Подключен как игрок %1" },
	{ "ConnectedAsSpectator", u8"Подключен как наблюдатель." },
	{ "ConnectionLost", u8"Соединение потеряно." },
	{ "CouldNotConnect", u8"Не удалось подключиться к серверу" },
	{ "CouldNotIniitalizeAudioSystem", u8"Не удалось инициализировать звуковую подсистему" },
	{ "CouldNotFindRom", u8"Не удалось найти подходящий ROM." },
	{ "CouldNotLoadFile", u8"Не удалось загрузить файл: %1" },
	{ "EmulationMaximumSpeed", u8"Максимальная скорость" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "GameCrash", u8"Игра была аварийно завершена (%1)" },
	{ "Lag", u8"Лаг" },
	{ "Mapper", u8"Mapper: %1, SubMapper: %2" },
	{ "MovieEnded", u8"Воспроизведение окончено." },
	{ "MovieInvalid", u8"Некорректная запись" },
	{ "MovieMissingRom", u8"Отсутствует ROM (%1) необходимый для воспроизведения" },
	{ "MovieNewerVersion", u8"Запись создана в более новой версии Mesen. Пожалуйста загрузите последнюю версию." },
	{ "MovieIncompatibleVersion", u8"Эта запись не совместима с вашей версией Mesen" },
	{ "MoviePlaying", u8"Воспроизведение записи: %1" },
	{ "MovieRecordingTo", u8"Запись начата: %1" },
	{ "MovieSaved", u8"Запись сохранена: %1" },
	{ "NetplayVersionMismatch", u8"Версия Mesen отличается. %1 был отключен." },
	{ "PrgSizeWarning", u8"Размер PRG меньше 32kb" },
	{ "SaveStateEmpty", u8"Слот пуст." },
	{ "SaveStateIncompatibleVersion", u8"Сохранение #%1 несовместимо с вашей версией Mesen." },
	{ "SaveStateInvalidFile", u8"Некорректное сохранение." },
	{ "SaveStateLoaded", u8"Сохранение #%1 загружено." },
	{ "SaveStateNewerVersion", u8"Сохранение создано в более новой версии Mesen. Пожалуйста загрузите последнюю версию." },
	{ "SaveStateSaved", u8"Сохранено в #%1 слот." },
	{ "ScanlineTimingWarning", u8"Тайминг PPU был изменён." },
	{ "ServerStarted", u8"Сервер запущен (Порт: %1)" },
	{ "SaveStateSlotSelected", u8"Выбран #%1 слот." },
	{ "ServerStopped", u8"Сервер остановлен" },
	{ "SoundRecorderStarted", u8"Запись начата to: %1" },
	{ "SoundRecorderStopped", u8"Запись сохранена: %1" },
	{ "TestFileSavedTo", u8"Тест сохранён: %1" },
	{ "UnsupportedMapper", u8"Неподдерживаемый mapper (%1), игра не загружена." },

	{ "GoogleDrive", u8"Google Drive" },
	{ "SynchronizationStarted", u8"Синхронизация начата." },
	{ "SynchronizationFailed", u8"Синхронизация не удалась." },
	{ "SynchronizationCompleted", u8"Синхронизация завершена." },
};

std::unordered_map<string, string> MessageManager::_esResources = {
	{ "Cheats", u8"Trucos" },
	{ "Debug", u8"Debug" },
	{ "EmulationSpeed", u8"Velocidad" },
	{ "ClockRate", u8"Frecuencia de Reloj" },
	{ "Error", u8"Error" },
	{ "GameInfo", u8"Info del Juego" },
	{ "GameLoaded", u8"Juego Cargado" },
	{ "IPS", u8"IPS" },
	{ "Movies", u8"Videos" },
	{ "NetPlay", u8"Juego Online" },
	{ "Region", u8"Región" },
	{ "SaveStates", u8"Partidas Guardadas" },
	{ "ScreenshotSaved", u8"Captura Guardada" },
	{ "SoundRecorder", u8"Grabadora de Sonido" },
	{ "Test", u8"Test" },

	{ "ApplyingIps", u8"Applying patch: %1" },
	{ "CheatApplied", u8"1 truco aplicado." },
	{ "CheatsApplied", u8"%1 trucos aplicados." },
	{ "ConnectedToServer", u8"Conectado al servidor." },
	{ "ConnectedAsPlayer", u8"Conectado como jugador %1" },
	{ "ConnectedAsSpectator", u8"Conectado como espectador." },
	{ "ConnectionLost", u8"Conexión con el servidor perdida." },
	{ "CouldNotConnect", u8"No se puede conectar con el servidor." },
	{ "CouldNotIniitalizeAudioSystem", u8"No se puede iniciar el sistema de sonido" },
	{ "CouldNotFindRom", u8"No se ha podido encontrar la ROM buscada." },
	{ "CouldNotLoadFile", u8"No se puede cargar el archivo: %1" },
	{ "EmulationMaximumSpeed", u8"Velocidad Máxima" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "GameCrash", u8"El juego se ha colgado (%1)" },
	{ "Lag", u8"Lag" },
	{ "Mapper", u8"Mapeado: %1, SubMapeado: %2" },
	{ "MovieEnded", u8"Video terminado." },
	{ "MovieInvalid", u8"Tipo de video no válido." },
	{ "MovieMissingRom", u8"La ROM (%1) del video seleccionado no se encuentra." },
	{ "MovieNewerVersion", u8"No se pueden cargar videos creados con una versión mas reciente de Mesen. Por favor descargue la última versión." },
	{ "MovieIncompatibleVersion", u8"Este video es incompatible con esta versión de Mesen." },
	{ "MoviePlaying", u8"Reproduciendo video: %1" },
	{ "MovieRecordingTo", u8"Grabando a: %1" },
	{ "MovieSaved", u8"Video guardado en el archivo: %1" },
	{ "NetplayVersionMismatch", u8"%1 no está ejecutando la misma versión de Mesen y ha sido desconectado." },
	{ "PrgSizeWarning", u8"El tamaño del PRG es menor de 32kb" },
	{ "SaveStateEmpty", u8"La partida guardada está vacia." },
	{ "SaveStateIncompatibleVersion", u8"Partida guardada #%1 incompatible con esta versión de Mesen." },
	{ "SaveStateInvalidFile", u8"Partida guardada no válida." },
	{ "SaveStateLoaded", u8"Partida #%1 cargada." },
	{ "SaveStateNewerVersion", u8"No se puede cargar una partida creada con una versión mas reciente de Mesen. Por favor descargue la última versión." },
	{ "SaveStateSaved", u8"Partida #%1 guardada." },
	{ "ScanlineTimingWarning", u8"El timing de PPU ha sido cambiado." },
	{ "ServerStarted", u8"Servidor iniciado (Puerto: %1)" },
	{ "ServerStopped", u8"Servidor detenido" },
	{ "SoundRecorderStarted", u8"Grabando en: %1" },
	{ "SoundRecorderStopped", u8"Grabación guardada en: %1" },
	{ "TestFileSavedTo", u8"Archivo test guardado en: %1" },
	{ "UnsupportedMapper", u8"Mapa (%1) no soportado, no se puede cargar el juego." },

	{ "GoogleDrive", u8"Google Drive" },
	{ "SynchronizationStarted", u8"Sincronización iniciada." },
	{ "SynchronizationFailed", u8"Sincronización fallida." },
	{ "SynchronizationCompleted", u8"Sincronización completada." },
};

std::unordered_map<string, string> MessageManager::_ukResources = {
	{ "Cheats", u8"Чiти" },
	{ "Debug", u8"Налагодження" },
	{ "EmulationSpeed", u8"Швидкість емуляції" },
	{ "ClockRate", u8"Тактова частота" },
	{ "Error", u8"Помилка" },
	{ "GameInfo", u8"Інформація про гру" },
	{ "GameLoaded", u8"Гра завантажена" },
	{ "IPS", u8"IPS" },
	{ "Movies", u8"Записи" },
	{ "NetPlay", u8"Гра по мережi" },
	{ "Region", u8"Регiон" },
	{ "SaveStates", u8"Збереження" },
	{ "ScreenshotSaved", u8"Скріншот збережений" },
	{ "SoundRecorder", u8"Запис звуку" },
	{ "Test", u8"Тест" },

	{ "ApplyingIps", u8"Застосування патчу: %1" },
	{ "CheatApplied", u8"1 Чiт застосований." },
	{ "CheatsApplied", u8"Чiтів застосовано %1" },
	{ "ConnectedToServer", u8"Підключення до сервера." },
	{ "ConnectedAsPlayer", u8"Пiдключен як гравець %1" },
	{ "ConnectedAsSpectator", u8"Підключений як спостерігач." },
	{ "ConnectionLost", u8"З'єднання втрачено." },
	{ "CouldNotConnect", u8"Не вдалося підключитися до сервера" },
	{ "CouldNotIniitalizeAudioSystem", u8"Не вдалося ініціалізувати звукову підсистему" },
	{ "CouldNotFindRom", u8"Не вдалося знайти відповідний ROM." },
	{ "CouldNotLoadFile", u8"Не вдалося завантажити файл: %1" },
	{ "EmulationMaximumSpeed", u8"Максимальна швидкiсть" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "GameCrash", u8"Гра була аварійно завершена (%1)" },
	{ "Lag", u8"Лаг" },
	{ "Mapper", u8"Mapper: %1, SubMapper: %2" },
	{ "MovieEnded", u8"Відтворення закінчено." },
	{ "MovieInvalid", u8"Некоректна запис" },
	{ "MovieMissingRom", u8"Вiдсутнiй ROM (%1) необхідний для відтворення" },
	{ "MovieNewerVersion", u8"Запис створена в більш нової версії Mesen. Будь ласка завантажте останню версію." },
	{ "MovieIncompatibleVersion", u8"Ця запись не сумісна з вашою версією Mesen" },
	{ "MoviePlaying", u8"Відтворення запису: %1" },
	{ "MovieRecordingTo", u8"Запис розпочато: %1" },
	{ "MovieSaved", u8"Запис збережена: %1" },
	{ "NetplayVersionMismatch", u8"Версія Mesen відрізняється. %1 був відключений." },
	{ "PrgSizeWarning", u8"Розмiр PRG менше 32kb" },
	{ "SaveStateEmpty", u8"Слот порожнiй." },
	{ "SaveStateIncompatibleVersion", u8"Збереження #%1 несумісне з вашою версією Mesen." },
	{ "SaveStateInvalidFile", u8"Некоректне збереження." },
	{ "SaveStateLoaded", u8"Збереження #%1 завантажено." },
	{ "SaveStateNewerVersion", u8"Збереження створено в більш нової версії Mesen. Будь ласка завантажте останню версію." },
	{ "SaveStateSaved", u8"Збережено в #%1 слот." },
	{ "ScanlineTimingWarning", u8"Таймiнг PPU був змінений." },
	{ "ServerStarted", u8"Сервер запущено (Порт: %1)" },
	{ "ServerStopped", u8"Сервер зупинено" },
	{ "SoundRecorderStarted", u8"Запис розпочато to: %1" },
	{ "SoundRecorderStopped", u8"Запис збережена: %1" },
	{ "TestFileSavedTo", u8"Тест збережений: %1" },
	{ "UnsupportedMapper", u8"Непідтримуваний mapper (%1), гра не завантажена." },

	{ "Google Диск", u8"Google Drive" },
	{ "SynchronizationStarted", u8"Синхронізацію розпочато." },
	{ "SynchronizationFailed", u8"Синхронізація не вдалася." },
	{ "SynchronizationCompleted", u8"Синхронізація завершена." }
};

std::list<string> MessageManager::_log;
SimpleLock MessageManager::_logLock;
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
		case Language::Russian: resources = &_ruResources; break;
		case Language::Spanish: resources = &_esResources; break;
		case Language::Ukrainian: resources = &_ukResources; break;
	}
	if(resources) {
		if(resources->find(key) != resources->end()) {
			return (*resources)[key];
		} else if(EmulationSettings::GetDisplayLanguage() != Language::English) {
			//Fallback on English if resource key is missing another language
			resources = &_enResources;
			if(resources->find(key) != resources->end()) {
				return (*resources)[key];
			}
		}
	}

	return key;
}

void MessageManager::DisplayMessage(string title, string message, string param1, string param2)
{
	if(MessageManager::_messageManager) {
		title = Localize(title);
		message = Localize(message);

		size_t startPos = message.find(u8"%1");
		if(startPos != std::string::npos) {
			message.replace(startPos, 2, param1);
		}

		startPos = message.find(u8"%2");
		if(startPos != std::string::npos) {
			message.replace(startPos, 2, param2);
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

void MessageManager::Log(string message)
{
	auto lock = _logLock.AcquireSafe();
	if(message.empty()) {
		message = "------------------------------------------------------";
	}
	if(_log.size() >= 1000) {
		_log.pop_front();
	}
	_log.push_back(message);
}

string MessageManager::GetLog()
{
	auto lock = _logLock.AcquireSafe();
	stringstream ss;
	for(string &msg : _log) {
		ss << msg << "\n";
	}
	return ss.str();
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