#include "pch.h"
#include "ReadHandle.h"

void WINAPI FileIOCompletionRoutineRead(DWORD dwError, DWORD dwTransferred, LPOVERLAPPED lpo)
{
	lpo->hEvent = nullptr;
}

ReadHandle::ReadHandle(HANDLE fileHandle, LPVOID buffer, DWORD offset, DWORD offsetHigh, DWORD length)
{
	FileHandle = fileHandle;
	if (FileHandle == INVALID_HANDLE_VALUE || FileHandle == nullptr || buffer == nullptr)
	{
		ErrorCode = -1;
		return;
	}
	overlapped.Offset = offset;
	overlapped.OffsetHigh = offsetHigh;
	overlapped.hEvent = this;
	Result = { ReadFileEx(FileHandle, buffer, length, &overlapped, FileIOCompletionRoutineRead) };
	ErrorCode = GetLastError();
}

ReadHandle::~ReadHandle()
{

}
