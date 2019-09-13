#pragma once
#include "pch.h"

/* dwError: 完了コード */
/* dwTransferred: 転送するバイト数 */
/* lpo: 入出力情報を持つ構造体のアドレス */
void WINAPI FileIOCompletionRoutineWrite(DWORD dwError, DWORD dwTransferred, LPOVERLAPPED lpo);