#pragma once

//#ifndef WMCPLIB_H
//#define WMCPLIB_H


#ifdef WMCPLIB_EXPORTS
#define WMCPLIB_API __declspec(dllexport)
#else
#define WMCPLIB_API __declspec(dllimport)
#endif

namespace WMCP {
	extern "C" WMCPLIB_API int test(int test);

	extern "C" WMCPLIB_API UINT pressPlayPauseKey();
	extern "C" WMCPLIB_API UINT pressNextKey();
	extern "C" WMCPLIB_API UINT pressPrevKey();
} //WMCP




//#endif