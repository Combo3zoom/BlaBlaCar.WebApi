using BusinnesLayer.Interface;
using BusinnesLayer.Models.Entities.Trip;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blablacar.Controllers;

[Authorize]
public class TripController : Controller
{
    private readonly ITripService _tripService;
    private readonly IUserService _userService;

    public TripController(ITripService tripService, IUserService userService)
    {
        _tripService = tripService;
        _userService = userService;
    }

    [HttpGet("trips"), AllowAnonymous]
    public Task<IEnumerable<Trip?>> GetTrips(CancellationToken cancellationToken = default)
        => _tripService.GetTrips(cancellationToken);

    [HttpGet("trip/{id:Guid}"), AllowAnonymous]
    public async Task<IActionResult> GetTripById([FromRoute]Guid id, CancellationToken cancellationToken=default)
        => Ok(await _tripService.GetById(id, cancellationToken));

    [HttpPost("trip/create"), Authorize]
    public async Task<IActionResult> CreateTrip([FromBody]CreateTripBody createTripBody,
        CancellationToken cancellationToken=default)
    {
        if (!ModelState.IsValid)
            return NotFound();
        
        var currentUserId = _userService.GetMyId();
        var currentUser = await _userService.GetById(currentUserId, cancellationToken);

        var trip = await _tripService.CreateTrip(currentUser, createTripBody, cancellationToken);
        await _tripService.Save(cancellationToken);
        
        return CreatedAtAction(nameof(GetTrips), new {id = trip.Id}, trip);
    }

    [HttpPut("trip/me/update")]
    public async Task<IActionResult> UpdateTrip([FromBody]UpdateTripBody newTrip, CancellationToken cancellationToken=default)
    {
        var userId = _userService.GetMyId();
        
        if (!ModelState.IsValid)
            return BadRequest();
        
        var currentTrip = await  _tripService.GetById(newTrip.Id, cancellationToken);
        _tripService.DoesUserHasTrip(currentTrip, userId);
        
        await _tripService.Save(cancellationToken);
        
        return Ok(currentTrip);
    }

    [HttpDelete("trip/me/delete/{id:Guid}")]
    public async Task<IActionResult> DeleteTrip([FromRoute]Guid id, CancellationToken cancellationToken=default)
    {
        var userId = _userService.GetMyId();
        var trip = await _tripService.GetById(id, cancellationToken);
        _tripService.DoesUserHasTrip(trip, userId);
        
        await _tripService.DeleteTrip(trip, cancellationToken);
        await _tripService.Save(cancellationToken);
        
        return Ok();
    }

    [HttpGet("trip/found_by_start_and_end_points/{startRoute}/{endRoute}")]
    public async Task<IEnumerable<Trip?>> FoundTrip([FromRoute]FoundTripBody foundTripBody, CancellationToken cancellationToken = default)
    => await _tripService.FoundTripByStartAndEndPoint(foundTripBody, cancellationToken);

}   