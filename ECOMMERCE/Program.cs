using DataAccessLayer.DataContext;
using DataAccessLayer.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag.Generation.Processors.Security;
using NSwag;
using System.Text;
using DataAccessLayer.Services.Interfaces;
using DataAccessLayer.Services;

var builder = WebApplication.CreateBuilder(args);

//Adding identity in out services container
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                 .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//Adding spa
builder.Services.AddSpaStaticFiles(options =>
{
    options.RootPath = "ClientApp/dist";
});
//Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Http Configuration
builder.Services.AddHttpContextAccessor();

//Adding DependencuyInjection
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

//swagger connection
builder.Services.AddOpenApiDocument(options =>
{
    options.Title= "ECommerce test Api";
    //This is for adding icon on the swagger

    options.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Type: Bearer {Your Jwt token}"
    });
    options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});
//Jwt Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenDefination:JwtKey"])),

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["TokenDefination:JwtIssuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["TokenDefination:JwtAudience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
    };
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

app.UseCors(cors =>
{
    cors.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
});

app.UseRouting();

//SwaggerConnection
app.UseOpenApi();
app.UseSwaggerUi3(options =>
{
    options.Path = "/api";
});

app.UseStaticFiles();


app.UseAuthentication();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("Default", "{controller}/{action=Index}/{id?}");
});
app.UseSpa(options =>
{
    options.Options.SourcePath = "ClientApp";
    options.UseProxyToSpaDevelopmentServer("http://localhost:4200/");
    
});

app.Run();
