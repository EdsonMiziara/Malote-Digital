using MaloteDigital.Domain.ValueObjects;

namespace MaloteDigital.Domain.Interfaces;

public interface IOfxParserService
{
    /// <summary>
    /// receives an OFX file stream and parses the transactions contained within it,
    /// returning a list of OfxTransactionResult objects that represent the details of each transaction,
    /// such as transaction ID, amount, date posted, and description.
    /// </summary>
    /// <param name="ofxStream"></param>
    /// <returns></returns>
    List<OfxTransactionResult> ParseDebits(Stream ofxStream);
}
