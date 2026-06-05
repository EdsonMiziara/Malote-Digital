using MaloteDigital.Domain.interfaces;
using MaloteDigital.Domain.ValueObjects;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MaloteDigital.InfraStructure.Services;

public partial class TextParserService : ITextParserService
{
    [GeneratedRegex(@"(?:valor|total|a\s+pagar|valor\s+liquido)[^\d]*(?:r\$)?\s*(?<valor>\d{1,3}(?:\.\d{3})*,\d{2})", RegexOptions.IgnoreCase)]
    private static partial Regex AmountRegex();

    [GeneratedRegex(@"(?:vencimento|vence\s+em|dt\s+venc)[^\d]*(?<data>\d{2}/\d{2}/\d{4})", RegexOptions.IgnoreCase)]
    private static partial Regex DueDateRegex();

    [GeneratedRegex(@"(?:emissão|emitido\s+em|data\s+do\s+documento)[^\d]*(?<data>\d{2}/\d{2}/\d{4})", RegexOptions.IgnoreCase)]
    private static partial Regex IssueDateRegex();
    public ExtractedDataResult ParseExpenseData(string rawText)
    {
        if (string.IsNullOrWhiteSpace(rawText))
            return new ExtractedDataResult(0, null, null);

        return new ExtractedDataResult(
            Amount: ExtractAmount(rawText),
            DueDate: ExtractDate(rawText, DueDateRegex()),
            IssueDate: ExtractDate(rawText, IssueDateRegex())
        );
    }

    private DateTime? ExtractDate(string rawText, Regex dateRegex)
    {
        var match = dateRegex.Match(rawText);

        if (match.Success &&
            DateTime.TryParseExact(match.Groups["data"].Value,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime parsedDate))
        {
            return parsedDate;
        }
        return null;
    }

    private decimal ExtractAmount(string rawText)
    {
        var match = AmountRegex().Match(rawText);

        if
            (match.Success &&
            decimal.TryParse(match.Groups["valor"].Value,
            CultureInfo.GetCultureInfo("pt-BR"),
            out decimal parsedAmount))
        {
            return parsedAmount;
        }

        return 0;
    }
}
