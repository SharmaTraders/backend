using CommandContracts.authentication;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.authentication;

public class LoginAdminHandler : IRequestHandler<LoginAdminCommand.Request, LoginAdminCommand.Response> {
    private readonly IAdminRepository _adminRepository;


    public LoginAdminHandler(IAdminRepository adminRepository) {
        _adminRepository = adminRepository;
    }

    public async Task<LoginAdminCommand.Response> Handle(LoginAdminCommand.Request request, CancellationToken cancellationToken) {
        AdminEntity adminEntity = new AdminEntity() {
            Email = request.Email,
            Password = request.Password
        };

        AdminEntity? adminFromDb = await _adminRepository.GetByEmailAsync(adminEntity.Email);
        if (adminFromDb is null) {
            throw new DomainValidationException("Email", ErrorCode.NotFound, ErrorMessages.EmailDoesntExist);
        }

        bool doesPasswordMatch = adminFromDb.DoesPasswordMatch(request.Password);

        if (!doesPasswordMatch) {
            throw new DomainValidationException("Password", ErrorCode.BadRequest, ErrorMessages.PasswordIncorrect);
        }

        return new LoginAdminCommand.Response(adminFromDb.Email, "Admin");
    }

}