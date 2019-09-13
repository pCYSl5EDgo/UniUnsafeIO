#pragma once
#include "IOHandle.h"

class WriteHandle : public IOHandle
{
public:
	WriteHandle(HANDLE handle, LPCVOID buffer, DWORD offset, DWORD offsetHigh, DWORD length);
	~WriteHandle();
private:
};
