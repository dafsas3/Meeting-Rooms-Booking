using MeetingRoomsBooking.Api.Extensions;
using MeetingRoomsBooking.Api.Middleware;
using MeetingRoomsBooking.Features.Extensions;
using MeetingRoomsBooking.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
