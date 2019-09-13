#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace UniUnsafeIO.Unsafe.LowLevel
{
    public static class Wrapper
    {
        [DllImport("kernel32.dll")]
        public static extern void CloseHandle(IntPtr fileHandle);

        private const string AsyncWriteManagerNativeDllName = "AsyncWriteManager";

        [DllImport(AsyncWriteManagerNativeDllName, CharSet = CharSet.Unicode)] public static extern IntPtr GetFileHandle(string path, FileAccess access);
        [DllImport(AsyncWriteManagerNativeDllName, CharSet = CharSet.Unicode)] public static extern IntPtr GetFileHandle(IntPtr bStrUtf16Buffer, FileAccess access);

        [DllImport(AsyncWriteManagerNativeDllName)] public static extern void SetFileLength(IntPtr fileHandle, long length);

        [DllImport(AsyncWriteManagerNativeDllName)] public static extern long GetFileLength(IntPtr fileHandle);

        [DllImport(AsyncWriteManagerNativeDllName)] public static extern IntPtr CreateWriteHandle(IntPtr fileHandle, IntPtr buffer, uint offset, uint offsetHigh, uint length);
        [DllImport(AsyncWriteManagerNativeDllName)] public static extern IntPtr CreateReadHandle(IntPtr fileHandle, IntPtr buffer, uint offset, uint offsetHigh, uint length);

        [DllImport(AsyncWriteManagerNativeDllName)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool IsCompleted(IntPtr manager);

        [DllImport(AsyncWriteManagerNativeDllName)] [return: MarshalAs(UnmanagedType.U1)] public static extern bool GetResult(IntPtr manager);

        [DllImport(AsyncWriteManagerNativeDllName)] public static extern uint GetError(IntPtr manager);

        [DllImport(AsyncWriteManagerNativeDllName)] public static extern uint WaitForComplete(IntPtr manager, [Out]out uint error, uint timeout);

        [DllImport(AsyncWriteManagerNativeDllName)] public static extern void Dispose(IntPtr something);
    }
}
#endif