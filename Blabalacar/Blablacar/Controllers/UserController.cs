using BusinnesLayer.Interface;
using BusinnesLayer.Models.Entities.User;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blablacar.Controllers;
[Authorize]
public class UserController: Controller
{
    private readonly IUserService _userService;
    private readonly ITripService _tripService;

    public UserController(IUserService userService, ITripService tripService)
    {
        _userService = userService;
        _tripService = tripService;
    }

    [HttpGet("/Users"), AllowAnonymous]
    public Task<IEnumerable<User?>> GetUsers(CancellationToken cancellationToken = default)
        => _userService.GetAll(cancellationToken);
    
    [HttpGet("/me"), Authorize]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken=default)
    {
        var id = _userService.GetMyId();
        var user = await _userService.GetById(id, cancellationToken);
        
        return Ok(user);
    }
    
    [HttpGet("user/{id:Guid}"), AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken=default)=> Ok(await _userService.GetById(id, cancellationToken));

    
    [HttpGet("/me/trips"), Authorize]
    public async Task<IActionResult> GetMyTrips(CancellationToken cancellationToken=default)
    {
        var id = _userService.GetMyId();
        var user = await _userService.GetById(id, cancellationToken);

        return Ok(user!.UserTrips);
    }
    
    [HttpPost("me/joinToTrip")]
    public async Task<IActionResult> JoinToTrip([FromRoute]Guid tripId, CancellationToken cancellationToken=default)
    {
        var userId = _userService.GetMyId();
        
        if (!ModelState.IsValid)
            return NotFound();

        var user = await _userService.GetById(userId, cancellationToken);
        var trip = await _tripService.GetById(tripId, cancellationToken);

        user.UserTrips!.Add(trip);
        trip.UserTrips!.Add(user);

        await _userService.Save(cancellationToken);

        return Ok();
    }

    [HttpPut("admin/put_user"), Authorize(Roles="Admin")]
    public async Task<IActionResult> AdminUpdateUser([FromBody]AdminUpdateUserBody updatedUser, 
        CancellationToken cancellationToken=default)
    {
        if (!ModelState.IsValid)
            return BadRequest("Incorrect input dates");

        var currentUser = await _userService.GetById(updatedUser.Id, cancellationToken);

        _userService.AdminUpdateUser(currentUser, updatedUser);
        await _userService.Save(cancellationToken);

        return Ok(currentUser);
    }
    
    [HttpPut("me/put"), Authorize]
    public async Task<IActionResult> UpdateUser([FromBody]UpdateUserBody updatedUser,
        CancellationToken cancellationToken=default)
    {
        var userId = _userService.GetMyId();

        if (!ModelState.IsValid)
            return BadRequest("Incorrect input dates");

        var currentUser = await _userService.GetById(userId, cancellationToken);
        
        _userService.UpdateSelfUser(currentUser, updatedUser);
        await _userService.Save(cancellationToken);

        return Ok(currentUser);
    }

    [HttpDelete("admin/{userId:Guid}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminDeleteUser([FromRoute]Guid userId, CancellationToken cancellationToken=default)
    {
        var deletedUser = await _userService.GetById(userId, cancellationToken);

        await _userService.DeleteByUser(deletedUser, cancellationToken);
        await _userService.Save(cancellationToken);

        return Ok();
    }

    [HttpPut("admin/user_verification"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserVerification(
        [FromBody] UpdateUserVerificationBody updateUserVerificationBody,
        CancellationToken cancellationToken = default)
    {
        await _userService.UpdateUserVerification(updateUserVerificationBody, cancellationToken);
        
        return Ok();
    }
}