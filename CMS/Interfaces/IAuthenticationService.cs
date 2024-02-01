using CMS.Dtos;
using FluentResults;

namespace CMS.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<string>> Register(RegisterRequestDto request);
        Task<Result<string>> Login(LoginRequestDto requst);
        Task<Result<string>> SocialLogin(SocialLoginRequest request);
    }
}
