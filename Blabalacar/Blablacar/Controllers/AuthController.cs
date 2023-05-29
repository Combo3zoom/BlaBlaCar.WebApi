using BusinnesLayer.Interface;
using BusinnesLayer.Models.Entities;
using BusinnesLayer.Models.Entities.User;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blablacar.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly IUserService _userService;
    private readonly IRegisterUserService _registerUserService;

    public AuthController(IRegisterUserService registerUserService, IUserService userService)
    {
        _registerUserService = registerUserService;
        _userService = userService;
    }

    [HttpGet, Authorize]
    public ActionResult<string> GetMyId()
    {
        _registerUserService.GetId();
        return Ok();
    }
    [HttpGet("logout/")]
    public async Task<IActionResult> Logout()
    {
        await _registerUserService.Logout();
        return Ok();
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterUserDto request)
    {
        var newUser = await _registerUserService.Register(request);
        return CreatedAtAction(nameof(GetMyId),new{id=newUser.Id}, newUser);
    } 

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login(RegisterUserDto request,
        CancellationToken cancellationToken=default)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var user = await _userService.GetByName(request.Name!, cancellationToken);
        await _registerUserService.Login(user, request, cancellationToken);

        // var accessToken = _registerUserService.CreateAccessToken(user);
        // _registerUserService.SetRefreshToken(user);
        
        await _userService.Save(cancellationToken);

        // _registerUserService.SetCacheRefreshToken(user.Id, user.RefreshToken);

        // var tokens = new TokenResponse(accessToken, user.RefreshToken);
        var roles = await _registerUserService.GetRoles(user);
        var tokens = new TokenResponse(roles.FirstOrDefault()!);
        return Ok((tokens));
    }
    

    // [HttpPost("refresh-token"),Authorize]
    // public async Task<ActionResult<TokenResponse>> RefreshToken([FromQuery] string refreshToken,
    //     CancellationToken cancellationToken = default)
    // {
    //     var currentUserId = _userService.GetMyId();
    //     var currentUser = await _registerUserService.GetValueFromCache(currentUserId, cancellationToken);
    //
    //     _registerUserService.IsCorrectRefreshToken(currentUser!, refreshToken);
    //     _registerUserService.SetRefreshToken(currentUser);
    //     await _userService.Save(cancellationToken);
    //
    //     _registerUserService.SetCacheRefreshToken(currentUserId, currentUser!.RefreshToken);
    //
    //     var accessToken = _registerUserService.CreateAccessToken(currentUser);
    //     var tokens = new TokenResponse(accessToken, refreshToken);
    //     
    //     return Ok(tokens);
    // }

} 