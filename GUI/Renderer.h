#include "stdafx.h"
#include "DirectXTK\SpriteBatch.h"
#include "DirectXTK\SpriteFont.h"
#include "../Core/IVideoDevice.h"
#include "../Core/IMessageManager.h"

using namespace DirectX;

namespace NES {
	enum UIFlags
	{
		ShowFPS = 1,
		ShowPauseScreen = 2,
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

		ID3D11SamplerState*		_samplerState = nullptr;
		
		ID3D11Texture2D*			_pTexture = nullptr;
		byte*							_videoRAM = nullptr;

		bool							_frameChanged = true;
		uint8_t*						_nextFrameBuffer = nullptr;

		unique_ptr<SpriteFont>	_font;
		ID3D11Texture2D*			_overlayTexture = nullptr;
		byte*							_overlayBuffer = nullptr;
		
		std::unique_ptr<SpriteBatch> _spriteBatch;
		//ID3D11PixelShader* _pixelShader = nullptr;

		uint32_t _screenWidth;
		uint32_t _screenHeight;
		uint32_t _bytesPerPixel;
		uint32_t _hdScreenWidth;
		uint32_t _hdScreenHeight;
		uint32_t _screenBufferSize;
		uint32_t _hdScreenBufferSize;

		uint32_t _flags = 0;

		list<wstring> _displayMessages;
		list<uint32_t> _displayTimestamps;

		HRESULT InitDevice();
		void CleanupDevice();

		void SetScreenSize(uint32_t screenWidth, uint32_t screenHeight);

		ID3D11ShaderResourceView* GetShaderResourceView(ID3D11Texture2D* texture);
		void DrawNESScreen();
		void DrawPauseScreen();

		void RemoveOldMessages();
		void DrawOutlinedString(wstring message, float x, float y, DirectX::FXMVECTOR color, float scale);

		//HRESULT CompileShader(wstring filename, LPCSTR szEntryPoint, LPCSTR szShaderModel, ID3DBlob** ppBlobOut);

	public:
		Renderer(HWND hWnd);
		~Renderer();

		void Render();

		void DisplayMessage(wstring text);
		
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
			_frameChanged = true;
			memcpy(_nextFrameBuffer, frameBuffer, 256 * 240 * 4);
		}
	};
}