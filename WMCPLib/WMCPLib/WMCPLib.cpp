#include "pch.h"
#include <WinUser.h>

#include "WMCPLib.h"


namespace WMCP {
	int test(int test) {
		return test + 1;
	}

	UINT pressKey(WORD Key) {
		INPUT inp;
		inp.type = INPUT_KEYBOARD;
		inp.ki.wScan = 0;
		inp.ki.time = 0;
		inp.ki.dwExtraInfo = 0;

		// Press "Play/Pause" key
		inp.ki.wVk = Key;
		inp.ki.dwFlags = 0; // zero for key press

		SendInput(1, &inp, sizeof(INPUT));

		// Release "Play/Pause" key
		inp.ki.dwFlags = KEYEVENTF_KEYUP;
		SendInput(1, &inp, sizeof(INPUT));

		return 0; // Exit normally
	}

	UINT pressPlayPauseKey() {
		return pressKey(VK_MEDIA_PLAY_PAUSE);
	}

	UINT pressNextKey() {
		return pressKey(VK_MEDIA_NEXT_TRACK);
	}
	UINT pressPrevKey() {
		return pressKey(VK_MEDIA_PREV_TRACK);
	}


} //WMCP