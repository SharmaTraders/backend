using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dto;
using Microsoft.IdentityModel.Tokens;

namespace IntegrationTests.util;

public static class JwtUtil {
    

    public static string GenerateJwt(UserDto userDto) {
        List<Claim> claims = GenerateClaims(userDto);

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is the custom Secret Key for Development , this will be changed in production123"));
        SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        JwtHeader header = new JwtHeader(signingCredentials);

        JwtPayload payload = new JwtPayload(
            "JWTAuthenticationServer",
            "web-client"
            ,
            claims,
            null,
            DateTime.Now.AddHours(2)); // Todo - think about expiration time

        JwtSecurityToken token = new JwtSecurityToken(header, payload);
        string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return serializedToken;
    }

    private static List<Claim> GenerateClaims(UserDto userDto) {
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, "JWTServiceAccessToken"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Name, userDto.Email),
            new Claim(ClaimTypes.Role, userDto.Role)
        };
        return claims.ToList();
    }
}