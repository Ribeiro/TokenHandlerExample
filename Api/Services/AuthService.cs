namespace services
{

using Newtonsoft.Json;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AuthService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        // Get the authorization server endpoint and credentials from configuration
        var tokenEndpoint = _configuration["AuthorizationServer: TokenEndpoint"];
        var clientId = _configuration["AuthorizationServer: ClientId"];
        var clientSecret = _configuration["AuthorizationServer: ClientSecret"];

        // Create the token request
        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);

        // Add the client credentials to the request body
        var formData = new List<KeyValuePair<string, string>>();
        formData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
        formData.Add(new KeyValuePair<string, string>("client_id", clientId));
        formData.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
        tokenRequest.Content = new FormUrlEncodedContent(formData);

        // Send the token request and get the response
        var response = await _httpClient.SendAsync(tokenRequest, cancellationToken);

        // Ensure the response is successful
        response.EnsureSuccessStatusCode();

        // Parse the access token from the response
        var content = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);
        return tokenResponse!.AccessToken;
    }
}

internal class TokenResponse
{
    public string AccessToken { get; set; }

    public int ExpiresIn { get; set; }
}

}