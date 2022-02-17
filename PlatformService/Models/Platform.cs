namespace PlatformService.Models
{
    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Publisher { get; set; } = String.Empty;
        public string Cost { get; set; } = String.Empty;
    }
}
