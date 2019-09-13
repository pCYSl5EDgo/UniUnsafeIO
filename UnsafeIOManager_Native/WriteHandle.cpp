#include "pch.h"
#include "WriteHandle.h"

void WINAPI FileIOCompletionRoutineWrite(DWORD dwError, DWORD dwTransferred, LPOVERLAPPED lpo)
{
	lpo->hEvent = nullptr;
}

WriteHandle::WriteHandle(HANDLE fileHandle, LPCVOID buffer, DWORD offset, DWORD offsetHigh, DWORD length)
{
	FileHandle = fileHandle;
	if (FileHandle == INVALID_HANDLE_VALUE || fileHandle == nullptr || buffer == nullptr)
	{
		ErrorCode = -1;
		return;
	}
	overlapped.Offset = offset;
	overlapped.OffsetHigh = offsetHigh;
	overlapped.hEvent = this;
	Result = { WriteFileEx(fileHandle, buffer, length, &overlapped, FileIOCompletionRoutineWrite) };
	ErrorCode = GetLastError();
}

WriteHandle::~WriteHandle()
{
}