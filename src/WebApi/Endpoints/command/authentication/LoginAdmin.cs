using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CommandContracts.authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Endpoints.command.authentication;

public class LoginAdmin : CommandEndPointBase.WithRequest<LoginAdminRequest>
    .WithResponse<LoginAdminResponse> {

    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public LoginAdmin(IMediator mediator, IConfiguration configuration) {
        _mediator = mediator;
        _configuration = configuration;
    }


    [HttpPost, Route("auth/login/admin")]
    public override async Task<ActionResult<LoginAdminResponse>> HandleAsync(LoginAdminRequest request) {
        LoginAdminCommand.Request commandRequest = new(request.RequestBody.Email, request.RequestBody.Password);
        var response = await _mediator.Send(commandRequest);

        string jwtToken = GenerateJwt(response.Email, response.Role);
        return Ok(new LoginAdminResponse {JwtToken =jwtToken});
    }


    private string GenerateJwt(string email, string role) {
        List<Claim> claims = GenerateClaims(email, role);

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

    private List<Claim> GenerateClaims(string email, string role) {
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["JWT:Subject"]!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, role)
        };
        return claims.ToList();
    }
}



public class LoginAdminRequest {
    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(string Email, string Password);
}

public class LoginAdminResponse {
    public string JwtToken { get; set; } = null!;
}