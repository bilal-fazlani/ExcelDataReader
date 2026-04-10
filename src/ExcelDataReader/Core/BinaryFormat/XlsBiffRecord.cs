namespace ExcelDataReader.Core.BinaryFormat;

/// <summary>
/// Represents basic BIFF record.
/// Base class for all BIFF record types.
/// </summary>
internal class XlsBiffRecord
{
    // Records with a total buffer size at or below this threshold are allocated with
    // new byte[] rather than rented from ArrayPool. The pool's per-operation overhead
    // (bucket lookup + thread-local push/pop) exceeds the benefit for tiny buffers.
    // Rented buffers always have Length > SmallRecordPoolThreshold because ArrayPool
    // rounds up to the next power-of-two bucket (≥128 when totalSize > 64), so the
    // check in Return() reliably distinguishes heap-allocated from pool-rented arrays.
    internal const int SmallRecordPoolThreshold = 64;

    protected const int ContentOffset = 4;
       
    public XlsBiffRecord(byte[] bytes)
    {
        if (bytes.Length < 4)
            throw new ArgumentException(Errors.ErrorBiffRecordSize);
        Bytes = bytes;
    }

    /// <summary>
    /// Gets the type Id of this entry.
    /// </summary>
    public BIFFRECORDTYPE Id => (BIFFRECORDTYPE)BitConverter.ToUInt16(Bytes, 0);

    /// <summary>
    /// Gets the data size of this entry.
    /// </summary>
    public ushort RecordSize => BitConverter.ToUInt16(Bytes, 2);

    /// <summary>
    /// Gets the whole size of structure.
    /// </summary>
    public int Size => ContentOffset + RecordSize;
    
    internal byte[] Bytes { get; }

    #if NETSTANDARD2_1_OR_GREATER || NET8_0_OR_GREATER
    public virtual void Return()
    {        
        // Only buffers rented from ArrayPool need to be returned.
        // Heap-allocated small records (Bytes.Length <= SmallRecordPoolThreshold) are GC-managed.
        if (Bytes.Length > SmallRecordPoolThreshold)
            System.Buffers.ArrayPool<byte>.Shared.Return(Bytes);
    }
    #endif

    public byte ReadByte(int offset)
    {
        return Buffer.GetByte(Bytes, ContentOffset + offset);
    }

    public ushort ReadUInt16(int offset)
    {
        return BitConverter.ToUInt16(Bytes, ContentOffset + offset);
    }

    public uint ReadUInt32(int offset)
    {
        return BitConverter.ToUInt32(Bytes, ContentOffset + offset);
    }

    public ulong ReadUInt64(int offset)
    {
        return BitConverter.ToUInt64(Bytes, ContentOffset + offset);
    }

    public short ReadInt16(int offset)
    {
        return BitConverter.ToInt16(Bytes, ContentOffset + offset);
    }

    public int ReadInt32(int offset)
    {
        return BitConverter.ToInt32(Bytes, ContentOffset + offset);
    }

    public long ReadInt64(int offset)
    {
        return BitConverter.ToInt64(Bytes, ContentOffset + offset);
    }

    public float ReadFloat(int offset)
    {
        return BitConverter.ToSingle(Bytes, ContentOffset + offset);
    }

    public double ReadDouble(int offset)
    {
        return BitConverter.ToDouble(Bytes, ContentOffset + offset);
    }
}
