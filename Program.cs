using HTQLTV.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("HtqltvContext");
builder.Services.AddDbContext<HtqltvContext>(x=>x.UseSqlServer(connectionString));

builder.Services.AddSession();

builder.Services.AddSession(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

//Thêm d?ch v? xác th?c và ?y quy?n
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = "abc",
//            ValidAudience = "abcd",
//            IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret"))
//        };
//    });
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy =>
//    {
//        policy.RequireRole("Admin");
//    });

//    options.AddPolicy("StaffPolicy", policy =>
//    {
//        policy.RequireRole("Staff");
//    });

//    options.AddPolicy("CanView", policy =>
//    {
//        policy.RequireClaim(" AssociatedID", "1");
//    });
//});

var app = builder.Build();


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


app.UseAuthorization();
app.UseAuthentication();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
