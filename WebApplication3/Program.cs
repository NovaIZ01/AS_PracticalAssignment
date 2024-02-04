using Microsoft.AspNetCore.Identity;
using WebApplication3.Model;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3; 
})
.AddEntityFrameworkStores<AuthDbContext>();
builder.Services.AddDataProtection();

builder.Services.AddDataProtection();


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{   
    options.IdleTimeout = TimeSpan.FromSeconds(10);
});

builder.Services.ConfigureApplicationCookie(config =>
{   
    config.LoginPath = "/login";
});

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

app.UseStatusCodePagesWithRedirects("/ErrorPages/custom{0}");
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
