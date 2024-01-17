using Core.Application;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.ReportGeneration
{
    public class ExcelReportGenerationService : IReportGenerationService
    {
        public async Task GenerateReportAsync(string[] rowHeaders, IEnumerable<object[]> rows, string reportName, Stream outputStream)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                if (outputStream.Length > 0)
                {
                    await package.LoadAsync(outputStream);
                }

                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(reportName);

                for (int i = 0; i < rowHeaders.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = rowHeaders[i];
                }

                int currentRowIdx = 2;
                foreach (object[] row in rows)
                {
                    for (int col = 0; col < rowHeaders.Length; col++)
                    {
                        worksheet.Cells[currentRowIdx, col + 1].Value = row[col];
                    }
                    currentRowIdx++;
                }
                worksheet.Cells.AutoFitColumns();

                outputStream.SetLength(0);
                await package.SaveAsAsync(outputStream);
            }
        }
    }
}
