using System.Xml;
using ExcelDataReader.Core.OpenXmlFormat.Records;

namespace ExcelDataReader.Core.OpenXmlFormat.XmlFormat;

internal sealed class XmlSharedStringsReader(XmlReader reader) : XmlRecordReader(reader)
{
    private const string ElementSst = "sst";
    private const string ElementStringItem = "si";
    private const string AttributeUniqueCount = "uniqueCount";

    protected override IEnumerable<Record> ReadOverride()
    {
        if (!Reader.IsStartElement(ElementSst, ProperNamespaces.NsSpreadsheetMl))
        {
            yield break;
        }

        var uniqueCountStr = Reader.GetAttribute(AttributeUniqueCount);
        if (int.TryParse(uniqueCountStr, out var uniqueCount) && uniqueCount > 0)
        {
            yield return new SstCountRecord(uniqueCount);
        }

        if (!XmlReaderHelper.ReadFirstContent(Reader))
        {
            yield break;
        }

        while (!Reader.EOF)
        {
            if (Reader.NodeType == XmlNodeType.Element && Reader.LocalName == ElementStringItem)
            {
                var value = StringHelper.ReadStringItem(Reader, ProperNamespaces.NsSpreadsheetMl);
                yield return new SharedStringRecord(value);
            }
            else if (!XmlReaderHelper.SkipContent(Reader))
            {
                break;
            }
        }
    }
}
