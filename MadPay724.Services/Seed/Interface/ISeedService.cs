using System.Threading.Tasks;

namespace MadPay724.Services.Seed.Interface
{
    public interface ISeedService
    {
        Task SeedUsersAsync();
        void SeedUsers();
    }
}
