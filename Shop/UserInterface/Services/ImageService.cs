namespace UserInterface.Services
{
    public class ImageService
    {
        private readonly IConfiguration _configuration;

        public ImageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetFullImageUrl(string url)
        {
            return $"{_configuration.GetValue<string>("ApiSettings:BaseUrl")!.TrimStart('/')}/{url.TrimStart('/')}";
        }
    }
}
