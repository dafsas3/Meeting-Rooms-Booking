using MeetingRoomsBooking.Api.Swagger;
using MeetingRoomsBooking.Features.Abstractions.Security;
using MeetingRoomsBooking.Infrastructure.Security;

namespace MeetingRoomsBooking.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<UserHeadersOperationFilter>();
            });

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();

            return services;
        }
    }
}
