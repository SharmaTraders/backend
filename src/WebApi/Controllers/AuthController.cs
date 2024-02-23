using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.auth;
using Dto;
using Dto.tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IConfiguration configuration, IAuthenticationDomain authDomain) : ControllerBase {
    [HttpPost, Route("login/admin")]
    public async Task<ActionResult<LoginResponseDto>> AdminLogin(LoginRequestDto loginRequest) {
        try {
            UserDto userDto = await authDomain.ValidateAdmin(loginRequest);
            string token = GenerateJwt(userDto);
            LoginResponseDto responseDto = new LoginResponseDto(token);
            return Ok(responseDto);
        }
        catch (ExceptionWithErrorCode e) {
            return StatusCode((int) e.ErrorCode, e.Message);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost, Route("register/admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> RegisterAdmin(RegisterAdminRequestDto registerAdminRequest) {
        try {
            await authDomain.RegisterAdmin(registerAdminRequest);
            return Ok();
        }
        catch (ExceptionWithErrorCode e) {
            return StatusCode((int) e.ErrorCode, e.Message);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }


    private string GenerateJwt(UserDto userDto) {
        List<Claim> claims = GenerateClaims(userDto);

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]!));
        SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        JwtHeader header = new JwtHeader(signingCredentials);

        JwtPayload payload = new JwtPayload(
            configuration["JWT:Issuer"],
            configuration["JWT:Audience"],
            claims,
            null,
            DateTime.Now.AddHours(2)); // Todo - think about expiration time

        JwtSecurityToken token = new JwtSecurityToken(header, payload);
        string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return serializedToken;
    }

    private List<Claim> GenerateClaims(UserDto userDto) {
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, configuration["JWT:Subject"]!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Name, userDto.Email),
            new Claim(ClaimTypes.Role, userDto.Role)
        };
        return claims.ToList();
    }
}