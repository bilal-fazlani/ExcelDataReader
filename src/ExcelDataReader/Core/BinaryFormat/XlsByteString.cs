using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

/// <summary>
/// Word-sized string, stored as single bytes with encoding from CodePage record. Used in BIFF2-5. 
/// </summary>
internal sealed class XlsByteString(byte[] bytes, uint offset) : IXlsString
{
    private readonly byte[] _bytes = bytes;
    private readonly uint _offset = offset;

    /// <summary>
    /// Gets the number of characters in the string.
    /// </summary>
    public ushort CharacterCount => BitConverter.ToUInt16(_bytes, (int)_offset);

    /// <summary>
    /// Gets the value.
    /// </summary>
    public string GetValue(Encoding encoding)
    {
        int byteCount = CharacterCount * (Helpers.IsSingleByteEncoding(encoding) ? 1 : 2);
        return encoding.GetString(_bytes, (int)_offset + 2, byteCount);
    }
}