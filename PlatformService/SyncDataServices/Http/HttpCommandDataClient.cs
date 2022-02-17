using PlatformService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{
    /// <summary>
    /// This will be injected into the PlatformsController, so that it can communicate with the CommandService microservice.
    /// For instance, When a new platform has been created, it should be sent to the CommandService.
    /// </summary>
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration config)
        {
            _config = config;
            _httpClient = httpClient;
        }

        //When a new platform has been created, it should be sent to the CommandService.
        //This will be an async communication between the two microservices.
        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            //Create a payload for the post request.
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
                );
            //Call the TestInboundConnection Action method in the CommandService's PlatformsController, using the path in the appsettings file
            var response = await _httpClient.PostAsync($"{_config["CommandService"]}", httpContent);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("--> Sync POST to CommandService was OK");
            else
                Console.WriteLine("--> Sync POST to CommandService was NOT OK");

            //return response.Content.ReadAsStringAsync();
        }
    }
}
