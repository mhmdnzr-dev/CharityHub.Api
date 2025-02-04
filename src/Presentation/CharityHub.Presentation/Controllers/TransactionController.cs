namespace CharityHub.Presentation.Controllers;

using Core.Contract.Terms.Queries.GetLastTerm;
using Core.Contract.Transactions.Queries.GetUserTransactions;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TransactionController:BaseController
{
    public TransactionController(IMediator mediator) : base(mediator)
    {
    }
    
    
    /// <summary>
    /// Retrieves the user transactions based on the provided query parameters.
    /// </summary>
    /// <param name="query">The query parameters to filter the user transactions data.</param>
    /// <returns>A response containing the user transactions data.</returns>
    [HttpGet("get-user-transactions")]
    [MapToApiVersion("1.0")]
    [Authorize]
    [OutputCache(PolicyName = "Expire20")]
    [SwaggerOperation(Summary = "Get User Transactions",
        Description = "Retrieves the user transactions based on the provided query parameters.")]
    [SwaggerResponse(200, "User transactions retrieved successfully",
        typeof(IEnumerable<UserTransactionsResponseDto>))] // Replace with your actual response DTO
    [SwaggerResponse(400, "Bad Request, invalid query parameters")]
    [SwaggerResponse(500, "Internal Server Error")]
    public async Task<IActionResult> Get([FromQuery] GetUserTransactionQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}