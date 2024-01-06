using working_good.business.api.Configuration;
using working_good.business.infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.SetConfiguration(builder.Configuration);
var app = builder.Build();
app.UseInfrastructure();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
