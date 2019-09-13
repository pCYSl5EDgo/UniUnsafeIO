#pragma once
#include "IOHandle.h"

class ReadHandle : public IOHandle
{
public:
	ReadHandle(HANDLE fileHandle, LPVOID buffer, DWORD offset, DWORD offsetHigh, DWORD length);
	~ReadHandle();
private:
};