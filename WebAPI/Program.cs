//using B;
using BOs;
using DAOs;
using Microsoft.EntityFrameworkCore;
using Repos.Implements;
using Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Repos.Response;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers()
.AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
 });
//builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICageService, CageService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IWorkService, WorkService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IUserShiftService, UserShiftService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<IHealthReportService, HealthReportService>();

builder.Services.AddControllers().AddOData(opt => 
{
    opt.EnableQueryFeatures();
    opt.Filter().OrderBy().Expand().Select().Count().SetMaxTop(100);
    opt.AddRouteComponents("odata", GetEdmModel()); 
});

//Add DbContext configuration
builder.Services.AddDbContext<PiggeryManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4176", "http://localhost:3000") // Add your allowed front-end URLs here
                  .AllowAnyHeader()    // Allow any headers
                  .AllowAnyMethod()    // Allow all HTTP methods
                  .AllowCredentials(); // Allow credentials
        });
});


// Load JWT settings from appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];
var key = Encoding.ASCII.GetBytes(secretKey);

// Configure JWT Bearer authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
})
.AddCookie()
.AddGoogle("Google", googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["GoogleKeys:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"];
    googleOptions.SaveTokens = true;
    googleOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});

static IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();
    odataBuilder.EntitySet<Cage>("Cage");
    odataBuilder.EntitySet<Animal>("Animal"); 
    odataBuilder.EntitySet<Work>("Work");
    odataBuilder.EntitySet<WorkResponse>("Works");
    return odataBuilder.GetEdmModel();
}
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Add JWT Bearer authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter your token in the format **Bearer {your token}**",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

// Swagger and Swagger UI in Development
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseRouting();

app.Run();

