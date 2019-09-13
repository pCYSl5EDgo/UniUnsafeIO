#pragma once

class IOHandle
{
public:
	DWORD ErrorCode{ };
	HANDLE FileHandle{ INVALID_HANDLE_VALUE };
	OVERLAPPED overlapped{ };
	BOOL Result{ };
};