using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GuildManager.Data;
using GuildManager.Models;
using GuildManager.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GuildManager.Services;

public class AuthenticationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
}

public class AuthenticationRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public interface IAuthenticationService
{
    AuthenticationResponse Authenticate(AuthenticationRequest request);
    AuthenticationResponse Register(AuthenticationRequest request);
    void AttachUserToContext(HttpContext context, string tokenString);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly AppSettings _appSettings;
    private readonly GMContext _context;

    public AuthenticationService(IOptions<AppSettings> appSettings, GMContext context)
    {
        _appSettings = appSettings.Value;
        _context = context;
    }

    public AuthenticationResponse Authenticate(AuthenticationRequest request)
    {
        var player = _context.Players.SingleOrDefault(x => x.Username == request.Username);
        if (player == null)
            return new AuthenticationResponse { Success = false, Message = "Username or password is incorrect" };

        var passwordHash = new PasswordHash(player.PasswordHash);
        if (!passwordHash.Verify(request.Password))
            return new AuthenticationResponse { Success = false, Message = "Username or password is incorrect" };

        // authentication successful so generate jwt token
        var token = generateJwtToken(player);

        return new AuthenticationResponse { Success = true, Token = token };
    }

    /// <summary>
    /// Registers a new Player with the given credentials.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public AuthenticationResponse Register(AuthenticationRequest request)
    {
        if (_context.Players.Any(x => x.Username == request.Username))
            return new AuthenticationResponse
                { Success = false, Message = "Playername \"" + request.Username + "\" is already taken" };

        var passwordHash = new PasswordHash(request.Password);
        var player = new Player
        {
            Username = request.Username,
            PasswordHash = passwordHash.ToArray()
        };

        _context.Players.Add(player);
        _context.SaveChanges();

        return Authenticate(request);
    }

    // helper methods

    private string generateJwtToken(Player Player)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", Player.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken? ValidateJwtToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
        catch
        {
            return null;
        }
    }

    public void AttachUserToContext(HttpContext context, string tokenString)
    {
        try
        {
            var jwtToken = ValidateJwtToken(tokenString);
            if (jwtToken == null)
                return;

            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            // attach user to context on successful jwt validation
            context.Items["Player"] = _context.Players.Find(userId);
        }
        catch
        {
            // do nothing if jwt validation fails
            // user is not attached to context so request won't have access to secure routes
        }
    }
}
