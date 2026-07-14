// Application/Commands/UpdateProfile/UpdateProfileHandler.cs
using Application.DTOs.User;
using Application.Interfaces;
using MediatR;
using Domain.Exceptions;


namespace Application.Commands.UpdateProfile;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, UserProfileDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserProfileDto> Handle(UpdateProfileCommand request, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, ct);
        if (user == null)
            throw new NotFoundException("Пользователь не найден");

        // Проверка нового email
        if (request.Email != null && request.Email != user.Email)
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email, ct))
                throw new InvalidOperationException("Email уже используется");
        }

        // Проверка нового username
        if (request.UserName != null && request.UserName != user.UserName)
        {
            if (await _userRepository.ExistsByUserNameAsync(request.UserName, ct))
                throw new InvalidOperationException("Имя пользователя уже занято");
        }

        user.UpdateProfile(request.UserName, request.Email);
        await _userRepository.UpdateAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return new UserProfileDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            CreatedAt = user.CreatedAt,
            TasksCount = user.Tasks?.Count ?? 0
        };
    }
}