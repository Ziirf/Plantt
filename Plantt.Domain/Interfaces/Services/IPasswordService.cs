using Plantt.Domain.Models;

namespace Plantt.Domain.Interfaces.Services
{
    public interface IPasswordService
    {
        Password CreatePassword(string password);
        bool VerifyPassword(string password, Password hashedPassword);
    }
}