using System;
using System.IO;
using UniUnsafeIO;
using NUnit.Framework;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Random = System.Random;

namespace Tests
{
    public unsafe class IOTest
    {
        struct TempFile : IDisposable
        {
            private string path;

            public static TempFile Create()
            {
                var p = new Random().Next() + ".txt";
                return new TempFile
                {
                    path = p
                };
            }

            public void Dispose()
            {
                File.Delete(path);
            }

            public override string ToString() => path;
        }

        const string inputString = "これが入力値だ！！！！";
        [Test]
        public void WriteTestNullFileName()
        {
            using (var array = new NativeArray<char>(inputString.ToCharArray(), Allocator.TempJob))
            {
                Assert.Throws<ArgumentNullException>(() => UnsafeIOManager.Write(null, array.GetUnsafePtr(), 0, (uint)inputString.Length * 2U));
            }
        }

        [Test]
        public void WriteTestNullBuffer()
        {
            using (var file = TempFile.Create())
            {
                Assert.Throws<ArgumentNullException>(() => UnsafeIOManager.Write(file.ToString(), null, 0, (uint)inputString.Length * 2U));
            }
        }

        [Test]
        public void WriteTestZeroLength()
        {
            using (var file = TempFile.Create())
            using (var array = new NativeArray<char>(inputString.ToCharArray(), Allocator.TempJob))
            {
                Assert.AreEqual(default(IOHandle), UnsafeIOManager.Write(file.ToString(), array.GetUnsafePtr(), 0, 0));
            }
        }

        [Test]
        public void WriteReadSyncTest()
        {
            using (var file = TempFile.Create())
            using (var array = new NativeArray<char>(inputString.ToCharArray(), Allocator.TempJob))
            {
                using (var handle = UnsafeIOManager.Write(file.ToString(), array.GetUnsafePtr(), 0, (uint)(array.Length << 1)))
                {
                    handle.Complete();
                }
                using (var handle = UnsafeIOManager.Read(file.ToString(), array.GetUnsafePtr(), 0, (uint)(array.Length << 1)))
                {
                    handle.Complete();
                }
                Assert.AreEqual(inputString, new string(array.ToArray()));
            }
        }

        [Test]
        public void ReadTestNullBuffer()
        {
            using (var file = TempFile.Create())
            {
                Assert.Throws<ArgumentNullException>(() => UnsafeIOManager.Read(file.ToString(), null, 0, (uint)inputString.Length * 2U));
            }
        }

        [Test]
        public void ReadTestNullFileName()
        {
            using (var array = new NativeArray<char>(inputString.ToCharArray(), Allocator.TempJob))
            {
                Assert.Throws<ArgumentNullException>(() => UnsafeIOManager.Read(null, array.GetUnsafePtr(), 0, (uint)inputString.Length * 2U));
            }
        }

        [Test]
        public void ReadTestZeroLength()
        {
            using (var file = TempFile.Create())
            using (var array = new NativeArray<char>(inputString.ToCharArray(), Allocator.TempJob))
            {
                Assert.AreEqual(default(IOHandle), UnsafeIOManager.Read(file.ToString(), array.GetUnsafePtr(), 0, 0));
            }
        }
    }
}
