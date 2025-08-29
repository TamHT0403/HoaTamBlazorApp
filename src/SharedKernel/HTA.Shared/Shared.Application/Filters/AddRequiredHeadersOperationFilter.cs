using Shared.Domain;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Application
{
    public class AddRequiredHeadersOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            // Thêm custom header "x-tenant-id"
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = SystemConstant.HEADER_TENANT_ID,
                In = ParameterLocation.Header,
                Required = false, // nếu bắt buộc thì để true
                Schema = new OpenApiSchema { Type = "string" },
                Description = "Tenant identifier"
            });

            // Thêm custom header "x-request-id"
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = SystemConstant.HEADER_REQUEST_ID,
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema { Type = "string" },
                Description = "Request trace identifier"
            });
        }
    }
}