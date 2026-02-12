namespace WebApplication1.Models
{
    public class GoogleOAuthConfig
    {
        public WebConfig web { get; set; } = new();

        public class WebConfig
        {
            public string client_id { get; set; } = "";
            public string client_secret { get; set; } = "";
            public string project_id { get; set; } = "";
        }
    }

}
