#include "pch.h"
#include "ReadHandle.h"
#include "FileIOCompletionRoutine.h"

ReadHandle::ReadHandle(HANDLE fileHandle, LPVOID buffer, DWORD offset, DWORD length)
	: FileHandle{ fileHandle }, overlapped{ nullptr }
{
	if (FileHandle == INVALID_HANDLE_VALUE || FileHandle == nullptr || buffer == nullptr)
	{
		ErrorCode = -1;
		return;
	}
	overlapped = new OVERLAPPED;
	*overlapped = {};
	overlapped->Offset = offset;
	Result = { ReadFileEx(FileHandle, buffer, length, overlapped, FileIOCompletionRoutine) };
	ErrorCode = GetLastError();
}

ReadHandle::~ReadHandle()
{
	if (overlapped != nullptr) {
		delete overlapped;
	}
}
