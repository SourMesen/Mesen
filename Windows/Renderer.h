#pragma once

#include "stdafx.h"
#include "../Core/IVideoDevice.h"
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
	class Renderer : public IVideoDevice, public IMessageManager
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
		uint16_t*					_ppuOutputBuffer = nullptr;
		uint16_t*					_ppuOutputSecondaryBuffer = nullptr;
		SimpleLock					_frameLock;

		bool _isHD = false;

		Timer _fpsTimer;
		uint32_t _frameCount = 0;
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
		//ID3D11PixelShader* _pixelShader = nullptr;

		const uint32_t _bytesPerPixel = 4;
		uint32_t _hdScreenWidth = 0;
		uint32_t _hdScreenHeight = 0;
		uint32_t _hdScreenBufferSize = 0;
		HdPpuPixelInfo *_hdScreenTiles = nullptr;
		HdPpuPixelInfo *_secondaryHdScreenTiles = nullptr;

		list<shared_ptr<ToastInfo>> _toasts;
		ID3D11ShaderResourceView* _toastTexture = nullptr;

		HRESULT InitDevice();
		void CleanupDevice();

		void SetScreenSize();

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

		bool Render();
		void DisplayMessage(string title, string message);
		void UpdateFrame(void* frameBuffer);
		void UpdateHdFrame(void *frameBuffer, HdPpuPixelInfo *screenTiles);
		void DisplayToast(shared_ptr<ToastInfo> toast);
	};
}