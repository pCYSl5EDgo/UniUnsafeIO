using System;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;

namespace UniUnsafeIO.Unsafe.LowLevel
{
    public struct FileHandle : IDisposable
    {
        public bool Equals(FileHandle other)
        {
            return Handle == other.Handle;
        }

        public override bool Equals(object obj)
        {
            return obj is FileHandle other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        [NativeDisableUnsafePtrRestriction] public IntPtr Handle;
#endif

        public FileHandle(string path, FileAccess access, bool isNoBuffering)
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            Handle = Wrapper.GetFileHandle(path, access, isNoBuffering ? 1 : 0);
#endif
        }

        public FileHandle(IntPtr fileHandle)
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            Handle = fileHandle;
#endif
        }

        public long Length
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            get => Wrapper.GetFileLength(Handle);
            set => Wrapper.SetFileLength(Handle, value);
#else
            get => throw new InvalidOperationException();
            set => throw new InvalidOperationException();
#endif
        }

        public void Dispose()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            if (Handle == IntPtr.Zero || Handle == new IntPtr(-1L)) return;
            Wrapper.CloseHandle(Handle);
            Handle = IntPtr.Zero;
#else
#endif
        }

        public static bool operator ==(FileHandle left, FileHandle right)
        {
            return left.Handle == right.Handle;
        }

        public static bool operator !=(FileHandle left, FileHandle right)
        {
            return left.Handle != right.Handle;
        }
    }
}