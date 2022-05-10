
var builder = WebApplication.CreateBuilder(args);
builder.Services.UseCommercetoolsApi(builder.Configuration,
                new List<string> { "Client", "ImportApiClient", "BerlinStoreClient", "MeClient" },
                TokenProviderExtension.CreateTokenProvider);
// Add services to the container.
builder.Services.AddScoped<IHelloWorldService, HelloWorldService>();
builder.Services.AddScoped<IDummyExcercise, DummyExcercise>();
builder.Services.AddScoped<ICreateService, CreateService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
