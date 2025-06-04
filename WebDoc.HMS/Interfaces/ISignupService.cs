using WebDoc.HMS.Models;

namespace WebDoc.HMS.Interfaces
{
    public interface IUserService
    {
        //Task<(bool Success, string ErrorMessage)> RegisterUserAsync(SignupModel model);
        Task<bool> RegisterUserAsync(User model);
    }
}
