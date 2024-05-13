using MediatR;

namespace CommandContracts.authentication;

public static class LoginAdminCommand {

    public record Request(
        string Email,
        string Password) : IRequest<Response>; 

    public record Response(string Email, string Role);

    
}