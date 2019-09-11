#pragma once

class WriteHandle
{
public:
	WriteHandle(HANDLE handle, LPCVOID buffer, DWORD offset, DWORD length);
	~WriteHandle();
	DWORD ErrorCode{ };
	HANDLE FileHandle{ INVALID_HANDLE_VALUE };
	OVERLAPPED overlapped{ };
	BOOL Result{ };
private:
};
