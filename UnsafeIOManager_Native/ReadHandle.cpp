#include "pch.h"
#include "ReadHandle.h"
#include "FileIOCompletionRoutine.h"

ReadHandle::ReadHandle(HANDLE fileHandle, LPVOID buffer, DWORD offset, DWORD length)
	: FileHandle{ fileHandle }
{
	if (FileHandle == INVALID_HANDLE_VALUE || FileHandle == nullptr)
	{
		ErrorCode = -1;
		return;
	}
	overlapped.Offset = offset;
	Result = { ReadFileEx(FileHandle, buffer, length, &overlapped, FileIOCompletionRoutine) };
	ErrorCode = GetLastError();
}

ReadHandle::~ReadHandle()
{
}
