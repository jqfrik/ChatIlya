using System.Security.Claims;
using System.Text;
using Chat.Dal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Bll;

public class AuthOptions
{
    public const string ISSUER = "ChatServer"; // издатель токена
    public const string AUDIENCE = "ChatClient"; // потребитель токена
    const string KEY = "iamilyaiamsecretman";   // ключ для шифрации
    public const int LIFETIME = 300; // время жизни токена - 300 минута
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}

public static class Authorization
{
    private static readonly string _alphabet = "abcdefghijklmnopqrstuvwxyz";
    private static readonly string _symbols = "0123456789";
    private static readonly Random Rnd = new Random();
    public static IServiceCollection AddAuthorizationLocal(this IServiceCollection collection)
    {
        collection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = AuthOptions.ISSUER,
 
                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = AuthOptions.AUDIENCE,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,
 
                    // установка ключа безопасности
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });
        return collection;
    }

    public static string Encode(string password)
    {
        byte[] encData_byte = new byte[password.Length]; 
        encData_byte = Encoding.UTF8.GetBytes(password); 
        string encodedData = Convert.ToBase64String(encData_byte); 
        return encodedData;
    }

    public static string DecodePassword(string password)
    {
        UTF8Encoding encoder = new UTF8Encoding(); 
        Decoder utf8Decode = encoder.GetDecoder();
        byte[] todecode_byte = Convert.FromBase64String(password); 
        int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length); 
        char[] decoded_char = new char[charCount]; 
        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0); 
        string result = new String(decoded_char); 
        return result;
    }
    
    public static ClaimsIdentity GetIdentity(string login, string password,ChatContext context)
    {
        var hashedPassword = Encode(password);
        var user = context.Users.FirstOrDefault(x => x.Login == login && x.HashPassword == hashedPassword);
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim("GUID",user.Id.ToString() ?? string.Empty),
            };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,null);
            return claimsIdentity;
        }
 
        // если пользователя не найдено
        return null;
    }

    public static string GenerateNewPassword(int charsCount)
    {
        var passwordResult = new StringBuilder();
        for (var i = 0; i < charsCount; i++)
        {
            var newRandomChar = _alphabet[Rnd.Next(0, _alphabet.Length)];
            passwordResult.Append(newRandomChar);
        }

        return passwordResult.ToString();
    }

    public static string GenerateSmsCheck(int charsCount)
    {
        var smsCheckResult = new StringBuilder();
        for (var i = 0; i < charsCount; i++)
        {
            var newRandomChar = _symbols[Rnd.Next(0, _symbols.Length)];
            smsCheckResult.Append(newRandomChar);
        }

        return smsCheckResult.ToString();
    }
}