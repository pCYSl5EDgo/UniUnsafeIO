# UniUnsafeIO for Windows

This library is an Unity Native Plugin for windows environment.
You can use NativeArray&lt;T&gt; IO in Unity.
You don't have to use byte[] any more!

# Requirements

Unity2018.4, 2019.1~

Supported OS : Windows(x86 and x64)

# LICENSE

Distributed under MIT License.

# API

static class UniUnsafeIO.UnsafeIOManager

 - IOHandle Write(string path, byte* buffer, long length, long offset, bool isSettingEnd = true)

struct IOHandle : IDisposable

 - bool IsCompleted { get; }
 - void Complete()
 - void Dispose()

# Example

You can wait the completion of IO.

```csharp
NativeArray<byte> bytesWrite;

// Some codes;

using(IOHandle handle = UniUnsafeIO.UnsafeIOManager.Write(@"a.txt", bytesWrite.GetUnsafePtr(), offset: 0, length: bytesWrite.Length))
{    
    // Some work;
    handle.Complete(); // Wait for Completion;
}
```

Or you can poll the IO operation.

```csharp
NativeArray<byte> bytesWrite;

// Some codes;

using(IOHandle handle = UniUnsafeIO.UnsafeIOManager.Write(@"a.txt", (byte*)bytesWrite.GetUnsafePtr(), offset: 0, length: bytesWrite.Length))
{
    while(true)
    {
        // Some codes;
        // 
        if(handle.IsCompleted) // Check for Completion;
        {
            break;
        }
    }
}
```