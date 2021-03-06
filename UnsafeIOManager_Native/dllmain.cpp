﻿// dllmain.cpp : DLL アプリケーションのエントリ ポイントを定義します。
#include "pch.h"
#include "WriteHandle.h"
#include "ReadHandle.h"

using IOHandlePointer = IOHandle*;
using WriteHandlePointer = WriteHandle*;
using ReadHandlePointer = ReadHandle*;

BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

extern "C"
{
	void UNITY_INTERFACE_EXPORT SetFileLength(HANDLE fileHandle, LONGLONG endOffset)
	{
		SetFilePointerEx(fileHandle, *reinterpret_cast<LARGE_INTEGER*>(&endOffset), nullptr, FILE_BEGIN);
		SetEndOfFile(fileHandle);
	}
	int64_t UNITY_INTERFACE_EXPORT GetFileLength(HANDLE handle)
	{
		LARGE_INTEGER integer;
		if (GetFileSizeEx(handle, &integer))
		{
			return integer.QuadPart;
		}
		return 0;
	}

	HANDLE UNITY_INTERFACE_EXPORT  GetFileHandle(LPCWSTR str, DWORD access)
	{
		DWORD fileAccess{};
		switch (access)
		{
		case 1:
			fileAccess = GENERIC_READ;
			break;
		case 2:
			fileAccess = GENERIC_WRITE;
			break;
		default:
			fileAccess = GENERIC_READ | GENERIC_WRITE;
			break;
		}
		return CreateFile(str, access, FILE_SHARE_READ | FILE_SHARE_WRITE, nullptr, OPEN_ALWAYS, FILE_FLAG_OVERLAPPED, nullptr);
	}

	ReadHandlePointer UNITY_INTERFACE_EXPORT CreateReadHandle(HANDLE fileHandle, LPVOID buffer, DWORD offset, DWORD offsetHigh, DWORD length)
	{
		return new ReadHandle{ fileHandle, buffer, offset, offsetHigh, length };
	}

	WriteHandlePointer UNITY_INTERFACE_EXPORT CreateWriteHandle(HANDLE fileHandle, LPCVOID buffer, DWORD offset, DWORD offsetHigh, DWORD length)
	{
		return new WriteHandle{ fileHandle, buffer, offset, offsetHigh, length };
	}

	uint32_t UNITY_INTERFACE_EXPORT WaitForComplete(HANDLE fileHandle, DWORD* error, DWORD timeout)
	{
		const auto answer{ WaitForSingleObjectEx(fileHandle, timeout, TRUE) };
		*error = GetLastError();
		return answer;
	}

	bool UNITY_INTERFACE_EXPORT IsCompleted(IOHandlePointer manager)
	{
		return manager->overlapped.hEvent == nullptr;
	}

	bool UNITY_INTERFACE_EXPORT GetResult(IOHandlePointer manager)
	{
		return manager->Result;
	}

	uint32_t UNITY_INTERFACE_EXPORT GetError(IOHandlePointer manager)
	{
		return manager->ErrorCode;
	}

	void UNITY_INTERFACE_EXPORT Dispose(void* manager)
	{
		delete manager;
	}
}