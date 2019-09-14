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

## static class UniUnsafeIO.UnsafeIOManager

### IOHandle Write(string path, void* buffer, ulong offset, ulong length)

This method issues async write order.

- string path
    - Ooutput destination file path
    - If path is null, ArgumentNullException will be thrown.
- byte* buffer
    - Source pointer. This buffer's contnet should not be changed while writing to the file.
    - If buffer is null, ArgumentNullException will be thrown.
- ulong offset
    - The byte offset at which to start writing to the file.
- ulong length
    - The byte length to write. 
    - If length is 0, default(IOHandle) will be returned.

*Remarks*
This api lengthens the file size when the length argument is larger than the original size but never shortens.
If you want exact length. You should assign the length to FileHandle.Length property.

---

## struct FileHandle : IDisposable

### FileHandle(string path, System.IO.FileAccess access)

Create File handle with path.

### FileHandle(IntPtr fileHandle)

Create from IntPtr representing Win32 native FileHandle.

### long Legnth { get; set; }

You can get/set the current byte length of the file.

---

## struct IOHandle : IDisposable

### FileHandle FileHandle

You can get/set the file size via this field.

### bool IsCompleted { get; }

Check for the completion.

### void Complete()

Wait for the completion.

### void Dispose()

You should call Dispose() after writing completion to free the native resources.

# Example

You can wait the completion of IO.

```csharp
NativeArray<byte> bytesWrite;

// Some codes;

using(IOHandle handle = UniUnsafeIO.UnsafeIOManager.Write(@"a.txt", bytesWrite.GetUnsafePtr(), offset: 0, length: (ulong)bytesWrite.Length))
{    
    // Some work;
    handle.Complete(); // Wait for Completion;
}
```

Or you can poll the IO operation.

```csharp
NativeArray<byte> bytesWrite;

// Some codes;

using(IOHandle handle = UniUnsafeIO.UnsafeIOManager.Write(@"a.txt", bytesWrite.GetUnsafePtr(), offset: 0, length: bytesWrite.Length))
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