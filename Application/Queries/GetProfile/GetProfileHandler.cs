using Application.DTOs.User;
using Application.Interfaces;
using MediatR;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Queries.GetProfile;

public class GetProfileHandler : IRequestHandler<GetProfileQuery, UserProfileDto>
{
    private readonly IUserRepository _userRepository;

    public GetProfileHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, ct);
        if (user == null)
            throw new DomainException("Пользователь не найден");

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