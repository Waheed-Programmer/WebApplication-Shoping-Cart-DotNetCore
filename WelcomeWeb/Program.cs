using Microsoft.EntityFrameworkCore;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.DataAccessLayer.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using MyApp.CommonHelper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using MyApp.DataAccessLayer.DbIntitializer;

var builder = WebApplication.CreateBuilder(args);

//Add DataIndependency Services Methods

builder.Services.AddScoped<IUnitofWork, UnitOfWork>();
builder.Services.AddScoped<IDbIntitializer, DbIntitialize>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Integration Stripe Api in project
builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("PaymentSettings"));



// Add Connections String to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();


//Add services email sent config
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}  );
        
        
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
StripeConfiguration.SetApiKey(builder.Configuration["PaymentSettings:SecretKey"]);
app.UseHttpsRedirection();
app.UseStaticFiles();
//StripeConfiguration.SetApiKey = builder.Configuration.GetSection("PaymentSettings: SecretKey").Get<string>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.UseSession();
dataSedding();


app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void dataSedding()
{
    using (var scope = app.Services.CreateScope())
    {
        var DbIntializer = scope.ServiceProvider.GetRequiredService<IDbIntitializer>();
        DbIntializer.Intitialize();
    }
}
