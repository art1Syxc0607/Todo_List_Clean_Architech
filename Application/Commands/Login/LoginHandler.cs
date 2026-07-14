// Application/Commands/Login/LoginHandler.cs
using Application.DTOs.User;
using Application.Interfaces;
using MediatR;

namespace Application.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken ct)
    {
        // 1. Поиск пользователя
        var user = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (user == null)
            throw new UnauthorizedAccessException("Неверный email или пароль");

        // 2. Проверка пароля
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Неверный email или пароль");

        // 3. Генерация токена
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