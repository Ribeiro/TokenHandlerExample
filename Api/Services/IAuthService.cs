
namespace services
{

    public interface IAuthService
    {
        public Task<string> GetAccessTokenAsync(CancellationToken cancellationToken);
    }

}