using System;
using System.IO;
using UniUnsafeIO.Unsafe.LowLevel;

namespace UniUnsafeIO
{
    public static class UnsafeIOManager
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        public static unsafe IOHandle Write(string path, void* buffer, uint offset, uint length)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (length == 0) return default;
            var handle = new FileHandle(path, FileAccess.Write, true);
            return new IOHandle(handle, Wrapper.CreateWriteHandle(handle.Handle, new IntPtr(buffer), offset, length), true);
        }
        public static unsafe IOHandle Read(string path, void* buffer, uint offset, uint length)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (length == 0) return default;
            var handle = new FileHandle(path, FileAccess.Read, true);
            return new IOHandle(handle, Wrapper.CreateReadHandle(handle.Handle, new IntPtr(buffer), offset, length), true);
        }
#else
#endif
    }
}