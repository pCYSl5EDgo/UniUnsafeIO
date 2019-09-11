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

        [DllImport(AsyncWriteManagerNativeDllName, CharSet = CharSet.Unicode, EntryPoint = "GetFileHandle")] public static extern IntPtr GetFileHandle(string path, FileAccess access, int isNoBuffering);
        [DllImport(AsyncWriteManagerNativeDllName, CharSet = CharSet.Unicode, EntryPoint = "GetFileHandle")] public static extern IntPtr GetFileHandle(IntPtr bStrUtf16Buffer, FileAccess access, int isNoBuffering);

        [DllImport(AsyncWriteManagerNativeDllName)] public static extern void SetFileLength(IntPtr fileHandle, long length);

        [DllImport(AsyncWriteManagerNativeDllName)] public static extern long GetFileLength(IntPtr fileHandle);

        [DllImport(AsyncWriteManagerNativeDllName)] public static extern IntPtr CreateWriteHandle(IntPtr fileHandle, IntPtr buffer, uint offset, uint length);
        [DllImport(AsyncWriteManagerNativeDllName)] public static extern IntPtr CreateReadHandle(IntPtr fileHandle, IntPtr buffer, uint offset, uint length);

        [DllImport(AsyncWriteManagerNativeDllName)] public static extern uint WaitForComplete(IntPtr manager, [Out]out uint error, uint timeout);

        [DllImport(AsyncWriteManagerNativeDllName)] public static extern void Dispose(IntPtr something);
    }
}
#endif