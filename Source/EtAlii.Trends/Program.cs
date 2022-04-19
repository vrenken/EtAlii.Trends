using System.Globalization;
using EtAlii.Trends.Diagrams;
using EtAlii.Trends.Editor.Layers;
using EtAlii.Trends.Editor.Trends;
using Serilog;

var options = new WebApplicationOptions { Args = args, WebRootPath = @"_Shared\wwwroot" };
var builder = WebApplication.CreateBuilder(options);

builder.WebHost.ConfigureAppConfiguration(configuration => configuration.ExpandEnvironmentVariablesInJson());

// Add services to the container.
builder.Services.AddRazorPages(o => o.RootDirectory = "/Pages");
builder.Services.AddServerSideBlazor();
builder.Services.AddSyncfusionBlazor();

builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddSingleton<IGetUserQueryHandler, GetUserQueryHandler>();

builder.Services.AddSingleton<IGetDiagramQueryHandler, GetDiagramQueryHandler>();
builder.Services.AddSingleton<IGetAllDiagramsForUserQueryHandler, GetAllDiagramsForUserQueryHandler>();
builder.Services.AddSingleton<IUpdateDiagramCommandHandler, UpdateDiagramCommandHandler>();

builder.Services.AddSingleton<IGetAllLayersQueryHandler, GetAllLayersQueryHandler>();
builder.Services.AddSingleton<IUpdateLayerCommandHandler, UpdateLayerCommandHandler>();
builder.Services.AddSingleton<IAddLayerCommandHandler, AddLayerCommandHandler>();
builder.Services.AddSingleton<IRemoveLayerCommandHandler, RemoveLayerCommandHandler>();

builder.Services.AddSingleton<IGetAllTrendsQueryHandler, GetAllTrendsQueryHandler>();
builder.Services.AddSingleton<IAddTrendCommandHandler, AddTrendCommandHandler>();
builder.Services.AddSingleton<IUpdateTrendCommandHandler, UpdateTrendCommandHandler>();

builder.Services.AddSingleton<IGetComponentsForTrendQueryHandler, GetComponentsForTrendQueryHandler>();
builder.Services.AddSingleton<IAddComponentCommandHandler, AddComponentCommandHandler>();
builder.Services.AddSingleton<IUpdateComponentCommandHandler, UpdateComponentCommandHandler>();
builder.Services.AddSingleton<IRemoveComponentCommandHandler, RemoveComponentCommandHandler>();

builder.Services.AddSingleton<IGetAllConnectionsQueryHandler, GetAllConnectionsQueryHandler>();
builder.Services.AddSingleton<IAddConnectionCommandHandler, AddConnectionCommandHandler>();

builder.Services.AddSingleton<ITrendNodesLoader, TrendNodesLoader>();
builder.Services.AddSingleton<IComponentConnectionLoader, ComponentConnectionLoader>();

builder.Services.AddSingleton<DataContext>();
new DatabaseInitializer().InitializeWhenNeeded();
var systemContext = new ApplicationContext();
systemContext.Initialize();
builder.Services.AddSingleton(systemContext);

Logging.ConfigureGlobalLogging(builder.Configuration);

builder.Host.UseSerilog((context, loggerConfiguration) => Logging.ConfigureWebHostLogging(context.Configuration, loggerConfiguration), true);

var app = builder.Build();

// Let's invariant culture for web presentation (i.e. have the whole application in english.
app.Use(async (_, next) =>
{
    CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

    // Call the next delegate/middleware in the pipeline
    await next().ConfigureAwait(false);
});

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
