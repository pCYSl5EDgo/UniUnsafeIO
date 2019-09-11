using System;
using Unity.Collections.LowLevel.Unsafe;
using UniUnsafeIO.Unsafe.LowLevel;

namespace UniUnsafeIO
{
    public struct IOHandle : IDisposable
    {
        [NativeDisableUnsafePtrRestriction] private IntPtr manager;
        [NativeDisableUnsafePtrRestriction] private FileHandle fileHandle;

        public IOHandle(IntPtr manager)
        {
            fileHandle = default;
            this.manager = manager;
        }

        public IOHandle(FileHandle fileHandle, IntPtr manager)
        {
            this.fileHandle = fileHandle;
            this.manager = manager;
        }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        public bool IsCompleted => manager == IntPtr.Zero || Wrapper.WaitForComplete(manager, out var error, 0) == 0;

        public void Complete()
        {
            if (manager == IntPtr.Zero)
            {
                return;
            }
            Wrapper.WaitForComplete(manager, out _, uint.MaxValue);
        }

        public void Dispose()
        {
            if (manager == IntPtr.Zero) return;
            Wrapper.Dispose(manager);
            manager = IntPtr.Zero;
            if (fileHandle == default) return;
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