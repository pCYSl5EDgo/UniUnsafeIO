#pragma once
#include "pch.h"

/* dwError: �����R�[�h */
/* dwTransferred: �]������o�C�g�� */
/* lpo: ���o�͏������\���̂̃A�h���X */
void WINAPI FileIOCompletionRoutineWrite(DWORD dwError, DWORD dwTransferred, LPOVERLAPPED lpo);