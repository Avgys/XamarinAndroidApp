using System.Threading.Tasks;

namespace XamarinAndroidApp.Services
{
    public interface IFirebaseAuthentication
    {
        Task<bool> RegisterWithEmailAndPasswordAsync(string email, string password);
        Task<bool> LoginWithEmailAndPasswordAsync(string email, string password);
        bool SignOut();
        bool IsSignIn();
    }
}