using Grpc.Core;

namespace PaySmartly.Legislation.Services;

public class LegislationService(ILogger<LegislationService> logger) : Legislation.LegislationBase
{
    private readonly ILogger<LegislationService> _logger = logger;

    public override Task<Response> GetTable(Request request, ServerCallContext context)
    {
        return Task.FromResult(new Response
        {
            Message = "Hello " + request.Name
        });
    }
}
