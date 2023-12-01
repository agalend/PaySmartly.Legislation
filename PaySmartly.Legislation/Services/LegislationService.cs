using Grpc.Core;

namespace PaySmartly.Legislation.Services;

public class LegislationService : Legislation.LegislationBase
{
    private readonly ILogger<LegislationService> _logger;

    private readonly TaxableIncomeTable taxableIncomeTable;

    public LegislationService(ILogger<LegislationService> logger)
    {
        _logger = logger;

        TaxableRange range1 = new() { Start = 0, End = 14_000, Tax = 0.105 };
        TaxableRange range2 = new() { Start = 14_001, End = 48_000, Tax = 0.175 };
        TaxableRange range3 = new() { Start = 48_001, End = 70_000, Tax = 0.3 };
        TaxableRange range4 = new() { Start = 70_001, End = 180_000, Tax = 0.33 };
        TaxableRange range5 = new() { Start = 180_001, End = double.MaxValue, Tax = 0.39 };

        taxableIncomeTable = new TaxableIncomeTable();
        taxableIncomeTable.Ranges.AddRange([range1, range2, range3, range4, range5]);
    }

    public override Task<Response> GetTable(Request request, ServerCallContext context)
    {
        if (request.PayPeriodFrom is null || request.PayPeriodTo is null)
        {
            Response response = new() { Exists = false, InvalidParameters = true };

            return Task.FromResult(response);
        }
        else
        {
            // TODO: we can use immutable db or blockchain to store any legislation data since
            // we should be not able to mutate this data

            Response response = new() { Exists = true, InvalidParameters = false, Table = taxableIncomeTable };

            return Task.FromResult(response);
        }
    }
}
