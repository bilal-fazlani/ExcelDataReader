using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

/// <summary>
/// Plain string without backing storage. Used internally.
/// </summary>
internal sealed class XlsInternalString(string value) : IXlsString
{
    public string GetValue(Encoding encoding) => value;
}
