using System.Security.Cryptography;
using System.Text;
using backend.ApiContracts;
using domain;
using domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController: ControllerBase
{
    private readonly IApplicationDbContext _context;

    public UsersController(IApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateUser([FromBody] UserArguments userArguments, CancellationToken token)
    {
        if (await _context.Users.AnyAsync(x => x.Username == userArguments.Username, token))
            return BadRequest(ErrorCodes.UserAlreadyExists);
        
        if (userArguments.Password.Length < 6)
            return BadRequest(ErrorCodes.PasswordTooSimple);
        
        if (userArguments.Password.Length > 16)
            return BadRequest(ErrorCodes.PasswordTooLong);
        
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(userArguments.Password);
        using var stream = new MemoryStream(passwordBytes);
        var user = new User(userArguments.Username, Convert.ToHexString(await sha256.ComputeHashAsync(stream, token)));

        await _context.Users.AddAsync(user, token);
        await _context.SaveChangesAsync(token);

        return Ok();
    }
}