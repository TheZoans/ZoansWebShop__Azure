using Zoans.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton<MongoDbService>();

builder.Services.AddAuthentication("Cookies").AddCookie("Cookies", options =>
{
    options.LoginPath = "/Account/Login";

    // added bcz it was required for azure app srvs
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.SlidingExpiration = true;

    // added bcz azure was causing redirect crashes
    options.Events.OnRedirectToLogin = ctx =>
    {
        ctx.Response.Redirect(ctx.RedirectUri);
        return Task.CompletedTask;
    };

});

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

