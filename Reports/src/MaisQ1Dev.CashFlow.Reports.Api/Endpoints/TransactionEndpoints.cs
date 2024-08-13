using MaisQ1Dev.CashFlow.Reports.Api.Extensions;
using MaisQ1Dev.CashFlow.Reports.Application.Transactions.Common;
using MaisQ1Dev.CashFlow.Reports.Application.Transactions.GetTransactionById;
using MaisQ1Dev.CashFlow.Reports.Application.Transactions.ListDailySummaryTransactionForPeriod;
using MaisQ1Dev.CashFlow.Reports.Application.Transactions.ListTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MaisQ1Dev.CashFlow.Reports.Api.Endpoints;

public static class TransactionEndpoints
{
    public static void MapTransactionsEndpoints(this IEndpointRouteBuilder app)
    {
        var transactionsGroup = app
            .MapGroup("v1/transactions")
            .WithTags("Transactions");

        transactionsGroup.MapGet("/", async (
            ISender sender,
            [FromQuery] Guid companyId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10) =>
        {
            var result = await sender.Send(new ListTransactionQuery(
                companyId,
                startDate,
                endDate,
                pageNumber,
                pageSize));

            return TypedResults.Ok(result);
        });

        transactionsGroup.MapGet("/{id:guid}", async (
            [SwaggerParameter("Transaction Id to be search", Required = true)] Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(new GetTransactionByIdQuery(id));
            return result.ToApiResult();
        })
        .WithMetadata(new SwaggerOperationAttribute(summary: "Get Transaction by TransactionId", description: "This endpoint returns the info for transaction id in the route"))
        .WithMetadata(new SwaggerResponseAttribute(200, "Transaction info", typeof(TransactionResponse)))
        .WithMetadata(new SwaggerResponseAttribute(404, "Record wasn't found", typeof(ProblemDetails)))
        .Produces<TransactionResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        transactionsGroup.MapGet("/{companyId:guid}/summaries/{startDate:datetime}/{endDate:datetime}", async (
            [SwaggerParameter("Company Id to get the daily summary", Required = true)] Guid companyId,
            [SwaggerParameter("Start date from the daily summary", Required = true)] DateTime startDate,
            [SwaggerParameter("End date from the daily summary", Required = true)] DateTime endDate,
            ISender sender) =>
        {
            var result = await sender.Send(new ListDailySummaryTransactionForPeriodQuery(
                companyId,
                startDate,
                endDate));
            return result.ToApiResult();
        })
        .WithMetadata(new SwaggerOperationAttribute(summary: "List daily summary of the transactions for CompanyId and Period", description: "This endpoint returns the daily summary of the transactions for company id and period in the route"))
        .WithMetadata(new SwaggerResponseAttribute(200, "Daily Summary info", typeof(TransactionResponse)))
        .Produces<TransactionResponse>(StatusCodes.Status200OK);
    }
}
