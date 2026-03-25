using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using pr.Data;
using pr.Models;
using Microsoft.AspNetCore.Authorization;
using pr.Models.Auth;

namespace AuthAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config) {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterDto request) {
        if(await _context.Users.AnyAsync(u => u.Email == request.Email)) {
            return BadRequest(new {error = "этот Email занят"});
        }
   
        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponse
        { 
            Token = token,
            RefreshToken = refreshToken,
            Email = user.Email,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        });    
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginDto request) {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) {
            return BadRequest(new { error = "Неверный email или пароль"});
        }

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Email = user.Email,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        });
    }

    [HttpGet("health")]
    public async Task<IActionResult> Health() {    
        return Ok(new {
            status = "ok",
            time = DateTime.UtcNow,
            server = "Сервер работает"
        });
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult> GetUserProfile() {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

        var user = await _context.Users
            .Where(u => u.Id == int.Parse(userIdClaim))
            .Select(u => new
            {
                u.Email,
                u.CreatedAt
            }).FirstOrDefaultAsync();

        if (user==null) return NotFound();

        return Ok(user);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> RefreshToken(RefreshTokenDto request) {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
        if (user == null || user.RefreshTokenExpiry <= DateTime.UtcNow) {
            return Unauthorized( new { error = "Сессия истекла"});
        }
        var newAccessToken = GenerateJwtToken(user);
        var NewRefreshToken = GenerateRefreshToken();

        user.RefreshToken = NewRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        await _context.SaveChangesAsync();

        return Ok(new AuthResponse {
            Token = newAccessToken,
            RefreshToken = NewRefreshToken,
            Email = user.Email,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        });
    } 

    private string GenerateJwtToken(User user){
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt:key"]!));  
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["jwt:Issuer"],
            audience: _config["jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken() {
        var RandNumber = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(RandNumber);

        return Convert.ToBase64String(RandNumber);
    }
}