#include "stdafx.h"
#include "Renderer.h"
#include "DirectXTK\SpriteBatch.h"
#include "..\Core\PPU.h"

namespace NES 
{
	bool Renderer::Initialize(HINSTANCE hInstance, HWND hWnd) {
		_hInst = hInstance;
		_hWnd = hWnd;
		
		if(FAILED(InitDevice())) {
			CleanupDevice();
			return false;
		} else {
			return true;
		}
	}

	//--------------------------------------------------------------------------------------
	// Create Direct3D device and swap chain
	//--------------------------------------------------------------------------------------
	HRESULT Renderer::InitDevice()
	{
		HRESULT hr = S_OK;

		RECT rc;
		GetClientRect(_hWnd, &rc);
		UINT width = rc.right - rc.left;
		UINT height = rc.bottom - rc.top;

		UINT createDeviceFlags = 0;
#ifdef _DEBUG
		createDeviceFlags |= D3D11_CREATE_DEVICE_DEBUG;
#endif

		D3D_DRIVER_TYPE driverTypes[] =
		{
			D3D_DRIVER_TYPE_HARDWARE,
			D3D_DRIVER_TYPE_WARP,
			D3D_DRIVER_TYPE_REFERENCE,
		};
		UINT numDriverTypes = ARRAYSIZE(driverTypes);

		D3D_FEATURE_LEVEL featureLevels[] =
		{
			D3D_FEATURE_LEVEL_11_1,
			D3D_FEATURE_LEVEL_11_0,
			D3D_FEATURE_LEVEL_10_1,
			D3D_FEATURE_LEVEL_10_0,
		};
		UINT numFeatureLevels = ARRAYSIZE(featureLevels);

		DXGI_SWAP_CHAIN_DESC sd;
		ZeroMemory(&sd, sizeof(sd));
		sd.BufferCount = 1;
		sd.BufferDesc.Width = width;
		sd.BufferDesc.Height = height;
		sd.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
		sd.BufferDesc.RefreshRate.Numerator = 60;
		sd.BufferDesc.RefreshRate.Denominator = 1;
		sd.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
		sd.OutputWindow = _hWnd;
		sd.SampleDesc.Count = 1;
		sd.SampleDesc.Quality = 0;
		sd.Windowed = TRUE;

		for(UINT driverTypeIndex = 0; driverTypeIndex < numDriverTypes; driverTypeIndex++) {
			_driverType = driverTypes[driverTypeIndex];
			hr = D3D11CreateDeviceAndSwapChain(nullptr, _driverType, nullptr, createDeviceFlags, featureLevels, numFeatureLevels,
				D3D11_SDK_VERSION, &sd, &_pSwapChain, &_pd3dDevice, &_featureLevel, &_pImmediateContext);

			if(hr == E_INVALIDARG) {
				// DirectX 11.0 platforms will not recognize D3D_FEATURE_LEVEL_11_1 so we need to retry without it
				hr = D3D11CreateDeviceAndSwapChain(nullptr, _driverType, nullptr, createDeviceFlags, &featureLevels[1], numFeatureLevels - 1,
					D3D11_SDK_VERSION, &sd, &_pSwapChain, &_pd3dDevice, &_featureLevel, &_pImmediateContext);
			}

			if(SUCCEEDED(hr)) {
				break;
			}
		}
		if(FAILED(hr)) {
			return hr;
		}

		// Obtain the Direct3D 11.1 versions if available
		hr = _pd3dDevice->QueryInterface(__uuidof(ID3D11Device1), reinterpret_cast<void**>(&_pd3dDevice1));
		if(SUCCEEDED(hr)) {
			(void)_pImmediateContext->QueryInterface(__uuidof(ID3D11DeviceContext1), reinterpret_cast<void**>(&_pImmediateContext1));
		}

		// Create a render target view
		ID3D11Texture2D* pBackBuffer = nullptr;
		hr = _pSwapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), (LPVOID*)&pBackBuffer);
		if(FAILED(hr)) {
			return hr;
		}

		hr = _pd3dDevice->CreateRenderTargetView(pBackBuffer, nullptr, &_pRenderTargetView);
		pBackBuffer->Release();
		if(FAILED(hr)) {
			return hr;
		}

		_pImmediateContext->OMSetRenderTargets(1, &_pRenderTargetView, nullptr);

		// Setup the viewport
		UINT fred;
		D3D11_VIEWPORT vp;
		vp.Width = (FLOAT)width;
		vp.Height = (FLOAT)height;
		vp.MinDepth = 0.0f;
		vp.MaxDepth = 1.0f;
		vp.TopLeftX = 0;
		vp.TopLeftY = 0;
		_pImmediateContext->RSSetViewports(1, &vp);

		_pd3dDevice->CheckMultisampleQualityLevels(DXGI_FORMAT_R8G8B8A8_UNORM, 16, &fred);

		uint16_t screenwidth = 256;
		uint16_t screenheight = 240;

		D3D11_TEXTURE2D_DESC desc;
		ZeroMemory(&desc, sizeof(D3D11_TEXTURE2D_DESC));
		desc.ArraySize = 1;
		desc.BindFlags = D3D11_BIND_SHADER_RESOURCE;
		desc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
		desc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
		desc.MipLevels = 1;
		desc.MiscFlags = 0;
		desc.SampleDesc.Count = 1;
		desc.SampleDesc.Quality = fred;
		desc.Usage = D3D11_USAGE_DYNAMIC;
		desc.Width = screenwidth;
		desc.Height = screenheight;
		desc.MiscFlags = 0;

		D3D11_RENDER_TARGET_VIEW_DESC renderTargetViewDescription;
		ZeroMemory(&renderTargetViewDescription, sizeof(renderTargetViewDescription));
		renderTargetViewDescription.Format = desc.Format;
		renderTargetViewDescription.ViewDimension = D3D11_RTV_DIMENSION_TEXTURE2D; // MS;

		_videoRAM = new byte[screenwidth*screenheight * 4];
		memset(_videoRAM, 0xFF, screenwidth*screenheight*4);

		D3D11_SUBRESOURCE_DATA tbsd;
		tbsd.pSysMem = (void *)_videoRAM;
		tbsd.SysMemPitch = screenwidth * 4;
		tbsd.SysMemSlicePitch = screenwidth*screenheight * 4; // Not needed since this is a 2d texture

		if(FAILED(_pd3dDevice->CreateTexture2D(&desc, &tbsd, &_pTexture))) {
			return 0;
		}

		////////////////////////////////////////////////////////////////////////////
		_sprites.reset(new SpriteBatch(_pImmediateContext));

		return S_OK;
	}


	//--------------------------------------------------------------------------------------
	// Clean up the objects we've created
	//--------------------------------------------------------------------------------------
	void Renderer::CleanupDevice()
	{
		if(_pImmediateContext) _pImmediateContext->ClearState();

		if(_pVertexBuffer) _pVertexBuffer->Release();
		if(_pVertexLayout) _pVertexLayout->Release();
		if(_pVertexShader) _pVertexShader->Release();
		if(_pPixelShader) _pPixelShader->Release();
		if(_pRenderTargetView) _pRenderTargetView->Release();
		if(_pSwapChain) _pSwapChain->Release();
		if(_pImmediateContext1) _pImmediateContext1->Release();
		if(_pImmediateContext) _pImmediateContext->Release();
		if(_pd3dDevice1) _pd3dDevice1->Release();
		if(_pd3dDevice) _pd3dDevice->Release();
	}

	//--------------------------------------------------------------------------------------
	// Render a frame
	//--------------------------------------------------------------------------------------
	void Renderer::Render()
	{
		// Clear the back buffer 
		//_pImmediateContext->ClearRenderTargetView(_pRenderTargetView, Colors::MidnightBlue);

		UINT screenwidth = 256, screenheight = 240;

		D3D11_MAPPED_SUBRESOURCE dd;
		dd.pData = (void *)_videoRAM;
		dd.RowPitch = screenwidth * 4;
		dd.DepthPitch = screenwidth* screenheight * 4;

		uint8_t *frameData = PPU::GetFrame();
		_pImmediateContext->Map(_pTexture, 0, D3D11_MAP_WRITE_DISCARD, 0, &dd);
		memcpy(dd.pData, frameData, screenwidth*screenheight * 4);
		_pImmediateContext->Unmap(_pTexture, 0);
		delete[] frameData;


		///////////////////////////////////////////////////////////////////////////////
		D3D11_SHADER_RESOURCE_VIEW_DESC srvDesc;
		D3D11_TEXTURE2D_DESC desc;
		D3D11_RESOURCE_DIMENSION type;
		_pTexture->GetType(&type);
		_pTexture->GetDesc(&desc);
		srvDesc.Format = desc.Format;
		srvDesc.ViewDimension = D3D11_SRV_DIMENSION_TEXTURE2D;
		srvDesc.Texture2D.MipLevels = desc.MipLevels;
		srvDesc.Texture2D.MostDetailedMip = desc.MipLevels - 1;

		ID3D11ShaderResourceView *pSRView = NULL;
		_pd3dDevice->CreateShaderResourceView(_pTexture, &srvDesc, &pSRView);

		/*
		D3D11_RENDER_TARGET_VIEW_DESC rtDesc;
		rtDesc.Format = desc.Format;
		rtDesc.ViewDimension = D3D11_RTV_DIMENSION_TEXTURE2D;
		rtDesc.Texture2D.MipSlice = 0;*/

		////////////////////////////////////////////////////////////////////////////

		_sprites->Begin();
		RECT x;
		x.left = 0;
		x.right = 256;
		x.bottom = 240;
		x.top = 0;
		_sprites->Draw(pSRView, x);
		_sprites->End();

		// Present the information rendered to the back buffer to the front buffer (the screen)
		_pSwapChain->Present(0, 0);
	}
}