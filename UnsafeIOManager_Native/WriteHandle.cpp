#include "pch.h"
#include "WriteHandle.h"
#include "FileIOCompletionRoutine.h"

WriteHandle::WriteHandle(HANDLE handle, LPCVOID buffer, DWORD offset, DWORD length)
	: FileHandle{ handle }
{
	if (FileHandle == INVALID_HANDLE_VALUE || handle == nullptr)
	{
		ErrorCode = -1;
		return;
	}
	overlapped.Offset = offset;
	Result = { WriteFileEx(FileHandle, buffer, length, &overlapped, FileIOCompletionRoutine) };
	ErrorCode = GetLastError();
}

WriteHandle::~WriteHandle()
{
}