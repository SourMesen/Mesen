#include "stdafx.h"
#include "DirectXTK\SpriteBatch.h"
#include "DirectXTK\SpriteFont.h"
#include "../Core/IVideoDevice.h"

using namespace DirectX;

namespace NES {
	class Renderer : IVideoDevice
	{
	private:
		HWND                    _hWnd = nullptr;

		D3D_DRIVER_TYPE         _driverType = D3D_DRIVER_TYPE_NULL;
		D3D_FEATURE_LEVEL       _featureLevel = D3D_FEATURE_LEVEL_11_0;
		ID3D11Device*           _pd3dDevice = nullptr;
		ID3D11Device1*          _pd3dDevice1 = nullptr;
		ID3D11DeviceContext*    _pImmediateContext = nullptr;
		ID3D11DeviceContext1*   _pImmediateContext1 = nullptr;
		IDXGISwapChain*         _pSwapChain = nullptr;
		ID3D11RenderTargetView* _pRenderTargetView = nullptr;

		ID3D11SamplerState*		_samplerState = nullptr;
		
		ID3D11Texture2D*			_pTexture = nullptr;
		byte*							_videoRAM;
		uint8_t*						_nextFrameBuffer;

		unique_ptr<SpriteFont>	_font;
		ID3D11Texture2D*			_overlayTexture = nullptr;
		byte*							_overlayBuffer;
		std::unique_ptr<SpriteBatch> _overlaySpriteBatch;

		std::unique_ptr<SpriteBatch> _sprites;

		HRESULT InitDevice();
		void CleanupDevice();
		ID3D11ShaderResourceView* GetDisplayBufferShaderResourceView();

	public:
		Renderer(HWND hWnd);
		~Renderer();

		void Render();

		void UpdateFrame(uint8_t* frameBuffer)
		{
			memcpy(_nextFrameBuffer, frameBuffer, 256 * 240 * 4);
		}
	};
}