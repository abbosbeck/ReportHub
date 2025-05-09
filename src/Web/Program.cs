using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Web.Authentication;
using Web.Components;
using Web.Services.Clients;
using Web.Services.Customers;
using Web.Services.Invoices;
using Web.Services.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTransient<AuthHeaderHandler>();
builder.Services
    .AddHttpClient("api", client =>
        {
            client.BaseAddress = new Uri("https://reporthub.jollyfield-db8a0240.swedencentral.azurecontainerapps.io/");
        })
    .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddSingleton<IUserProviderService, UserProviderService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
