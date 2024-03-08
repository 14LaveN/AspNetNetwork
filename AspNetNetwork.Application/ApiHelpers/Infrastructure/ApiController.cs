using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AspNetNetwork.Application.ApiHelpers.Contracts;
using AspNetNetwork.Application.ApiHelpers.Policy;
using AspNetNetwork.Application.Core.Helpers.JWT;
using AspNetNetwork.Database.Identity.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Primitives;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Core.Primitives.Result;
using AspNetNetwork.Domain.Identity.Entities;

namespace AspNetNetwork.Application.ApiHelpers.Infrastructure;

/// <summary>
/// Represents the api controller class.
/// </summary>
[ApiController]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class ApiController : ControllerBase
{
    protected ApiController(
        ISender sender,
        IUserRepository userRepository, string controllerName)
    {
        UserId = GetUserId().GetAwaiter().GetResult();
        Sender = sender;
        UserRepository = userRepository;
        ControllerName = controllerName;
    }

    protected string ControllerName { get; }

    protected ISender Sender { get; }

    protected Maybe<Guid> UserId { get; }

    protected IUserRepository UserRepository { get; }

    protected string? Token => 
        ControllerName is "UsersController" ? "exceptToken" :
            Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

    /// <summary>
    /// Get name
    /// </summary>
    /// <returns>Base information about get name method</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">Return name from token.</response>
    /// <response code="400"></response>
    /// <response code="500">Internal server error</response>
    ///
    /// 
    [HttpGet("get-name")]
    public  string GetName()
    {
        var name = BaseRetryPolicy.Policy.Execute(() =>
            GetClaimByJwtToken.GetNameByToken(Token));
        
        ArgumentException
            .ThrowIfNullOrEmpty(
                name,
                nameof(name));
        
        return name;
    }

    /// <summary>
    /// Get profile
    /// </summary>
    /// <returns>Base information about get pfoile method</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">Return app user</response>
    /// <response code="400"></response>
    /// <response code="500">Internal server error</response>
    ///
    
    [HttpGet("get-profile")]
    public async Task<Maybe<User>> GetProfile()
    {
        var name = GetName();
        var profile = await BaseRetryPolicy.Policy.Execute(async () =>
            await UserRepository.GetByNameAsync(name));

        return profile;
    }

    [HttpGet("get-userId")]
    public async Task<Maybe<Guid>> GetUserId()
    {
        string name = GetName();
        var profile = await BaseRetryPolicy.Policy.Execute(async () =>
            await UserRepository.GetByNameAsync(name));

        return profile.Value.Id;
    }

    [HttpGet("get-profile-by-id")]
    public async Task<Maybe<User>> GetProfileById(Guid authorId)
    {
        var profile = 
            await UserRepository.GetByIdAsync(authorId);

        return profile;
    }
        
    /// <summary>
    /// Creates an <see cref="BadRequestObjectResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/>.
    /// response based on the specified <see cref="Result"/>.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>The created <see cref="BadRequestObjectResult"/> for the response.</returns>
    protected IActionResult BadRequest(Error error) => BadRequest(new ApiErrorResponse(new[] { error }));
    
    /// <summary>
    /// Creates an <see cref="BadRequestObjectResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/>.
    /// response based on the specified <see cref="Result"/>.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>The created <see cref="BadRequestObjectResult"/> for the response.</returns>
    protected IActionResult Unauthorized(Error error) => Unauthorized(new ApiErrorResponse(new[] { error }));

    /// <summary>
    /// Creates an <see cref="OkObjectResult"/> that produces a <see cref="StatusCodes.Status200OK"/>.
    /// </summary>
    /// <returns>The created <see cref="OkObjectResult"/> for the response.</returns>
    /// <returns></returns>
    protected new IActionResult Ok(object value) => base.Ok(value);

    /// <summary>
    /// Creates an <see cref="NotFoundResult"/> that produces a <see cref="StatusCodes.Status404NotFound"/>.
    /// </summary>
    /// <returns>The created <see cref="NotFoundResult"/> for the response.</returns>
    protected new IActionResult NotFound() => base.NotFound();
}