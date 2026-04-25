using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MeetingRoomsBooking.Api.Swagger
{
    public class UserHeadersOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-EmployeeID",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Format = "uuid"
                },
                Description = "Employee identifier (GUID)"
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-UserRole",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Description = "User role (Employee/Admin)"
            });
        }
    }
}
