using MediatR;

namespace Applicationds.ChangePassword;

public class ChangePasswordCommand : IRequest
{
    public int UserId { get; set; }
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}