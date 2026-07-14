using Application.DTOs.User;
using MediatR;

namespace Application.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<UserProfileDto>
{
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
}