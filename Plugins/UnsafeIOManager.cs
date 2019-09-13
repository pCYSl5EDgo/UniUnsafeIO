using System;
using System.IO;
using System.Runtime.CompilerServices;
using UniUnsafeIO.Unsafe.LowLevel;

namespace UniUnsafeIO
{
    // ReSharper disable once InconsistentNaming
    public static unsafe class UnsafeIOManager
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IOHandle Write(string path, void* buffer, uint offset, uint length) => Write(path, buffer, offset, length, out _, out _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IOHandle Write(string path, void* buffer, ulong offset, uint length) => Write(path, buffer, offset, length, out _, out _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IOHandle Write(string path, void* buffer, uint offset, uint length, out bool result, out uint error)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (length == 0)
            {
                result = false;
                error = default;
                return default;
            }
            return Write(path, buffer, offset, 0, length, out result, out error);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IOHandle Write(string path, void* buffer, ulong offset, uint length, out bool result, out uint error)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (length == 0)
            {
                result = false;
                error = default;
                return default;
            }
            return Write(path, buffer, (uint)offset, (uint)(offset >> 32), length, out result, out error);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IOHandle Write(string path, void* buffer, uint offset, uint offsetHigh, uint length, out bool result, out uint error)
        {
            var handle = new FileHandle(path, FileAccess.Write);
            var writeHandle = Wrapper.CreateWriteHandle(handle.Handle, new IntPtr(buffer), offset, offsetHigh, length);
            var ioHandle = new IOHandle(handle, writeHandle, true);
            result = Wrapper.GetResult(writeHandle);
            error = Wrapper.GetError(writeHandle);
            return ioHandle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IOHandle Read(string path, void* buffer, uint offset, uint length) => Read(path, buffer, offset, length, out _, out _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IOHandle Read(string path, void* buffer, ulong offset, uint length) => Read(path, buffer, (uint)offset, (uint)(offset >> 32), length, out _, out _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IOHandle Read(string path, void* buffer, ulong offset, uint length, out bool result, out uint error)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (length == 0)
            {
                result = false;
                error = default;
                return default;
            }
            return Read(path, buffer, (uint)offset, (uint)(offset >> 32), length, out result, out error);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IOHandle Read(string path, void* buffer, uint offset, uint length, out bool result, out uint error)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (length == 0)
            {
                result = false;
                error = default;
                return default;
            }
            return Read(path, buffer, offset, 0, length, out result, out error);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IOHandle Read(string path, void* buffer, uint offset, uint offsetHigh, uint length, out bool result, out uint error)
        {
            var handle = new FileHandle(path, FileAccess.Read);
            var readHandle = Wrapper.CreateReadHandle(handle.Handle, new IntPtr(buffer), offset, offsetHigh, length);
            var ioHandle = new IOHandle(handle, readHandle, true);
            result = Wrapper.GetResult(readHandle);
            error = Wrapper.GetError(readHandle);
            return ioHandle;
        }
#else
#endif
    }
}