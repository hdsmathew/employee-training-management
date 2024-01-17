using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Core.Application
{
    public interface IReportGenerationService
    {
        Task GenerateReportAsync(string[] rowHeaders, IEnumerable<object[]> rows, string reportName, Stream outputStream);
    }
}
