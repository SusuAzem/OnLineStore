using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;

using Business;

using Data;
using Data.IRepository;
using Data.Repository;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using OnLineStore.Profiles;

using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(cfg => { },
    typeof(ProductProfile),
    typeof(UserProfile));
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
   .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
   .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true)
   .AddEnvironmentVariables();
builder.Services.Configure<LibraryOptions>(builder.Configuration.GetSection("Path:Images"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("Settings"));
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IFileManager, FileManager>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDataProtection()
    .SetDefaultKeyLifetime(TimeSpan.FromDays(7))
    .PersistKeysToDbContext<AppDbContext>();

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 8;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomLeft;
});
builder.Services.AddSession(op =>
{
    op.IdleTimeout = TimeSpan.FromMinutes(30);
    op.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(options =>
{
    // Set the default authentication scheme to use cookies
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // Set Google as the default challenge (login) scheme
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme,options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
});
builder.Services.AddAuthorizationBuilder()
   .AddPolicy("Admin", policy =>
    { 
        policy.RequireClaim(ClaimTypes.Email, StringDefault.AdminEmail);
    });

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
}).AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseNotyf();

app.MapControllerRoute(
    name: "areasOnly",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.Run();



//builder.Services.AddLogging(builder => builder.AddDebug());
//builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();