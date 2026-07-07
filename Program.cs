using IdentityManager.Data;
using IdentityManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resend;
using Microsoft.AspNetCore.Identity.UI.Services;
using IdentityManager.Services;
using IdentityManager;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using IdentityManager.Authorize;
using IdentityManager.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddOptions();

builder.Services.Configure<ResendClientOptions>(options =>
{
    options.ApiToken = builder.Configuration["Resend:ApiKey"];
});

builder.Services.AddHttpClient();

builder.Services.AddTransient<IResend, ResendClient>();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddScoped<INumberOfDaysForAccount, NumberOfDaysForAccount>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Account/NoAccess");
});

//Overriding the default password settings using Identity options
builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Lockout.MaxFailedAccessAttempts = 3;
    opt.SignIn.RequireConfirmedEmail = false;
    //opt.Password.RequiredLength = 8;
    //opt.Password.RequireUppercase = true;
    //opt.User.RequireUniqueEmail = false;
});

//Defining the authorization policies for the application
builder.Services.AddAuthorization(options =>
{
    //This is the policy for the admin role. Only users with the admin role can access the resources that require this policy.
    options.AddPolicy("Admin", policy => policy.RequireRole(SD.Admin));

    options.AddPolicy("AdminAndUser", policy => policy.RequireRole(SD.Admin).RequireRole(SD.User));
    options.AddPolicy("AdminRole_CreateClaim", policy => policy.RequireRole(SD.Admin).RequireClaim("Create", "True"));
    options.AddPolicy("AdminRole_CreateEditDeleteClaim", policy => policy
                .RequireRole(SD.Admin)
                .RequireClaim("Create", "True")
                .RequireClaim("Edit", "True")
                .RequireClaim("delete", "True")
            );

    //Below The Func Type is used with policy based authorization
    options.AddPolicy("AdminRole_CreateEditDeleteClaim_OR_SuperAdminRole", policy => policy.RequireAssertion(context =>
            AdminRole_CreateEditDeleteClaim_OR_SuperAdminRole(context)
    ));
    options.AddPolicy("OnlySuperAdminChecker", policy => policy.Requirements.Add(new OnlySuperAdminChecker()));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

bool AdminRole_CreateEditDeleteClaim_OR_SuperAdminRole(AuthorizationHandlerContext context)
{
    return  (
                context.User.IsInRole(SD.Admin) && context.User.HasClaim(c => c.Type == "Create" && c.Value == "True")
                && context.User.HasClaim(c => c.Type == "Edit" && c.Value == "True")
                && context.User.HasClaim(c => c.Type == "Delete" && c.Value == "True")
            )       || context.User.IsInRole(SD.SuperAdmin);
}
