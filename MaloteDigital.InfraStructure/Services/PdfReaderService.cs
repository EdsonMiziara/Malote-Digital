using MaloteDigital.Domain.interfaces;
using System.Text;
using UglyToad.PdfPig;

namespace MaloteDigital.InfraStructure.Services;

public class PdfReaderService : IPdfReaderService
{
    public async Task<string> ExtractTextAsync(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("O arquivo PDF não foi encontrado.", filePath);

        return await Task.Run(() =>
        {
            var textBuilder = new StringBuilder();

            using (PdfDocument pdf = PdfDocument.Open(filePath))
            {
                foreach (var page in pdf.GetPages())
                {
                    textBuilder.AppendLine(page.Text);
                }
            }
            return textBuilder.ToString();
        });
    }
}
