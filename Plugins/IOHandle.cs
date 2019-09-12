using System;
using Unity.Collections.LowLevel.Unsafe;
using UniUnsafeIO.Unsafe.LowLevel;

namespace UniUnsafeIO
{
    public struct IOHandle : IDisposable
    {
        [NativeDisableUnsafePtrRestriction] private IntPtr manager;
        [NativeDisableUnsafePtrRestriction] private FileHandle fileHandle;
        private readonly int isFileHandleOwner;

        public IOHandle(FileHandle fileHandle, IntPtr manager, bool isFileHandleOwner)
        {
            this.fileHandle = fileHandle;
            this.manager = manager;
            this.isFileHandleOwner = isFileHandleOwner ? 1 : 0;
        }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        public bool IsCompleted => manager == IntPtr.Zero || Wrapper.WaitForComplete(fileHandle.Handle, out var error, 0) == 0;

        public void Complete()
        {
            if (manager == IntPtr.Zero)
            {
                return;
            }
            Wrapper.WaitForComplete(fileHandle.Handle, out _, uint.MaxValue);
        }

        public void Dispose()
        {
            if (manager == IntPtr.Zero) return;
            Wrapper.Dispose(manager);
            manager = IntPtr.Zero;
            if (fileHandle == default || isFileHandleOwner == 0) return;
            Wrapper.CloseHandle(fileHandle.Handle);
            fileHandle = default;
        }
#else
        public bool IsCompleted => throw new InvalidOperationException();
        public void Dispose()
        {
            throw new InvalidOperationException();
        }
        public void Complete()
        {
            throw new InvalidOperationException();
        }
#endif
    }
}