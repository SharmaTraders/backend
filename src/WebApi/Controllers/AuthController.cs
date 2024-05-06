using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.auth;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase {
    private readonly IConfiguration _configuration;
    private readonly IAuthenticationDomain _authDomain;

    public AuthController(IConfiguration configuration, IAuthenticationDomain authDomain) {
        _configuration = configuration;
        _authDomain = authDomain;
    }

    [HttpPost, Route("login/admin")]
    public async Task<ActionResult<LoginResponse>> AdminLogin(LoginRequest loginRequest) {
        UserDto userDto = await _authDomain.ValidateAdmin(loginRequest);
        string token = GenerateJwt(userDto);
        LoginResponse response = new LoginResponse(token);
        return Ok(response);
    }

    [HttpPost, Route("register/admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> RegisterAdmin(RegisterAdminRequest registerAdminRequest) {
        await _authDomain.RegisterAdmin(registerAdminRequest);
        return Ok();
    }


    private string GenerateJwt(UserDto userDto) {
        List<Claim> claims = GenerateClaims(userDto);

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));
        SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        JwtHeader header = new JwtHeader(signingCredentials);

        JwtPayload payload = new JwtPayload(
            _configuration["JWT:Issuer"],
            _configuration["JWT:Audience"],
            claims,
            null,
            DateTime.Now.AddHours(24)); // Todo - think about expiration time

        JwtSecurityToken token = new JwtSecurityToken(header, payload);
        string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return serializedToken;
    }

    private List<Claim> GenerateClaims(UserDto userDto) {
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["JWT:Subject"]!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Name, userDto.Email),
            new Claim(ClaimTypes.Role, userDto.Role)
        };
        return claims.ToList();
    }
}