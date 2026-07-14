using Application.Commands.Register;
using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken ct)
    {
        // 1. Проверка email
        if (await _userRepository.ExistsByEmailAsync(request.Email, ct))
            throw new InvalidOperationException("Email уже используется");

        // 2. Проверка username
        if (await _userRepository.ExistsByUserNameAsync(request.UserName, ct))
            throw new InvalidOperationException("Имя пользователя уже занято");

        // 3. Хеширование пароля
        var passwordHash = _passwordHasher.HashPassword(request.Password);

        // 4. Создание пользователя
        var user = new User(request.Email, request.UserName, passwordHash);
        await _userRepository.AddAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        // 5. Генерация токена
        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email,
            UserName = user.UserName
        };
    }
}