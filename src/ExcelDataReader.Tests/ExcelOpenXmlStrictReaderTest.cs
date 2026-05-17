using System.Data;

namespace ExcelDataReader.Tests;

public class ExcelOpenXmlStrictReaderTest : ExcelOpenXmlReaderBase
{
    protected override DateTime Issue82_TodayDate => new(2013, 4, 19);

    [TestCase("Issue498")]
    public void Issue498_ReadStrictOpenXmlExcelFile(string fileName)
    {
        using IExcelDataReader reader = OpenReader(fileName);
        DataTableCollection tables = reader.AsDataSet().Tables;

        Assert.That(tables.Count, Is.EqualTo(2));

        foreach (DataTable table in tables)
        {
            Assert.That(table.Rows.Count, Is.EqualTo(2));
            Assert.That(table.Columns.Count, Is.EqualTo(2));
            Assert.That(table.Rows[0][0].ToString(), Is.EqualTo("A1"));
        }
    }

    protected override IExcelDataReader OpenReader(Stream stream, ExcelReaderConfiguration configuration = null)
    {
        return ExcelReaderFactory.CreateOpenXmlReader(stream, configuration);
    }

    protected override Stream OpenStream(string name)
    {
        return Configuration.GetTestWorkbook(Path.Combine("strict", name + ".xlsx"));
    }
}