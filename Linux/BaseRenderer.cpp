#include "BaseRenderer.h"
#include <cmath>
#include "../Core/EmulationSettings.h"
#include "../Core/VideoDecoder.h"

void BaseRenderer::DisplayMessage(string title, string message)
{
	shared_ptr<ToastInfo> toast(new ToastInfo(title, message, 4000, ""));
	_toasts.push_front(toast);
}

void BaseRenderer::RemoveOldToasts()
{
	_toasts.remove_if([](shared_ptr<ToastInfo> toast) { return toast->IsToastExpired(); });
}

void BaseRenderer::DrawToasts()
{
	RemoveOldToasts();

	int counter = 0;
	int lastHeight = 5;
	for(shared_ptr<ToastInfo> toast : _toasts) {
		if(counter < 6) {
			DrawToast(toast, lastHeight);
		} else {
			break;
		}
		counter++;
	}
}

std::wstring BaseRenderer::WrapText(string utf8Text, float maxLineWidth, uint32_t &lineCount)
{
	using std::wstring;
	wstring text = utf8::utf8::decode(utf8Text);
	wstring wrappedText;
	list<wstring> words;
	wstring currentWord;
	for(size_t i = 0, len = text.length(); i < len; i++) {
		if(text[i] == L' ' || text[i] == L'\n') {
			if(currentWord.length() > 0) {
				words.push_back(currentWord);
				currentWord.clear();
			}
		} else {
			currentWord += text[i];
		}
	}
	if(currentWord.length() > 0) {
		words.push_back(currentWord);
	}

	lineCount = 1;
	float spaceWidth = MeasureString(L" ");

	float lineWidth = 0.0f;
	for(wstring word : words) {
		for(unsigned int i = 0; i < word.size(); i++) {
			if(!ContainsCharacter(word[i])) {
				word[i] = L'?';
			}
		}

		float wordWidth = MeasureString(word.c_str());

		if(lineWidth + wordWidth < maxLineWidth) {
			wrappedText += word + L" ";
			lineWidth += wordWidth + spaceWidth;
		} else {
			wrappedText += L"\n" + word + L" ";
			lineWidth = wordWidth + spaceWidth;
			lineCount++;
		}
	}

	return wrappedText;
}

void BaseRenderer::DrawToast(shared_ptr<ToastInfo> toast, int &lastHeight)
{
	//Get opacity for fade in/out effect
	uint8_t opacity = (uint8_t)(toast->GetOpacity()*255);
	int textLeftMargin = 4;

	int lineHeight = 25;
	string text = "[" + toast->GetToastTitle() + "] " + toast->GetToastMessage();
	uint32_t lineCount = 0;
	std::wstring wrappedText = WrapText(text, _screenWidth - textLeftMargin * 2 - 20, lineCount);
	lastHeight += lineCount * lineHeight;
	DrawString(wrappedText, textLeftMargin, (float)(_screenHeight - lastHeight), opacity, opacity, opacity);
}

void BaseRenderer::DrawString(std::string message, int x, int y, uint8_t r, uint8_t g, uint8_t b)
{
	std::wstring textStr = utf8::utf8::decode(message);
	DrawString(textStr, x, y, r, g, b);
}

void BaseRenderer::ShowFpsCounter()
{
	double elapsedSeconds = _fpsTimer.GetElapsedMS() / 1000;
	if(elapsedSeconds > 1.0) {
		//Update fps every sec
		uint32_t frameCount = VideoDecoder::GetInstance()->GetFrameCount();
		if(frameCount - _lastFrameCount < 0) {
			_currentFPS = 0;
		} else {
			_currentFPS = (int)(std::round((double)(frameCount - _lastFrameCount) / elapsedSeconds));
			_currentRenderedFPS = (int)(std::round((double)(_renderedFrameCount - _lastRenderedFrameCount) / elapsedSeconds));
		}
		_lastFrameCount = frameCount;
		_lastRenderedFrameCount = _renderedFrameCount;
		_fpsTimer.Reset();
	}

	if(_currentFPS > 5000) {
		_currentFPS = 0;
	}
	if(_currentRenderedFPS > 5000) {
		_currentRenderedFPS = 0;
	}

	string fpsString = string("FPS: ") + std::to_string(_currentFPS) + " / " + std::to_string(_currentRenderedFPS);
	DrawString(fpsString, (float)(_screenWidth - 120), 13, 250, 235, 215);
}

void BaseRenderer::ShowLagCounter()
{
	float yPos = EmulationSettings::CheckFlag(EmulationFlags::ShowFPS) ? 37.0f : 13.0f;
	string lagCounter = MessageManager::Localize("Lag") + ": " + std::to_string(Console::GetLagCounter());
	DrawString(lagCounter, (float)(_screenWidth - 120), yPos, 250, 235, 215);
}

bool BaseRenderer::IsMessageShown()
{
	return !_toasts.empty();
}	
