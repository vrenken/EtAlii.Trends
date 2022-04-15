using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.ConfigureAppConfiguration(configuration => configuration.ExpandEnvironmentVariablesInJson());

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSyncfusionBlazor();

builder.Services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddSingleton<IGetUserQueryHandler, GetUserQueryHandler>();
builder.Services.AddSingleton<IGetDiagramQueryHandler, GetDiagramQueryHandler>();
builder.Services.AddSingleton<IGetAllDiagramsForUserQueryHandler, GetAllDiagramsForUserQueryHandler>();
builder.Services.AddSingleton<IGetAllLayersQueryHandler, GetAllLayersQueryHandler>();
builder.Services.AddSingleton<IGetAllTrendsQueryHandler, GetAllTrendsQueryHandler>();
builder.Services.AddSingleton<IAddTrendCommandHandler, AddTrendCommandHandler>();
builder.Services.AddSingleton<IUpdateTrendCommandHandler, UpdateTrendCommandHandler>();


builder.Services.AddSingleton<IUpdateLayerCommandHandler, UpdateLayerCommandHandler>();
builder.Services.AddSingleton<IAddLayerCommandHandler, AddLayerCommandHandler>();
builder.Services.AddSingleton<IRemoveLayerCommandHandler, RemoveLayerCommandHandler>();

builder.Services.AddSingleton<DataContext>();
new DatabaseInitializer().InitializeWhenNeeded();
var systemContext = new ApplicationContext();
systemContext.Initialize();
builder.Services.AddSingleton(systemContext);

Logging.ConfigureGlobalLogging(builder.Configuration);

builder.Host.UseSerilog((context, loggerConfiguration) => Logging.ConfigureWebHostLogging(context.Configuration, loggerConfiguration), true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
