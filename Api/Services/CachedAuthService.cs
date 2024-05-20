namespace services
{

    using Microsoft.Extensions.Caching.Memory;

    public class CachedAuthService : IAuthService
    {
        private readonly IAuthService _authService;
        private readonly IMemoryCache _cache;

        public CachedAuthService(IAuthService authService, IMemoryCache cache)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            const string cacheKey = "AccessToken";
            if (!_cache.TryGetValue(cacheKey, out string accessToken))
            {
                // Access token not found in cache, call the authentication service to obtain it
                var token = await _authService.GetAccessTokenAsync(cancellationToken);

                // Cache the access token till it expires
                //var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(tokenResponse.ExpiresIn — 60)); // reduce 1 min from actual expiration time to avoid unauthorized
                //_cache.Set(cacheKey, accessToken, cacheOptions);

                 //return token.AccessToken;
                 return token;
            }
            return null;
           
        }
    }

}
