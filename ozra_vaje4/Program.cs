using Microsoft.AspNetCore.Identity;
using ozra_vaje4;
using ozra_vaje4.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddIdentity<UserViewModel, IdentityRole>(
      
        )
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddHttpClient();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Administrator"));

    options.AddPolicy("BasicPolicy", policy =>
        policy.RequireRole("Uporabnik"));
});
// Add services to the container.
builder.Services.AddControllersWithViews().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

builder.Services.AddLocalization(options=>
    options.ResourcesPath= "Resources"

    );

builder.Services.Configure<RequestLocalizationOptions>(options =>
{

    var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("sl-SI"),

            };
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("sl-SI");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});





var app = builder.Build();
app.UseRequestLocalization();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Uporabnik", "Administrator" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }

    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
