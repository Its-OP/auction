using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using backend.ApiContracts;
using domain;
using domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController: ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public UsersController(IApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    [HttpPost]
    [Route("signUp")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SignUpErrorCodes))]
    public async Task<IActionResult> SignUp([FromBody] SignUpArguments signUpArguments, CancellationToken token)
    {
        if (await _context.Users.AnyAsync(x => x.Username == signUpArguments.Username, token))
            return BadRequest(SignUpErrorCodes.UserAlreadyExists);
        
        if (signUpArguments.Password.Length < 6)
            return BadRequest(SignUpErrorCodes.PasswordTooSimple);
        
        if (signUpArguments.Password.Length > 16)
            return BadRequest(SignUpErrorCodes.PasswordTooLong);
        
        var user = new User(signUpArguments.Username, await GetPasswordHash(signUpArguments.Password, token));

        await _context.Users.AddAsync(user, token);
        await _context.SaveChangesAsync(token);

        return SignInInternal(user);
    }
    
    [HttpPost]
    [Route("signIn")]
    public async Task<IActionResult> SignIn([FromBody] SignInArguments signInArguments, CancellationToken token)
    {
        var passwordHash = await GetPasswordHash(signInArguments.Password, token);
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == signInArguments.Username
                                                                  && x.PasswordHash == passwordHash, token);
        
        if (user is null)
            return Unauthorized();

        return SignInInternal(user);
    }

    private IActionResult SignInInternal(User user)
    {
        var secureKey = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        if (string.IsNullOrEmpty(secureKey) || string.IsNullOrEmpty(issuer))
            return BadRequest();
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Username)
        };

        var jwtToken = new JwtSecurityToken(issuer,
            issuer,
            claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var jwtString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return Ok(new { Token = jwtString });
    }

    private async Task<string> GetPasswordHash(string password, CancellationToken token)
    {
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        using var stream = new MemoryStream(passwordBytes);

        return Convert.ToHexString(await sha256.ComputeHashAsync(stream, token));
    }
}