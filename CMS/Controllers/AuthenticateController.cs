using CMS.Dtos;
using CMS.Extensions;
using CMS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticateController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticateController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultDto<string>))]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authenticationService.Login(request);

        var resultDto = result.ToResultDto();

        if(!resultDto.IsSuccess)
        {
            return BadRequest(resultDto);
        }

        return Ok(resultDto);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultDto<string>))]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var result = await _authenticationService.Register(request);

        var resultDto = result.ToResultDto();

        if (!resultDto.IsSuccess)
        {
            return BadRequest(resultDto);
        }
        return Ok(resultDto);
    }

    [AllowAnonymous]
    [HttpPost("social-login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<string>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultDto<string>))]
    public async Task<IActionResult> SocialLogin([FromBody] SocialLoginRequest request)
    {
        var result = await _authenticationService.SocialLogin(request);

        var resultDto = result.ToResultDto();

        if (!resultDto.IsSuccess)
        {
            return BadRequest(resultDto);
        }
        return Ok(resultDto);
    }
}
