using Application.DTOs.User;
using MediatR;

namespace Application.Queries.GetProfile;

public class GetProfileQuery : IRequest<UserProfileDto>
{
    public int UserId { get; set; }
}