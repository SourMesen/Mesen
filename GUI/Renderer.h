#include "stdafx.h"
#include "DirectXTK\SpriteBatch.h"
#include "DirectXTK\SpriteFont.h"
#include "../Core/IVideoDevice.h"

using namespace DirectX;

namespace NES {
	enum UIFlags
	{
		ShowFPS = 1,
		ShowPauseScreen = 2,
	};

	class Renderer : IVideoDevice
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

		ID3D11SamplerState*		_samplerState = nullptr;
		
		ID3D11Texture2D*			_pTexture = nullptr;
		byte*							_videoRAM;
		uint8_t*						_nextFrameBuffer;

		unique_ptr<SpriteFont>	_font;
		ID3D11Texture2D*			_overlayTexture = nullptr;
		byte*							_overlayBuffer;
		std::unique_ptr<SpriteBatch> _spriteBatch;

		uint32_t _flags = 0;

		wstring _displayMessage = L"";
		uint32_t _displayTimestamp = 0;

		HRESULT InitDevice();
		void CleanupDevice();

		ID3D11ShaderResourceView* GetShaderResourceView(ID3D11Texture2D* texture);
		void DrawNESScreen();
		void DrawPauseScreen();

	public:
		Renderer(HWND hWnd);
		~Renderer();

		void Render();

		void Renderer::DisplayMessage(wstring text, uint32_t duration);
		
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

		void UpdateFrame(uint8_t* frameBuffer)
		{
			memcpy(_nextFrameBuffer, frameBuffer, 256 * 240 * 4);
		}
	};
}