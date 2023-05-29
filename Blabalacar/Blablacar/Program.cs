using System.Text;
using Blablacar;
using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.TryAdd(ServiceDescriptor
    .Singleton<IMemoryCache, MemoryCache>());
builder.Services.AddHttpContextAccessor();

builder.Services.AddBusinessServices();
builder.Services.AddDataLayerServices();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standart Authorization header using the Bearer scheme(\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) 
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey =
//                 new SymmetricSecurityKey(Encoding.UTF8
//                     .GetBytes(builder.Configuration
//                         .GetSection("JWT:Key").Value!)),
//             ValidateIssuer = false,
//             ValidateAudience = false
//         };
//     });

builder.Services.AddIdentity<User, ApplicationRole>()
    .AddEntityFrameworkStores<BlablacarContext>();

builder.Services.AddDbContext<BlablacarContext>
    (options => options
        .UseNpgsql(builder.Configuration
            .GetConnectionString("Connection")));

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.HttpOnly = false;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});
builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    }));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NgOrigins");

app.UseAuthentication(); 
app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
    if (!await roleManager!.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new ApplicationRole("Admin"));
    if (!await roleManager.RoleExistsAsync("Users"))
        await roleManager.CreateAsync(new ApplicationRole("Users"));
    
    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
    var admin = await userManager!.FindByNameAsync("Admin");
    if (admin is null)
    {
        admin = new User { Id = new Guid(), Name = "Admin", UserName = "Admin" };
        await userManager.CreateAsync(admin, "Admin@2");
    }
    if (!await userManager.IsInRoleAsync(admin, "Admin"))
    {
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}

app.MapControllers();

app.Run();




