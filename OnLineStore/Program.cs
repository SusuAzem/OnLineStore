using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.Run();


//builder.Services.AddAutoMapper(cfg => { },
//    typeof(ImageProfile),
//    typeof(PostProfile),
//    typeof(ProductProfile),
//    typeof(UserProfile));
//builder.Configuration
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//   .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
//   .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true)
//   .AddEnvironmentVariables();
//builder.Services.Configure<LibraryOptions>(
//    builder.Configuration.GetSection("Path:Images")
//);
//builder.Services.AddScoped<IFileManager>();

////builder.Services.AddDbContext<AppDbContext>(op =>
////    op.UseSqlServer(builder.Configuration.GetConnectionString("ServerConnection")));
//builder.Services.AddDbContext<AppDbContext>(op =>
//    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//builder.Services.AddSession(op =>
//{
//    op.IdleTimeout = TimeSpan.FromMinutes(30);
//    op.Cookie.IsEssential = true;
//});

//builder.Services.AddLogging(builder => builder.AddDebug());
//builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("Settings"));
//builder.Services.AddTransient<IMailService, MailService>();
//builder.Services.AddTransient<IFileManager, FileManager>();
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddNotyf(config =>
//{
//    config.DurationInSeconds = 8;
//    config.IsDismissable = true;
//    config.Position = NotyfPosition.BottomLeft;
//});

//builder.Services.AddControllersWithViews(options =>
//{
//    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
//    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
//})
//    .AddNewtonsoftJson(options =>
//    {
//        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
//        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
//    }).AddJsonOptions(x =>
//            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//})
//.AddCookie()
//.AddGoogle(options =>
//{
//    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
//    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
//    options.Events.OnTicketReceived = ctx =>
//    {
//        var user = ctx.Principal!.Identities.FirstOrDefault();
//        var value = user!.Claims.FirstOrDefault(m => m.Type == ClaimTypes.Email)!.Value;
//        if (value == StringDefault.AdminEmail1)
//        {
//            user.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
//            user.AddClaim(new Claim(type: "Registered", "true"));
//        }
//        List<AuthenticationToken> tokens = ctx.Properties!.GetTokens().ToList();

//        tokens.Add(new AuthenticationToken()
//        {
//            Name = "TicketCreated",
//            Value = DateTime.UtcNow.ToString()
//        });
//        ctx.Properties!.StoreTokens(tokens);
//        return Task.CompletedTask;
//    };
//});



//builder.Services.AddDataProtection()
//    .SetDefaultKeyLifetime(TimeSpan.FromDays(7))
//    .PersistKeysToDbContext<AppDbContext>();

//builder.Services.AddAuthorizationBuilder()
//    .AddPolicy("Admin", policy =>
//    policy.RequireClaim(ClaimTypes.Email, StringDefault.AdminEmail1))
//    .AddPolicy("Reg", policy =>
//        policy.RequireClaim(claimType: "Registered", allowedValues: "true"))
//    .AddPolicy("Client", policy =>
//    {
//        policy.RequireClaim(ClaimTypes.Role, StringDefault.Client);
//        policy.RequireClaim(claimType: "Registered", allowedValues: "true");
//    });

//var app = builder.Build();

//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}
//else
//{
//    app.UseDeveloperExceptionPage();
//}
//app.UseHttpsRedirection();
//app.UseDefaultFiles();
//app.UseStaticFiles();
//app.UseForwardedHeaders();
//app.UseRouting();
//app.UseSession();
//app.UseCors("Moyasar");
//app.UseAuthentication();
//app.UseAuthorization();
//app.UseNotyf();
//app.MapAreaControllerRoute(
//    name: "ECommerce",
//    areaName: "ECommerce",
//    pattern: "ECommerce/{controller=Home}/{action=Index}/{id?}");

//app.MapAreaControllerRoute(
//    name: "Admin",
//    areaName: "Admin",
//    pattern: "Admin/{controller=Panel}/{action=FrontPage}/{id?}");

//app.MapAreaControllerRoute(
//    name: "UserData",
//    areaName: "UserData",
//    pattern: "UserData/{controller=Panel}/{action=FrontPage}/{id?}");

//app.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");
//app.Run();