using DependencyAnalysis.Services; 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IExternalServiceClient, ExternalServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/"); 
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

app.Run("http://localhost:5000");