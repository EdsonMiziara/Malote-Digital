using MaloteDigital.Domain.Interfaces;
using MaloteDigital.Domain.ValueObjects;
using System.Globalization;

namespace MaloteDigital.InfraStructure.Services;

public class OfxParserService : IOfxParserService
{
    public List<OfxTransactionResult> ParseDebits(Stream ofxStream)
    {
        var transactions = new List<OfxTransactionResult>();
        using var reader = new StreamReader(ofxStream);

        string? line;
        bool insideTransaction = false;

        string txId = string.Empty;
        decimal amount = 0;
        DateTime datePosted = DateTime.MinValue;
        string memo = string.Empty;
        string type = string.Empty;

        while ((line = reader.ReadLine()) != null)
        {
            line = line.Trim();

            if (line.StartsWith("<STMTTRN>", StringComparison.OrdinalIgnoreCase))
            {
                insideTransaction = true;
                continue;
            }

            if (line.StartsWith("</STMTTRN>", StringComparison.OrdinalIgnoreCase))
            {
                insideTransaction = false;

                if (type == "DEBIT" || amount < 0)
                {
                    decimal positiveAmount = Math.Abs(amount);

                    transactions.Add(new OfxTransactionResult(txId, positiveAmount, datePosted, memo));
                }

                txId = string.Empty; amount = 0; memo = string.Empty; type = string.Empty;
                continue;
            }

            if (insideTransaction)
            {
                if (line.StartsWith("<TRNTYPE>", StringComparison.OrdinalIgnoreCase))
                    type = ExtractValue(line, "<TRNTYPE>");

                else if (line.StartsWith("<TRNAMT>", StringComparison.OrdinalIgnoreCase))
                    amount = decimal.Parse(ExtractValue(line, "<TRNAMT>"), CultureInfo.InvariantCulture);

                else if (line.StartsWith("<FITID>", StringComparison.OrdinalIgnoreCase))
                    txId = ExtractValue(line, "<FITID>");

                else if (line.StartsWith("<MEMO>", StringComparison.OrdinalIgnoreCase))
                    memo = ExtractValue(line, "<MEMO>");

                else if (line.StartsWith("<DTPOSTED>", StringComparison.OrdinalIgnoreCase))
                    datePosted = ParseOfxDate(ExtractValue(line, "<DTPOSTED>"));
            }
        }

        return transactions;
    }

    private DateTime ParseOfxDate(string rawDate)
    {
        if (rawDate.Length >= 8)
        {
            string datePart = rawDate.Substring(0, 8);
            if (DateTime.TryParseExact(datePart, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }
        }
        return DateTime.MinValue;
    }

    private string ExtractValue(string line, string tag)
    {
        return line.Substring(tag.Length).Trim();
    }
}
