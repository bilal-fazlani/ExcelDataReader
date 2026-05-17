using System.Text;
using BenchmarkDotNet.Attributes;

namespace ExcelDataReader.Benchmarks;

/// <summary>
/// Benchmarks for reading all string cell values, exercising SST lookup performance.
/// For XLS, this validates the lazy string cache (issue #525): strings are decoded and cached
/// on first access in GetString(), releasing the raw byte[] backing after the first lookup.
/// Repeated access of the same SST index returns the cached string with zero allocation.
/// </summary>
[MemoryDiagnoser]
public class ReadAllStrings
{
    [GlobalSetup]
    public void Setup()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    [Benchmark]
    public string ReadAllStringsXlsx()
    {
        return ReadStrings(ExcelReaderFactory.CreateReader(
            typeof(OpenXmlFile).Assembly.GetManifestResourceStream("ExcelDataReader.Benchmarks.10x10000.xlsx")));
    }

    [Benchmark]
    public string ReadAllStringsXlsb()
    {
        return ReadStrings(ExcelReaderFactory.CreateReader(
            typeof(OpenXmlFile).Assembly.GetManifestResourceStream("ExcelDataReader.Benchmarks.10x10000.xlsb")));
    }

    [Benchmark]
    public string ReadAllStringsXls()
    {
        return ReadStrings(ExcelReaderFactory.CreateReader(
            typeof(OpenXmlFile).Assembly.GetManifestResourceStream("ExcelDataReader.Benchmarks.10x10000.xls")));
    }

    private static string ReadStrings(IExcelDataReader reader)
    {
        using (reader)
        {
            string last = null;
            while (reader.Read())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                    last = reader.GetString(i);
            }

            return last;
        }
    }
}
