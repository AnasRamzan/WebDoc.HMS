using WebDoc.HMS.Models;

namespace WebDoc.HMS.Interfaces
{
    public interface ILoginService
    {
        Task<User> LoginUserAsync(LoginModel loginmodel);
    }
}
