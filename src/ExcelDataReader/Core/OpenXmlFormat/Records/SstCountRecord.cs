namespace ExcelDataReader.Core.OpenXmlFormat.Records;

internal sealed class SstCountRecord(int uniqueCount) : Record
{
    public int UniqueCount { get; } = uniqueCount;
}
