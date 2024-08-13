using MaisQ1Dev.CashFlow.Transactions.Api.Endpoints.Request;
using MaisQ1Dev.CashFlow.Transactions.Api.Extensions;
using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.Common;
using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.CreateTransaction;
using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.GetTransactionById;
using MaisQ1Dev.CashFlow.Transactions.Application.Transactions.ListTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MaisQ1Dev.CashFlow.Transactions.Api.Endpoints;
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

        transactionsGroup.MapPost("/", async (
            [SwaggerParameter("Transaction info to be created.", Required = true)] CreateTransactionCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToApiResult();
        })
        .WithMetadata(new SwaggerOperationAttribute(summary: "Create a new transaction", description: "This endpoint is used to add new transactions"))
        .WithMetadata(new SwaggerResponseAttribute(201, "Transaction created successfully", typeof(Guid)))
        .WithMetadata(new SwaggerResponseAttribute(422, "Errors occurred in the validation of business rules", typeof(ProblemDetails)))
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        transactionsGroup.MapPut("/{id:guid}", async (
            [SwaggerParameter("Transaction Id to be updated", Required = true)] Guid id,
            [SwaggerParameter("Transaction Info to be updated", Required = true)] UpdateTransactionRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(request.ToCommand(id));
            return result.ToApiResult();
        })
        .WithMetadata(new SwaggerOperationAttribute(summary: "Update a transaction", description: "This endpoint is used to update the info of a registered transaction"))
        .WithMetadata(new SwaggerResponseAttribute(204, "Transaction updated successfully"))
        .WithMetadata(new SwaggerResponseAttribute(404, "Record wasn't found", typeof(ProblemDetails)))
        .WithMetadata(new SwaggerResponseAttribute(422, "Errors occurred in the validation of business rules", typeof(ProblemDetails)))
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
    }
}