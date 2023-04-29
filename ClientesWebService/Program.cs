using Books.DB;
using ClientesWebService.DB;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

var ConfBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration configuration = ConfBuilder.Build();



builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Trace);
})
.UseNLog();



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ICommandAddBook, CommandAddBook>();
builder.Services.AddScoped<ICommandRegUpdateCustomer, CommandRegUpdateCustomer>();
builder.Services.AddScoped<IQueryGetCustomers, QueryGetCustomers>();
builder.Services.AddScoped<IQueryGetProducts, QueryGetProducts>();
builder.Services.AddScoped<IQueryGetCustomerCode, QueryGetCustomerCode>();
builder.Services.AddScoped<IQueryGetBookCode, QueryGetBookCode>();
builder.Services.AddScoped<IQueryGetBookByCode, QueryGetBookByCode>();
builder.Services.AddScoped<ICommandAddStock, CommandAddStock>();
builder.Services.AddScoped<ICommandRetirarLibrosUsuario, CommandRetirarLibrosUsuario>();
builder.Services.AddScoped<IQueryGetBooksUser, QueryGetBooksUser>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
