#pragma once

#include "stdafx.h"
#include "../Core/IRenderingDevice.h"
#include "../Core/IMessageManager.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/SimpleLock.h"
#include "../Utilities/Timer.h"

using namespace DirectX;

namespace DirectX {
	class SpriteBatch;
	class SpriteFont;
}

namespace NES {
	class Renderer : public IRenderingDevice, public IMessageManager
	{
	private:
		HWND                    _hWnd = nullptr;

		D3D_DRIVER_TYPE         _driverType = D3D_DRIVER_TYPE_NULL;
		D3D_FEATURE_LEVEL       _featureLevel = D3D_FEATURE_LEVEL_11_0;
		ID3D11Device*           _pd3dDevice = nullptr;
		ID3D11Device1*          _pd3dDevice1 = nullptr;
		ID3D11DeviceContext*    _pDeviceContext = nullptr;
		ID3D11DeviceContext1*   _pDeviceContext1 = nullptr;
		IDXGISwapChain*         _pSwapChain = nullptr;
		ID3D11RenderTargetView* _pRenderTargetView = nullptr;
		ID3D11DepthStencilState* _pDepthDisabledStencilState = nullptr;
		ID3D11BlendState*			_pAlphaEnableBlendingState = nullptr;


		ID3D11SamplerState*		_samplerState = nullptr;
		
		ID3D11Texture2D*			_pTexture = nullptr;
		uint32_t*					_videoRAM = nullptr;

		bool							_frameChanged = true;
		SimpleLock					_frameLock;

		Timer _fpsTimer;
		uint32_t _lastFrameCount = 0;
		uint32_t _renderedFrameCount = 0;
		uint32_t _lastRenderedFrameCount = 0;
		uint32_t _currentFPS = 0;
		uint32_t _currentRenderedFPS = 0;

		unique_ptr<SpriteFont>	_font;
		unique_ptr<SpriteFont>	_smallFont;
		ID3D11Texture2D*			_overlayTexture = nullptr;
		byte*							_overlayBuffer = nullptr;
		
		unique_ptr<SpriteBatch> _spriteBatch;

		const uint32_t _bytesPerPixel = 4;
		uint32_t _screenWidth = 0;
		uint32_t _screenHeight = 0;
		uint32_t _screenBufferSize = 0;

		uint32_t _nesFrameHeight = 0;
		uint32_t _nesFrameWidth = 0;
		uint32_t _newFrameBufferSize = 0;

		list<shared_ptr<ToastInfo>> _toasts;
		ID3D11ShaderResourceView* _toastTexture = nullptr;

		HRESULT InitDevice();
		void CleanupDevice();

		void SetScreenSize(uint32_t width, uint32_t height);

		ID3D11Texture2D* CreateTexture(uint32_t width, uint32_t height);
		ID3D11ShaderResourceView* GetShaderResourceView(ID3D11Texture2D* texture);
		void DrawNESScreen();
		void DrawPauseScreen();

		std::wstring WrapText(string text, SpriteFont* font, float maxLineWidth);
		void DrawOutlinedString(string message, float x, float y, DirectX::FXMVECTOR color, float scale);

		void DrawToasts();
		void DrawToast(shared_ptr<ToastInfo> toast, int posIndex);
		void RemoveOldToasts();
	
	public:
		Renderer(HWND hWnd);
		~Renderer();

		void Render();
		void DisplayMessage(string title, string message);
		void DisplayToast(shared_ptr<ToastInfo> toast);

		void UpdateFrame(void *frameBuffer, uint32_t width, uint32_t height);
	};
}