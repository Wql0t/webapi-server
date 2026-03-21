using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using pr.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(connectionString));


var jwtkey = builder.Configuration["Jwt:Key"];

var keyb = Encoding.UTF8.GetBytes(jwtkey);

var seckey = new SymmetricSecurityKey(keyb);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

        options.TokenValidationParameters = new TokenValidationParameters
        {

            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            

            ValidateLifetime = true,
            

            ValidateIssuerSigningKey = true,
            

            IssuerSigningKey = seckey
        };
    });


builder.Services.AddCors(options =>
{options.AddPolicy("ReactApp", policy =>
{
    policy.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbContext.Database.EnsureCreated();
}
app.UseMiddleware<pr.Middleware.ExMiddleware>();
if (app.Environment.IsDevelopment()) {
   app.UseSwagger();

    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("ReactApp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();