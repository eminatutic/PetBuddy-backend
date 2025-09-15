using api.Models;

namespace api.Token
{
    public interface IToken
    {
        Task<string> CreateTokenAsync(User user);
    }
}