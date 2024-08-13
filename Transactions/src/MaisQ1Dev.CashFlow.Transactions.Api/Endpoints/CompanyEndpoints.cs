using MaisQ1Dev.CashFlow.Transactions.Api.Endpoints.Request;
using MaisQ1Dev.CashFlow.Transactions.Api.Extensions;
using MaisQ1Dev.CashFlow.Transactions.Application.Companies.Common;
using MaisQ1Dev.CashFlow.Transactions.Application.Companies.CreateCompany;
using MaisQ1Dev.CashFlow.Transactions.Application.Companies.GetCompanyById;
using MaisQ1Dev.CashFlow.Transactions.Application.Companies.ListCompany;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MaisQ1Dev.CashFlow.Transactions.Api.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompaniesEndpoints(this IEndpointRouteBuilder app)
    {
        var companiesGroup = app
            .MapGroup("v1/companies")
            .WithTags("Companies");


        companiesGroup.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new ListCompanyQuery());
            return result.ToApiResult();
        })
        .WithMetadata(new SwaggerOperationAttribute(summary: "List registered companies", description: "This endpoint returns the list of registered companies"))
        .WithMetadata(new SwaggerResponseAttribute(200, "List registered companies", typeof(CompanyResponse)))
        .Produces<CompanyResponse>(StatusCodes.Status200OK);

        companiesGroup.MapGet("/{id:guid}", async (
            [SwaggerParameter("Company Id to be search", Required = true)] Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(new GetCompanyByIdQuery(id));
            return result.ToApiResult();
        })
        .WithMetadata(new SwaggerOperationAttribute(summary: "Get Company by CompanyId", description: "This endpoint returns the info for company id in the route"))
        .WithMetadata(new SwaggerResponseAttribute(200, "Company info", typeof(CompanyResponse)))
        .WithMetadata(new SwaggerResponseAttribute(404, "Record wasn't found", typeof(ProblemDetails)))
        .Produces<CompanyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        companiesGroup.MapPost("/", async (
            [SwaggerParameter("Company info to be created", Required = true)] CreateCompanyCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToApiResult();
        })
        .WithMetadata(new SwaggerOperationAttribute(summary: "Create a new company", description: "This endpoint is used to add new companies"))
        .WithMetadata(new SwaggerResponseAttribute(201, "Company created successfully", typeof(Guid)))
        .WithMetadata(new SwaggerResponseAttribute(422, "Errors occurred in the validation of business rules", typeof(ProblemDetails)))
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        companiesGroup.MapPut("/{id:guid}", async (
            [SwaggerParameter("Company Id to be updated", Required = true)] Guid id,
            [SwaggerParameter("Company Info to be updated", Required = true)] UpdateCompanyRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(request.ToCommand(id));
            return result.ToApiResult();
        })
        .WithMetadata(new SwaggerOperationAttribute(summary: "Update a company", description: "This endpoint is used to update the info of a registered company"))
        .WithMetadata(new SwaggerResponseAttribute(204, "Company updated successfully"))
        .WithMetadata(new SwaggerResponseAttribute(404, "Record wasn't found", typeof(ProblemDetails)))
        .WithMetadata(new SwaggerResponseAttribute(422, "Errors occurred in the validation of business rules", typeof(ProblemDetails)))
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
    }
}

