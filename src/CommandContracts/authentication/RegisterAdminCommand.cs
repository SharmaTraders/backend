using MediatR;

namespace CommandContracts.authentication;

public static class RegisterAdminCommand {
    public record Request(string Email, string Password) : IRequest;

}