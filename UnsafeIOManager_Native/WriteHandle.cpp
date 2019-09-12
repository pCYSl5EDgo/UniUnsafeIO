#include "pch.h"
#include "WriteHandle.h"
#include "FileIOCompletionRoutine.h"

WriteHandle::WriteHandle(HANDLE handle, LPCVOID buffer, DWORD offset, DWORD length)
	: FileHandle{ handle }, overlapped{ nullptr }
{
	if (FileHandle == INVALID_HANDLE_VALUE || handle == nullptr || buffer == nullptr)
	{
		ErrorCode = -1;
		return;
	}
	overlapped = new OVERLAPPED;
	*overlapped = {};
	
	overlapped->Offset = 0xffffffff;
	overlapped->OffsetHigh = 0xffffffff;
	Result = { WriteFileEx(handle, buffer, length, overlapped, FileIOCompletionRoutine) };
	ErrorCode = GetLastError();
}

WriteHandle::~WriteHandle()
{
	if (overlapped != nullptr) {
		delete overlapped;
	}
}