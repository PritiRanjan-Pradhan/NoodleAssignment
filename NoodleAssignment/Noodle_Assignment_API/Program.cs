
var builder = WebApplication.CreateBuilder(args);
builder.Services.UseCommercetoolsApi(builder.Configuration,
                new List<string> { "Client", "ImportApiClient", "BerlinStoreClient", "MeClient" },
                TokenProviderExtension.CreateTokenProvider);
// Add services to the container.
builder.Services.AddScoped<IHelloWorldService, HelloWorldService>();
builder.Services.AddScoped<IDummyExcercise, DummyExcercise>();
builder.Services.AddScoped<ICreateService, CreateService>();
builder.Services.AddScoped<IUpdateGroupService, UpdateGroupService>();
builder.Services.AddScoped<IImportApiService,ImportAPIService>();
builder.Services.AddScoped<IStateMachineService, StateMachineService>();
builder.Services.AddScoped<ICheckoutService, CheckoutServivce>();
builder.Services.AddScoped<ICartMerging,CartMerging>();
builder.Services.AddScoped<IMeService, MeService>();
builder.Services.AddScoped<IInStore,InStoreService>();
builder.Services.AddScoped<ISerchService,SearchService>();
builder.Services.AddScoped<IProductSelectionService, ProductSelectionService>();
builder.Services.AddScoped<IPagedQuery, PagedQuery>();
builder.Services.AddScoped<IGraphQLService,GraphQlService>();
builder.Services.AddScoped<ICustomType,CustomType>(); 
builder.Services.AddScoped<ICustomObjectService,CustomObjectsService>();
builder.Services.AddScoped<IApiExtensionService,ApiExtensionSevice>();
builder.Services.AddScoped<ISubscriptionService , SubscriptionService>();
builder.Services.AddScoped<IErrorHandlingService, ErrorHandlingService>();
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
