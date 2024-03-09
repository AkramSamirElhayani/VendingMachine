using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using VendingMachine.Api.Identity;
using VendingMachine.Api.Identity.TokenHelpers;
using VendingMachine.Api.Infrastructer;
using VendingMachine.Applicaion;
using VendingMachine.Infrastructer;
using VendingMachine.Infrastructer.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfigration) =>
{
    loggerConfigration.ReadFrom.Configuration(context.Configuration);
});
// Add services to the container.

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));


builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JwtSettings")["JwtIssuer"],
        ValidAudience = builder.Configuration.GetSection("JwtSettings")["JwtAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings")["JwtKey"]))
    };
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
             .AddApplication()
             .AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

//builder.Services.BuildServiceProvider().GetService<ApplicationIdentityDbContext>()!.Database.Migrate();
//builder.Services.BuildServiceProvider().GetService<VendingDbContext>()!.Database.Migrate();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>()!.Database.Migrate();
    scope.ServiceProvider.GetRequiredService<VendingDbContext>()!.Database.Migrate();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    await RoleInitializer.InitializeRoles(roleManager);
}



//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseMiddleware<RequstLogMiddleware>();
app.UseSerilogRequestLogging();
app.UseAuthorization();
app.UseAuthentication();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.UseCors();
app.Run();



public class RoleInitializer
{
    public static async Task InitializeRoles(RoleManager<ApplicationRole> roleManager)
    {
        string[] roleNames = { "Admin", "Buyer", "Seller" };

        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName); 
            if (!roleExist)
            {
                // Create the roles and seed them to the database
                roleResult = await roleManager.CreateAsync(new ApplicationRole(roleName));
            }
        }
    }
}