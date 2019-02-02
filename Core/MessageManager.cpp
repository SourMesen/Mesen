﻿#include "stdafx.h"
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
	{ "Input", u8"Input" },
	{ "Patch", u8"Patch" },
	{ "Movies", u8"Movies" },
	{ "NetPlay", u8"Net Play" },
	{ "Region", u8"Region" },
	{ "SaveStates", u8"Save States" },
	{ "ScreenshotSaved", u8"Screenshot Saved" },
	{ "SoundRecorder", u8"Sound Recorder" },
	{ "Test", u8"Test" },
	{ "VideoRecorder", u8"Video Recorder" },

	{ "ApplyingPatch", u8"Applying patch: %1" },
	{ "CheatApplied", u8"1 cheat applied." },
	{ "CheatsApplied", u8"%1 cheats applied." },
	{ "CheatsDisabled", u8"All cheats disabled." },
	{ "CoinInsertedSlot", u8"Coin inserted (slot %1)" },
	{ "ConnectedToServer", u8"Connected to server." },
	{ "ConnectedAsPlayer", u8"Connected as player %1" },
	{ "ConnectedAsSpectator", u8"Connected as spectator." },
	{ "ConnectionLost", u8"Connection to server lost." },
	{ "CouldNotConnect", u8"Could not connect to the server." },
	{ "CouldNotInitializeAudioSystem", u8"Could not initialize audio system" },
	{ "CouldNotFindRom", u8"Could not find matching game ROM." },
	{ "CouldNotLoadFile", u8"Could not load file: %1" },
	{ "EmulationMaximumSpeed", u8"Maximum speed" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "FdsDiskInserted", u8"Disk %1 Side %2 inserted." },
	{ "Frame", u8"Frame" },
	{ "GameCrash", u8"Game has crashed (%1)" },
	{ "KeyboardModeDisabled", u8"Keyboard mode disabled." },
	{ "KeyboardModeEnabled", u8"Keyboard mode enabled." },
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
	{ "SaveStateIncompatibleVersion", u8"Save state is incompatible with this version of Mesen." },
	{ "SaveStateInvalidFile", u8"Invalid save state file." },
	{ "SaveStateLoaded", u8"State #%1 loaded." },
	{ "SaveStateMissingRom", u8"Missing ROM required (%1) to load save state." },
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
	{ "VideoRecorderStarted", u8"Recording to: %1" },
	{ "VideoRecorderStopped", u8"Recording saved to: %1" },

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
	{ "Input", u8"Contrôles" },
	{ "Patch", u8"Patch" },
	{ "Movies", u8"Films" },
	{ "NetPlay", u8"Jeu en ligne" },
	{ "Region", u8"Région" },
	{ "SaveStates", u8"Sauvegardes" },
	{ "ScreenshotSaved", u8"Capture d'écran" },
	{ "SoundRecorder", u8"Enregistreur audio" },
	{ "Test", u8"Test" },
	{ "VideoRecorder", u8"Enregistreur vidéo" },

	{ "ApplyingPatch", u8"Patch appliqué : %1" },
	{ "CheatApplied", u8"%1 code activé." },
	{ "CheatsApplied", u8"%1 codes activés." },
	{ "CheatsDisabled", u8"Tous les codes ont étés désactivés." },
	{ "CoinInsertedSlot", u8"Pièce insérée (%1)" },
	{ "ConnectedToServer", u8"Connecté avec succès au serveur." },
	{ "ConnectedAsPlayer", u8"Connecté en tant que joueur #%1" },
	{ "ConnectedAsSpectator", u8"Connecté en tant que spectateur." },
	{ "ConnectionLost", u8"La connexion avec le serveur a été perdue." },
	{ "CouldNotConnect", u8"Impossible de se connecter au serveur." },
	{ "CouldNotInitializeAudioSystem", u8"L'initialisation du système de son à échoué" },
	{ "CouldNotFindRom", u8"Impossible de trouvé un rom correspondant." },
	{ "CouldNotLoadFile", u8"Impossible de charger le fichier : %1" },
	{ "EmulationMaximumSpeed", u8"Vitesse maximale" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "FdsDiskInserted", u8"Disque %1 Côté %2 inséré." },
	{ "Frame", u8"Image" },
	{ "GameCrash", u8"Le jeu a planté (%1)" },
	{ "KeyboardModeDisabled", u8"Mode clavier activé." },
	{ "KeyboardModeEnabled", u8"Mode clavier désactivé." },
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
	{ "SaveStateIncompatibleVersion", u8"La sauvegarde est incompatible avec cette version de Mesen." },
	{ "SaveStateInvalidFile", u8"Fichier de sauvegarde invalide ou corrompu." },
	{ "SaveStateLoaded", u8"Sauvegarde #%1 chargée." },
	{ "SaveStateMissingRom", u8"Le rom (%1) correspondant à la sauvegarde rapide sélectionnée est introuvable." },
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
	{ "VideoRecorderStarted", u8"En cours d'enregistrement : %1" },
	{ "VideoRecorderStopped", u8"Enregistrement audio sauvegardé : %1" },

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
	{ "Input", u8"コントローラ" },
	{ "Patch", u8"パッチ" },
	{ "Movies", u8"動画" },
	{ "NetPlay", u8"ネットプレー" },
	{ "Region", u8"地域" },
	{ "SaveStates", u8"クイックセーブ" },
	{ "ScreenshotSaved", u8"スクリーンショット" },
	{ "SoundRecorder", u8"サウンドレコーダー" },
	{ "Test", u8"テスト" },
	{ "VideoRecorder", u8"動画レコーダー" },

	{ "ApplyingPatch", u8"パッチファイルを適用しました:　%1" },
	{ "CheatApplied", u8"チートコード%1個を有効にしました。" },
	{ "CheatsApplied", u8"チートコード%1個を有効にしました。" },
	{ "CheatsDisabled", u8"チートコードを無効にしました。" },
	{ "CoinInsertedSlot", u8"インサートコイン (%1)" },
	{ "ConnectedToServer", u8"サーバに接続しました。" },
	{ "ConnectedAsPlayer", u8"プレーヤー %1として接続しました。" },
	{ "ConnectedAsSpectator", u8"観客として接続しました。" },
	{ "ConnectionLost", u8"サーバから切断されました。" },
	{ "CouldNotConnect", u8"サーバに接続出来ませんでした。" },
	{ "CouldNotInitializeAudioSystem", u8"オーディオデバイスを初期化出来ませんでした。" },
	{ "CouldNotFindRom", u8"該当するゲームファイルは見つかりませんでした。" },
	{ "CouldNotLoadFile", u8"ファイルをロードできませんでした: %1" },
	{ "EmulationMaximumSpeed", u8"最高速度" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "FdsDiskInserted", u8"ディスク%1%2。" },
	{ "Frame", u8"フレーム" },
	{ "GameCrash", u8"ゲームは停止しました (%1)" },
	{ "KeyboardModeDisabled", u8"キーボードモードを無効にしました。" },
	{ "KeyboardModeEnabled", u8"キーボードモードを有効にしました。" },
	{ "Lag", u8"ラグ" },
	{ "Mapper", u8"Mapper: %1, SubMapper: %2" },
	{ "MovieEnded", u8"動画の再生が終了しました。" },
	{ "MovieInvalid", u8"動画データの読み込みに失敗しました。" },
	{ "MovieMissingRom", u8"動画に必要なゲームファイルを見つかりませんでした。(%1)" },
	{ "MovieNewerVersion", u8"この動画は使用中のMesenより新しいバージョンで作られたため、 ロードできません。　Mesenのサイトで最新のバージョンをダウンロードしてください。" },
	{ "MovieIncompatibleVersion", u8"この動画は古いMesenのバージョンで作られたもので、 ロードできませんでした。" },
	{ "MoviePlaying", u8"動画再生: %1" },
	{ "MovieRecordingTo", u8"%1に録画しています。" },
	{ "MovieSaved", u8"録画を終了しました: %1" },
	{ "NetplayVersionMismatch", u8"%1さんはMesenの別のバージョンを使っているため、接続はできませんでした。" },
	{ "PrgSizeWarning", u8"PRG size is smaller than 32kb" },
	{ "SaveStateEmpty", u8"セーブデータがありませんでした。" },
	{ "SaveStateIncompatibleVersion", u8"クイックセーブデータは古いMesenのバージョンで作られたため、 ロードできませんでした。" },
	{ "SaveStateInvalidFile", u8"クイックセーブデータを読めませんでした。" },
	{ "SaveStateLoaded", u8"クイックセーブ%1をロードしました。" },
	{ "SaveStateMissingRom", u8"クイックセーブデータをロードするためのゲームファイルを見つかりませんでした。(%1)" },
	{ "SaveStateNewerVersion", u8"クイックセーブデータは使用中のMesenより新しいバージョンで作られたため、 ロードできません。　Mesenのサイトで最新のバージョンをダウンロードしてください。" },
	{ "SaveStateSaved", u8"クイックセーブ%1をセーブしました。" },
	{ "SaveStateSlotSelected", u8"クイックセーブスロット%1。" },
	{ "ServerStarted", u8"サーバは起動しました (ポート: %1)" },
	{ "ServerStopped", u8"サーバは停止しました。" },
	{ "ScanlineTimingWarning", u8"PPUのタイミングは変更されました。" },
	{ "SoundRecorderStarted", u8"%1に録音しています。" },
	{ "SoundRecorderStopped", u8"録音を終了しました: %1" },
	{ "TestFileSavedTo", u8"Test file saved to: %1" },
	{ "UnsupportedMapper", u8"このMapper (%1)を使うゲームはロードできません。" },
	{ "VideoRecorderStarted", u8"%1に録画しています。" },
	{ "VideoRecorderStopped", u8"録画を終了しました: %1" },

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
	{ "Input", u8"Input" },
	{ "Patch", u8"Patch" },
	{ "Movies", u8"Записи" },
	{ "NetPlay", u8"Игра по сети" },
	{ "Region", u8"Регион" },
	{ "SaveStates", u8"Сохранения" },
	{ "ScreenshotSaved", u8"Скриншот сохранён" },
	{ "SoundRecorder", u8"Запись звука" },
	{ "Test", u8"Тест" },
	{ "VideoRecorder", u8"Video Recorder" },

	{ "ApplyingPatch", u8"Применён патч: %1" },
	{ "CheatApplied", u8"1 Чит применён." },
	{ "CheatsApplied", u8"Читов применено %1" },
	{ "CheatsDisabled", u8"All cheats disabled." },
	{ "CoinInsertedSlot", u8"Coin inserted (slot %1)" },
	{ "ConnectedToServer", u8"Подключение к серверу." },
	{ "ConnectedAsPlayer", u8"Подключен как игрок %1" },
	{ "ConnectedAsSpectator", u8"Подключен как наблюдатель." },
	{ "ConnectionLost", u8"Соединение потеряно." },
	{ "CouldNotConnect", u8"Не удалось подключиться к серверу" },
	{ "CouldNotInitializeAudioSystem", u8"Не удалось инициализировать звуковую подсистему" },
	{ "CouldNotFindRom", u8"Не удалось найти подходящий ROM." },
	{ "CouldNotLoadFile", u8"Не удалось загрузить файл: %1" },
	{ "EmulationMaximumSpeed", u8"Максимальная скорость" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "FdsDiskInserted", u8"Disk %1 Side %2 inserted." },
	{ "Frame", u8"Frame" },
	{ "GameCrash", u8"Игра была аварийно завершена (%1)" },
	{ "KeyboardModeDisabled", u8"Keyboard mode disabled." },
	{ "KeyboardModeEnabled", u8"Keyboard mode enabled." },
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
	{ "SaveStateIncompatibleVersion", u8"Сохранение несовместимо с вашей версией Mesen." },
	{ "SaveStateInvalidFile", u8"Некорректное сохранение." },
	{ "SaveStateLoaded", u8"Сохранение #%1 загружено." },
	{ "SaveStateMissingRom", u8"Missing ROM required (%1) to load save state." },
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
	{ "VideoRecorderStarted", u8"Запись начата to: %1" },
	{ "VideoRecorderStopped", u8"Запись сохранена: %1" },

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
	{ "Input", u8"Input" },
	{ "Patch", u8"Patch" },
	{ "Movies", u8"Videos" },
	{ "NetPlay", u8"Juego Online" },
	{ "Region", u8"Región" },
	{ "SaveStates", u8"Partidas Guardadas" },
	{ "ScreenshotSaved", u8"Captura Guardada" },
	{ "SoundRecorder", u8"Grabadora de Sonido" },
	{ "Test", u8"Test" },
	{ "VideoRecorder", u8"Video Recorder" },

	{ "ApplyingPatch", u8"Aplicando parche: %1" },
	{ "CheatApplied", u8"1 truco aplicado." },
	{ "CheatsApplied", u8"%1 trucos aplicados." },
	{ "CheatsDisabled", u8"Todos los trucos deshabilitados." },
	{ "CoinInsertedSlot", u8"Coin inserted (slot %1)" },
	{ "ConnectedToServer", u8"Conectado al servidor." },
	{ "ConnectedAsPlayer", u8"Conectado como jugador %1" },
	{ "ConnectedAsSpectator", u8"Conectado como espectador." },
	{ "ConnectionLost", u8"Conexión con el servidor perdida." },
	{ "CouldNotConnect", u8"No se puede conectar con el servidor." },
	{ "CouldNotInitializeAudioSystem", u8"No se puede iniciar el sistema de sonido" },
	{ "CouldNotFindRom", u8"No se ha podido encontrar la ROM buscada." },
	{ "CouldNotLoadFile", u8"No se puede cargar el archivo: %1" },
	{ "EmulationMaximumSpeed", u8"Velocidad Máxima" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "FdsDiskInserted", u8"Disk %1 Side %2 inserted." },
	{ "Frame", u8"Frame" },
	{ "GameCrash", u8"El juego se ha colgado (%1)" },
	{ "KeyboardModeDisabled", u8"Keyboard mode disabled." },
	{ "KeyboardModeEnabled", u8"Keyboard mode enabled." },
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
	{ "SaveStateEmpty", u8"La partida guardada está vacía." },
	{ "SaveStateIncompatibleVersion", u8"Partida guardada incompatible con esta versión de Mesen." },
	{ "SaveStateInvalidFile", u8"Partida guardada no válida." },
	{ "SaveStateLoaded", u8"Partida #%1 cargada." },
	{ "SaveStateMissingRom", u8"Missing ROM required (%1) to load save state." },
	{ "SaveStateNewerVersion", u8"No se puede cargar una partida creada con una versión mas reciente de Mesen. Por favor descargue la última versión." },
	{ "SaveStateSaved", u8"Partida #%1 guardada." },
	{ "SaveStateSlotSelected", u8"Espacio de guardado #%1 elegido." },
	{ "ScanlineTimingWarning", u8"El timing de PPU ha sido cambiado." },
	{ "ServerStarted", u8"Servidor iniciado (Puerto: %1)" },
	{ "ServerStopped", u8"Servidor detenido" },
	{ "SoundRecorderStarted", u8"Grabando en: %1" },
	{ "SoundRecorderStopped", u8"Grabación guardada en: %1" },
	{ "TestFileSavedTo", u8"Archivo test guardado en: %1" },
	{ "UnsupportedMapper", u8"Mapa (%1) no soportado, no se puede cargar el juego." },
	{ "VideoRecorderStarted", u8"Grabando en: %1" },
	{ "VideoRecorderStopped", u8"Grabación guardada en: %1" },


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
	{ "Input", u8"Input" },
	{ "Patch", u8"Patch" },
	{ "Movies", u8"Записи" },
	{ "NetPlay", u8"Гра по мережi" },
	{ "Region", u8"Регiон" },
	{ "SaveStates", u8"Збереження" },
	{ "ScreenshotSaved", u8"Скріншот збережений" },
	{ "SoundRecorder", u8"Запис звуку" },
	{ "Test", u8"Тест" },
	{ "VideoRecorder", u8"Video Recorder" },

	{ "ApplyingPatch", u8"Застосовано патч: %1" },
	{ "CheatApplied", u8"1 Чiт застосований." },
	{ "CheatsApplied", u8"Чiтів застосовано %1" },
	{ "CheatsDisabled", u8"All cheats disabled." },
	{ "CoinInsertedSlot", u8"Coin inserted (slot %1)" },
	{ "ConnectedToServer", u8"Підключення до сервера." },
	{ "ConnectedAsPlayer", u8"Пiдключен як гравець %1" },
	{ "ConnectedAsSpectator", u8"Підключений як спостерігач." },
	{ "ConnectionLost", u8"З'єднання втрачено." },
	{ "CouldNotConnect", u8"Не вдалося підключитися до сервера" },
	{ "CouldNotInitializeAudioSystem", u8"Не вдалося ініціалізувати звукову підсистему" },
	{ "CouldNotFindRom", u8"Не вдалося знайти відповідний ROM." },
	{ "CouldNotLoadFile", u8"Не вдалося завантажити файл: %1" },
	{ "EmulationMaximumSpeed", u8"Максимальна швидкiсть" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "FdsDiskInserted", u8"Disk %1 Side %2 inserted." },
	{ "Frame", u8"Frame" },
	{ "GameCrash", u8"Гра була аварійно завершена (%1)" },
	{ "KeyboardModeDisabled", u8"Keyboard mode disabled." },
	{ "KeyboardModeEnabled", u8"Keyboard mode enabled." },
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
	{ "SaveStateIncompatibleVersion", u8"Збереження несумісне з вашою версією Mesen." },
	{ "SaveStateInvalidFile", u8"Некоректне збереження." },
	{ "SaveStateLoaded", u8"Збереження #%1 завантажено." },
	{ "SaveStateMissingRom", u8"Missing ROM required (%1) to load save state." },
	{ "SaveStateNewerVersion", u8"Збереження створено в більш нової версії Mesen. Будь ласка завантажте останню версію." },
	{ "SaveStateSaved", u8"Збережено в #%1 слот." },
	{ "ScanlineTimingWarning", u8"Таймiнг PPU був змінений." },
	{ "ServerStarted", u8"Сервер запущено (Порт: %1)" },
	{ "SaveStateSlotSelected", u8"Обрано #%1 слот." },
	{ "ServerStopped", u8"Сервер зупинено" },
	{ "SoundRecorderStarted", u8"Запис розпочато to: %1" },
	{ "SoundRecorderStopped", u8"Запис збережена: %1" },
	{ "TestFileSavedTo", u8"Тест збережений: %1" },
	{ "UnsupportedMapper", u8"Непідтримуваний mapper (%1), гра не завантажена." },
	{ "VideoRecorderStarted", u8"Запис розпочато to: %1" },
	{ "VideoRecorderStopped", u8"Запис збережена: %1" },

	{ "GoogleDrive", u8"Google Диск" },
	{ "SynchronizationStarted", u8"Синхронізацію розпочато." },
	{ "SynchronizationFailed", u8"Синхронізація не вдалася." },
	{ "SynchronizationCompleted", u8"Синхронізація завершена." }
};

std::unordered_map<string, string> MessageManager::_ptResources = {
	{ "Cheats", u8"Trapaças" },
	{ "Debug", u8"Depuração" },
	{ "EmulationSpeed", u8"Velocidade" },
	{ "ClockRate", u8"Frequência do clock" },
	{ "Error", u8"Erro" },
	{ "GameInfo", u8"Informações do jogo" },
	{ "GameLoaded", u8"Jogo carregado" },
	{ "Input", u8"Controles" },
	{ "Patch", u8"Patch" },
	{ "Movies", u8"Vídeos" },
	{ "NetPlay", u8"Jogo online" },
	{ "Region", u8"Região" },
	{ "SaveStates", u8"Estados salvos" },
	{ "ScreenshotSaved", u8"Captura de tela salva" },
	{ "SoundRecorder", u8"Gravador de som" },
	{ "Test", u8"Teste" },
	{ "VideoRecorder", u8"Gravador de vídeo" },

	{ "ApplyingPatch", u8"Aplicando patch: %1" },
	{ "CheatApplied", u8"1 trapaça aplicada." },
	{ "CheatsApplied", u8"%1 trapaças aplicadas." },
	{ "CheatsDisabled", u8"Todas as trapaças desabilitadas." },
	{ "CoinInsertedSlot", u8"Ficha %1 inserida)" },
	{ "ConnectedToServer", u8"Conectado ao servidor." },
	{ "ConnectedAsPlayer", u8"Conectado como jogador %1" },
	{ "ConnectedAsSpectator", u8"Conectado como espectador." },
	{ "ConnectionLost", u8"Conexão com o servidor perdida." },
	{ "CouldNotConnect", u8"Não foi possível conectar ao servidor." },
	{ "CouldNotInitializeAudioSystem", u8"Não foi possível iniciar o sistema de som" },
	{ "CouldNotFindRom", u8"Não foi possível encontrar a ROM procurada." },
	{ "CouldNotLoadFile", u8"Não foi possível carregar o arquivo: %1" },
	{ "EmulationMaximumSpeed", u8"Velocidade máxima" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "FdsDiskInserted", u8"Disco %1 lado %2 inserido." },
	{ "Frame", u8"Quadro" },
	{ "GameCrash", u8"O jogo travou (%1)" },
	{ "KeyboardModeDisabled", u8"Modo teclado desabilitado." },
	{ "KeyboardModeEnabled", u8"Modo teclado habilitado." },
	{ "Lag", u8"Lag" },
	{ "Mapper", u8"Mapeador: %1, SubMapeado: %2" },
	{ "MovieEnded", u8"Vídeo terminado." },
	{ "MovieInvalid", u8"Tipo de vídeo inválido." },
	{ "MovieMissingRom", u8"A ROM (%1) do vídeo selecionado não se encontra." },
	{ "MovieNewerVersion", u8"Não se pode carregar vídeos criados com uma versão mais recente do Mesen. Por favor, baixe a última versão." },
	{ "MovieIncompatibleVersion", u8"Este vídeo é incompatível com esta versão do Mesen." },
	{ "MoviePlaying", u8"Reproduzindo vídeo: %1" },
	{ "MovieRecordingTo", u8"Gravando a: %1" },
	{ "MovieSaved", u8"Vídeo salvo no arquivo: %1" },
	{ "NetplayVersionMismatch", u8"%1 não está executando a mesma versão do Mesen e foi desconectado." },
	{ "PrgSizeWarning", u8"O tamanho do PRG é menor que 32kb" },
	{ "SaveStateEmpty", u8"O estado salvo está vazio." },
	{ "SaveStateIncompatibleVersion", u8"Estado salvo incompatível com esta versão do Mesen." },
	{ "SaveStateInvalidFile", u8"Estado salvo inválido." },
	{ "SaveStateLoaded", u8"Estado salvo #%1 carregado." },
	{ "SaveStateMissingRom", u8"Faltando rom requerida (%1) para carregar o estado salvo." },
	{ "SaveStateNewerVersion", u8"Não se pode carregar um estado salvo com uma versão mais recente do Mesen. Por favor, baixe a última versão." },
	{ "SaveStateSaved", u8"Estado salvo #%1 salvo." },
	{ "SaveStateSlotSelected", u8"Compartimento do estado salvo #%1 escolhido." },
	{ "ScanlineTimingWarning", u8"O timing da PPU foi trocado." },
	{ "ServerStarted", u8"Servidor iniciado (Port: %1)" },
	{ "ServerStopped", u8"Servidor parado" },
	{ "SoundRecorderStarted", u8"Gravando em: %1" },
	{ "SoundRecorderStopped", u8"Gravação salva em: %1" },
	{ "TestFileSavedTo", u8"Arquivo teste salvo em: %1" },
	{ "UnsupportedMapper", u8"Mapeador (%1) não suportado, não se pode carregar o jogo." },
	{ "VideoRecorderStarted", u8"Gravando em: %1" },
	{ "VideoRecorderStopped", u8"Gravação salva em: %1" },

	{ "GoogleDrive", u8"Google Drive" },
	{ "SynchronizationStarted", u8"Sincronização iniciada." },
	{ "SynchronizationFailed", u8"Sincronização falhou." },
	{ "SynchronizationCompleted", u8"Sincronização completada." },
};

std::unordered_map<string, string> MessageManager::_caResources = {
	{ "Cheats", u8"Trucs" },
	{ "Debug", u8"Depuració" },
	{ "EmulationSpeed", u8"Velocitat de l'emulació" },
	{ "ClockRate", u8"Freqüència de rellotge" },
	{ "Error", u8"Error" },
	{ "GameInfo", u8"Informació del joc" },
	{ "GameLoaded", u8"Joc carregat" },
	{ "Input", u8"Input" },
	{ "Patch", u8"Pedaç" },
	{ "Movies", u8"Pel·lícules" },
	{ "NetPlay", u8"Joc en línia" },
	{ "Region", u8"Regió" },
	{ "SaveStates", u8"Partides guardades" },
	{ "ScreenshotSaved", u8"Captura de pantalla" },
	{ "SoundRecorder", u8"Enregistrador de so" },
	{ "Test", u8"Prova" },
	{ "VideoRecorder", u8"Enregistrador de vídeo" },

	{ "ApplyingPatch", u8"Aplicant pedaç: %1" },
	{ "CheatApplied", u8"1 truc activat." },
	{ "CheatsApplied", u8"%1 trucs activats." },
	{ "CheatsDisabled", u8"Tots els trucs han estat desactivats." },
	{ "CoinInsertedSlot", u8"Coin inserted (slot %1)" },
	{ "ConnectedToServer", u8"Connectat al servidor." },
	{ "ConnectedAsPlayer", u8"Connectat com a %1" },
	{ "ConnectedAsSpectator", u8"Connectat com a espectador." },
	{ "ConnectionLost", u8"S'ha perdut la connexió amb el servidor." },
	{ "CouldNotConnect", u8"No s'ha pogut connectar amb el servidor." },
	{ "CouldNotInitializeAudioSystem", u8"No s'ha pogut inicialitzar el sistema de so" },
	{ "CouldNotFindRom", u8"Incapaç de trobar una ROM corresponent." },
	{ "CouldNotLoadFile", u8"Incapaç de carregar el fitxer: %1" },
	{ "EmulationMaximumSpeed", u8"Velocitat màxima" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "FdsDiskInserted", u8"Disk %1 Side %2 inserted." },
	{ "Frame", u8"Fotograma" },
	{ "GameCrash", u8"El joc ha fallat (%1)" },
	{ "KeyboardModeDisabled", u8"Keyboard mode disabled." },
	{ "KeyboardModeEnabled", u8"Keyboard mode enabled." },
	{ "Lag", u8"Retard" },
	{ "Mapper", u8"Mapat: %1, SubMapat: %2" },
	{ "MovieEnded", u8"Final de pel·lícula." },
	{ "MovieInvalid", u8"Fitxer de pel·lícula invàlid." },
	{ "MovieMissingRom", u8"No s'ha trobat la ROM necessària (%1) per reproduir la pel·lícula." },
	{ "MovieNewerVersion", u8"Incapaç de reproduir pel·lícules creades per una versió més recent de Mesen. Si us plau, descarregueu-vos la darrera versió de Mesen." },
	{ "MovieIncompatibleVersion", u8"Aquesta pel·lícula és incompatible amb aquesta versió de Mesen." },
	{ "MoviePlaying", u8"Reproduint pel·lícula: %1" },
	{ "MovieRecordingTo", u8"Enregistrament en curs: %1" },
	{ "MovieSaved", u8"Pel·lícula desada: %1" },
	{ "NetplayVersionMismatch", u8"%1 no fa servir la vostra mateixa versió de Mesen i ha estat desconnectat automàticament." },
	{ "PrgSizeWarning", u8"La mida del PRG és més petit que 32kb" },
	{ "SaveStateEmpty", u8"Aquesta partida guardada és buida." },
	{ "SaveStateIncompatibleVersion", u8"La partida guardada és incompatible amb aquesta versió de Mesen." },
	{ "SaveStateInvalidFile", u8"Fitxer de partida guardada invàlid." },
	{ "SaveStateLoaded", u8"Partida guardada #%1 carregada." },
	{ "SaveStateMissingRom", u8"No s'ha trobat la ROM necessària (%1) per carregar la partida guardada." },
	{ "SaveStateNewerVersion", u8"Incapaç de carregar partides guardades creades per una versió més recent de Mesen. Si us plau, descarregueu-vos la darrera versió de Mesen." },
	{ "SaveStateSaved", u8"Partida guardada #%1 desada." },
	{ "SaveStateSlotSelected", u8"Partida guardada #%1 seleccionada." },
	{ "ScanlineTimingWarning", u8"La sincronització de la PPU ha estat modificada." },
	{ "ServerStarted", u8"El servidor s'ha iniciat (Port: %1)" },
	{ "ServerStopped", u8"El servidor s'ha aturat" },
	{ "SoundRecorderStarted", u8"Enregistrament en curs: %1" },
	{ "SoundRecorderStopped", u8"Enregistrament de so desat: %1" },
	{ "TestFileSavedTo", u8"Prova desada: %1" },
	{ "UnsupportedMapper", u8"El mapat (%1) encara no està suportat - el joc no es pot iniciar." },
	{ "VideoRecorderStarted", u8"Enregistrament en curs: %1" },
	{ "VideoRecorderStopped", u8"Enregistrament de vídeo desat: %1" },

	{ "GoogleDrive", u8"Google Drive" },
	{ "SynchronizationStarted", u8"Sincronització en curs." },
	{ "SynchronizationFailed", u8"La sincronització ha fallat." },
	{ "SynchronizationCompleted", u8"Sincronització finalitzada." },
};

std::unordered_map<string, string> MessageManager::_zhResources = {
	{ "Cheats", u8"作弊" },
	{ "Debug", u8"调试" },
	{ "EmulationSpeed", u8"模拟速度" },
	{ "ClockRate", u8"时钟频率" },
	{ "Error", u8"错误" },
	{ "GameInfo", u8"游戏信息" },
	{ "GameLoaded", u8"游戏已加载" },
	{ "Input", u8"输入" },
	{ "Patch", u8"补丁" },
	{ "Movies", u8"影片" },
	{ "NetPlay", u8"网络游戏" },
	{ "Region", u8"地区" },
	{ "SaveStates", u8"保存状态" },
	{ "ScreenshotSaved", u8"截屏已保存" },
	{ "SoundRecorder", u8"录音机" },
	{ "Test", u8"测试" },
	{ "VideoRecorder", u8"录像机" },

	{ "ApplyingPatch", u8"应用补丁: %1" },
	{ "CheatApplied", u8"1 个作弊已应用." },
	{ "CheatsApplied", u8"%1 个作弊已应用." },
	{ "CheatsDisabled", u8"所有作弊已禁用." },
	{ "CoinInsertedSlot", u8"已投币 (投币口 %1)" },
	{ "ConnectedToServer", u8"已连接到服务器." },
	{ "ConnectedAsPlayer", u8"已作为玩家 %1 连接" },
	{ "ConnectedAsSpectator", u8"已作为观众连接." },
	{ "ConnectionLost", u8"丢失服务器连接." },
	{ "CouldNotConnect", u8"无法连接到服务器." },
	{ "CouldNotInitializeAudioSystem", u8"无法初始化音频系统" },
	{ "CouldNotFindRom", u8"找不到匹配的游戏 ROM." },
	{ "CouldNotLoadFile", u8"无法加载文件: %1" },
	{ "EmulationMaximumSpeed", u8"最高速" },
	{ "EmulationSpeedPercent", u8"%1%" },
	{ "FdsDiskInserted", u8"%1 盘 %2 面已插入." },
	{ "Frame", u8"帧" },
	{ "GameCrash", u8"游戏已崩溃 (%1)" },
	{ "KeyboardModeDisabled", u8"键盘模式已禁用." },
	{ "KeyboardModeEnabled", u8"键盘模式已启用." },
	{ "Lag", u8"延迟" },
	{ "Mapper", u8"Mapper: %1, 子 mapper: %2" },
	{ "MovieEnded", u8"影片已结束." },
	{ "MovieInvalid", u8"影片文件无效." },
	{ "MovieMissingRom", u8"丢失所需的 ROM (%1) 来播放影片." },
	{ "MovieNewerVersion", u8"无法加载由最新版本的 Mesen 创建的影片. 请下载最新版本." },
	{ "MovieIncompatibleVersion", u8"这部影片与此版本的 Mesen 不兼容." },
	{ "MoviePlaying", u8"正在播放影片: %1" },
	{ "MovieRecordingTo", u8"录制到: %1" },
	{ "MovieSaved", u8"影片已保存到: %1" },
	{ "NetplayVersionMismatch", u8"%1 没有运行相同版本的 Mesen 并且已断开连接." },
	{ "PrgSizeWarning", u8"PRG 大小小于 32kb" },
	{ "SaveStateEmpty", u8"插槽为空." },
	{ "SaveStateIncompatibleVersion", u8"进度与此版本的 Mesen 不兼容." },
	{ "SaveStateInvalidFile", u8"无效的进度文件." },
	{ "SaveStateLoaded", u8"进度 #%1 已加载." },
	{ "SaveStateMissingRom", u8"缺少所需 ROM (%1) 来加载进度." },
	{ "SaveStateNewerVersion", u8"无法加载由更新版本的 Mesen 创建的进度. 请下载最新版本." },
	{ "SaveStateSaved", u8"状态 #%1 已保存." },
	{ "SaveStateSlotSelected", u8"插槽 #%1 已被选中." },
	{ "ScanlineTimingWarning", u8"PPU 计时已更改." },
	{ "ServerStarted", u8"服务器已启动 (端口: %1)" },
	{ "ServerStopped", u8"服务器已停止" },
	{ "SoundRecorderStarted", u8"录制到: %1" },
	{ "SoundRecorderStopped", u8"录音文件已保存到: %1" },
	{ "TestFileSavedTo", u8"测试文件已保存到: %1" },
	{ "UnsupportedMapper", u8"不支持的 Mapper (%1), 无法加载游戏." },
	{ "VideoRecorderStarted", u8"录制到: %1" },
	{ "VideoRecorderStopped", u8"视频已保存到: %1" },

	{ "GoogleDrive", u8"Google Drive" },
	{ "SynchronizationStarted", u8"同步已开始." },
	{ "SynchronizationFailed", u8"同步失败." },
	{ "SynchronizationCompleted", u8"同步已完成." },
};

std::list<string> MessageManager::_log;
SimpleLock MessageManager::_logLock;
SimpleLock MessageManager::_messageLock;
bool MessageManager::_osdEnabled = false;
IMessageManager* MessageManager::_messageManager = nullptr;

void MessageManager::RegisterMessageManager(IMessageManager* messageManager)
{
	auto lock = _messageLock.AcquireSafe();
	MessageManager::_messageManager = messageManager;
}

void MessageManager::UnregisterMessageManager(IMessageManager* messageManager)
{
	auto lock = _messageLock.AcquireSafe();
	if(MessageManager::_messageManager == messageManager) {
		MessageManager::_messageManager = nullptr;
	}
}

void MessageManager::SetOsdState(bool enabled)
{
	_osdEnabled = enabled;
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
		case Language::Portuguese: resources = &_ptResources; break;
		case Language::Catalan: resources = &_caResources; break;
		case Language::Chinese: resources = &_zhResources; break;
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
		auto lock = _messageLock.AcquireSafe();
		if(!MessageManager::_messageManager) {
			return;
		}

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

		if(_osdEnabled) {
			MessageManager::_messageManager->DisplayMessage(title, message);
		} else {
			MessageManager::Log("[" + title + "] " + message);
		}
	}
}

void MessageManager::Log(string message)
{
#ifndef LIBRETRO
	auto lock = _logLock.AcquireSafe();
	if(message.empty()) {
		message = "------------------------------------------------------";
	}
	if(_log.size() >= 1000) {
		_log.pop_front();
	}
	_log.push_back(message);
#else
	if(MessageManager::_messageManager) {
		MessageManager::_messageManager->DisplayMessage("", message + "\n");
	}
#endif
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
