#include "stdafx.h"
#include "DirectXTK\SpriteBatch.h"

using namespace DirectX;

namespace NES {
	struct SimpleVertex
	{
		XMFLOAT3 Pos;
	};

	class Renderer
	{
	private:
		HINSTANCE               _hInst = nullptr;
		HWND                    _hWnd = nullptr;

		D3D_DRIVER_TYPE         _driverType = D3D_DRIVER_TYPE_NULL;
		D3D_FEATURE_LEVEL       _featureLevel = D3D_FEATURE_LEVEL_11_0;
		ID3D11Device*           _pd3dDevice = nullptr;
		ID3D11Device1*          _pd3dDevice1 = nullptr;
		ID3D11DeviceContext*    _pImmediateContext = nullptr;
		ID3D11DeviceContext1*   _pImmediateContext1 = nullptr;
		IDXGISwapChain*         _pSwapChain = nullptr;
		ID3D11RenderTargetView* _pRenderTargetView = nullptr;
		ID3D11VertexShader*     _pVertexShader = nullptr;
		ID3D11PixelShader*      _pPixelShader = nullptr;
		ID3D11InputLayout*      _pVertexLayout = nullptr;
		ID3D11Buffer*           _pVertexBuffer = nullptr;
		ID3D11Texture2D*			_pTexture = nullptr;

		byte*							_videoRAM;
		//SpriteBatch*				_sprites;
		std::unique_ptr<SpriteBatch> _sprites;

		HRESULT Renderer::CompileShaderFromFile(WCHAR* szFileName, LPCSTR szEntryPoint, LPCSTR szShaderModel, ID3DBlob** ppBlobOut);
		HRESULT Renderer::InitDevice();
		void Renderer::CleanupDevice();

	public:
		bool Initialize(HINSTANCE hInst, HWND hWnd);
		void Render();
	};
}