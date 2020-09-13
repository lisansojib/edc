namespace Microsoft.AspNetCore.Http
{
    public static class HttpExtensions
    {
        public static string BaseUrl(this HttpRequest request) => $"{request.Scheme}://{request.Host}{request.PathBase}";
    }
}
