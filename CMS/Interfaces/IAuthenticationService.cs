using CMS.Dtos;

namespace CMS.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Register(RegisterRequestDto request);
        Task<string> Login(LoginRequestDto requst);
    }
}
