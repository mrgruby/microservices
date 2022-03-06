namespace PlatformService.Dtos
{
    /// <summary>
    /// This model will be sent to the CommandsService, when a new Platform is created here in the PlatformService.
    /// </summary>
    public class PlatformPublishedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Event { get; set; }
    }
}