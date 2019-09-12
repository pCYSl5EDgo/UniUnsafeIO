using System;
using Unity.Collections.LowLevel.Unsafe;
using UniUnsafeIO.Unsafe.LowLevel;

namespace UniUnsafeIO
{
    public struct IOHandle : IDisposable
    {
        [NativeDisableUnsafePtrRestriction] public IntPtr Manager;
        [NativeDisableUnsafePtrRestriction] public FileHandle FileHandle;
        private readonly int isFileHandleOwner;

        public IOHandle(FileHandle fileHandle, IntPtr manager, bool isFileHandleOwner)
        {
            this.FileHandle = fileHandle;
            this.Manager = manager;
            this.isFileHandleOwner = isFileHandleOwner ? 1 : 0;
        }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        public bool IsCompleted => Manager == IntPtr.Zero || Wrapper.WaitForComplete(FileHandle.Handle, out var error, 0) == 0;

        public void Complete()
        {
            if (Manager == IntPtr.Zero)
            {
                return;
            }
            Wrapper.WaitForComplete(FileHandle.Handle, out _, uint.MaxValue);
        }

        public void Dispose()
        {
            if (Manager == IntPtr.Zero) return;
            Wrapper.Dispose(Manager);
            Manager = IntPtr.Zero;
            if (FileHandle == default || isFileHandleOwner == 0) return;
            Wrapper.CloseHandle(FileHandle.Handle);
            FileHandle = default;
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