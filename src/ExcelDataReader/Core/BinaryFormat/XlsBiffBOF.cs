namespace ExcelDataReader.Core.BinaryFormat;

/// <summary>
/// Represents BIFF BOF record.
/// </summary>
internal sealed class XlsBiffBOF : XlsBiffRecord
{
    internal XlsBiffBOF(byte[] bytes)
        : base(bytes)
    {
    }

    /// <summary>
    /// Gets the version.
    /// </summary>
    public ushort Version => ReadUInt16(0x0);

    /// <summary>
    /// Gets the type of the BIFF block.
    /// </summary>
    public BIFFTYPE Type => (BIFFTYPE)ReadUInt16(0x2);
}