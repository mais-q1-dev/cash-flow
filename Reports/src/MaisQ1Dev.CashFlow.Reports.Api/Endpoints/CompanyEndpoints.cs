using MaisQ1Dev.CashFlow.Reports.Api.Extensions;
using MaisQ1Dev.CashFlow.Reports.Application.Companies.GetCompanyById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MaisQ1Dev.CashFlow.Reports.Api.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompaniesEndpoints(this IEndpointRouteBuilder app)
    {
        var companiesGroup = app
            .MapGroup("v1/companies")
            .WithTags("Companies");

        companiesGroup.MapGet("/{id:guid}", async (
            [SwaggerParameter("Company Id to be search", Required = true)] Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(new GetCompanyByIdQuery(id));
            return result.ToApiResult();
        })
        .WithMetadata(new SwaggerOperationAttribute(summary: "Get Company by CompanyId", description: "This endpoint returns the info for company id in the route"))
        .WithMetadata(new SwaggerResponseAttribute(200, "Company info", typeof(GetCompanyByIdResponse)))
        .WithMetadata(new SwaggerResponseAttribute(404, "Record wasn't found", typeof(ProblemDetails)))
        .Produces<GetCompanyByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}