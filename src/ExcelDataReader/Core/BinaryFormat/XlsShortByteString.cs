using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

/// <summary>
/// Byte sized string, stored as bytes, with encoding from CodePage record. Used in BIFF2-5 .
/// </summary>
internal sealed class XlsShortByteString(byte[] bytes, uint offset) : IXlsString
{
    private readonly byte[] _bytes = bytes;
    private readonly uint _offset = offset;

    public ushort CharacterCount => _bytes[_offset];

    public string GetValue(Encoding encoding) =>
        encoding.GetString(_bytes, (int)_offset + 1, CharacterCount * (Helpers.IsSingleByteEncoding(encoding) ? 1 : 2));
}
