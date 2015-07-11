#pragma once

#include "stdafx.h"
#include "../Core/IVideoDevice.h"
#include "../Core/IMessageManager.h"
#include "../Utilities/PNGWriter.h"
#include "../Utilities/FolderUtilities.h"
#include "../Utilities/SimpleLock.h"

using namespace DirectX;

namespace DirectX {
	class SpriteBatch;
	class SpriteFont;
}

namespace NES {
	enum UIFlags
	{
		ShowFPS = 1,
	};

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
		byte*							_videoRAM = nullptr;

		bool							_frameChanged = true;
		uint8_t*						_nextFrameBuffer = nullptr;
		SimpleLock					_frameLock;


		unique_ptr<SpriteFont>	_font;
		unique_ptr<SpriteFont>	_smallFont;
		ID3D11Texture2D*			_overlayTexture = nullptr;
		byte*							_overlayBuffer = nullptr;
		
		unique_ptr<SpriteBatch> _spriteBatch;
		//ID3D11PixelShader* _pixelShader = nullptr;

		uint32_t _screenWidth;
		uint32_t _screenHeight;
		uint32_t _bytesPerPixel;
		uint32_t _hdScreenWidth;
		uint32_t _hdScreenHeight;
		uint32_t _screenBufferSize;
		uint32_t _hdScreenBufferSize;

		uint32_t _flags = 0;

		list<shared_ptr<ToastInfo>> _toasts;
		ID3D11ShaderResourceView* _toastTexture;

		HRESULT InitDevice();
		void CleanupDevice();

		void SetScreenSize(uint32_t screenWidth, uint32_t screenHeight);

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
		
		void SetFlags(uint32_t flags)
		{
			_flags |= flags;
		}

		void ClearFlags(uint32_t flags)
		{
			_flags &= ~flags;
		}

		bool CheckFlag(uint32_t flag)
		{
			return (_flags & flag) == flag;
		}

		void UpdateFrame(uint8_t* frameBuffer);
		void DisplayToast(shared_ptr<ToastInfo> toast);

		void TakeScreenshot(string romFilename);
	};
}