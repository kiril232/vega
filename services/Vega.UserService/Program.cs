using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Vega.UserService.Data;
using Vega.UserService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// keep dev painless — apply pending migrations on boot
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
