using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TodoApi.Utils
{
    public class AuthOptions
    {
        public const string ISSUER = "TodoApiServer"; // издатель токена
        public const string AUDIENCE = "TodoApiClient"; // потребитель токена
        const string KEY = "todo_api_secret_key_qwerty_12345";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
