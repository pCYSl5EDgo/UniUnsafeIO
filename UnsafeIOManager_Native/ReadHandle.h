#pragma once

class ReadHandle
{
public:
	ReadHandle(HANDLE fileHandle, LPVOID buffer, DWORD offset, DWORD length);
	~ReadHandle();
	DWORD ErrorCode{ };
	HANDLE FileHandle{ INVALID_HANDLE_VALUE };
	OVERLAPPED overlapped{ };
	BOOL Result{ };
private:
};