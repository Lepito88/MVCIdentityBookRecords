using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MVCIdentityBookRecords.Data;
using MVCIdentityBookRecords.Interfaces;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Configuration;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection").ToString();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
    //options.UseSqlServer(connectionString));
    o => o.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, entity) => $"{schema ?? "dbo"}_{entity}")
    )
    );

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();


builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        
        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";

    })
    .AddJwtBearer(options => 
    {
        //options.Audience = builder.Configuration["JWT:ValidAudience"];
        //options.Authority = builder.Configuration["JWT:ValidIssuer"];
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            //ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });
//builder.Services.AddAuthentication().AddIdentityCookies();
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = null;
//});

////Jwt bearer options
//.AddJwtBearer(options => {
//options.SaveToken = true;
//options.RequireHttpsMetadata = false;
//options.TokenValidationParameters = new TokenValidationParameters()
//{
//    ValidateIssuer = true,
//    //ValidateAudience = true,
//    ValidAudience = builder.Configuration["JWT:ValidAudience"],
//    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
//    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
//};
//});
builder.Services.AddAuthorization();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();
//builder.Services.AddScoped<ILogger>();
//builder.Services.AddTransient<ITokenService, TokenService>();
//builder.Services.AddTransient<ILoginService, LoginService>();
//builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<IAuthorService, AuthorService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddTransient<IEntityRelationShipManagerService, EntityRelationShipManagerService>();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMigrationsEndPoint();
    using var scope = app.Services.CreateScope();
    //var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

    //try
    //{
    //    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    //    await ContextSeed.SeedRolesAsync(userManager, roleManager);
    //    await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine(ex);
    //    logger.LogError(ex, "An error occurred seeding the DB.");
    //}
    app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
        string.Join("\n", endpointSources.SelectMany(source => source.Endpoints))
        
        
        );



}
else
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

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


app.Run();
