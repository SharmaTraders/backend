using CommandContracts.authentication;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.authentication;

public class RegisterAdminHandler : IRequestHandler<RegisterAdminCommand.Request>{

    private readonly IAdminRepository _adminRepository;
    private readonly IUnitOfWork _unitOfWork;


    public RegisterAdminHandler(IAdminRepository adminRepository, IUnitOfWork unitOfWork) {
        _adminRepository = adminRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RegisterAdminCommand.Request request, CancellationToken cancellationToken) {
        AdminEntity? adminFromDb = await _adminRepository.GetByEmailAsync(request.Email);
        if (adminFromDb is not null) {
            throw new DomainValidationException("Email",ErrorCode.Conflict, ErrorMessages.EmailAlreadyExists);
        }

        AdminEntity adminEntity = new AdminEntity() {
            Email = request.Email,
            Password = request.Password
        };

        await _adminRepository.AddAsync(adminEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

    }
}