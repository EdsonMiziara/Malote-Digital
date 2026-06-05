using MaloteDigital.Domain.ValueObjects;

namespace MaloteDigital.Domain.interfaces;

public interface ITextParserService
{
    ///<summary>
    /// Receives a raw text and finds specified parameters like amount and due date.
    /// </summary>
    ExtractedDataResult ParseExpenseData(string rawText);
}
