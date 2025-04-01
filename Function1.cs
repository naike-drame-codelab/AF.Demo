using System;
using System.Net.Http.Json;
using AF.Demo.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AF.Demo
{
    public class Function1
    {
        [Function("Function1")]
        public void Run([TimerTrigger("0 30 12 * * 1")] TimerInfo myTimer, ILogger log)
        {
            using HttpClient client = new HttpClient();

            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationRoot config = builder.Build();

            HttpResponseMessage message = client.PostAsJsonAsync(config["apiUrl"]+"/api/login", new { Username = config["username"], Password = config["password"] }).Result;
            if (message.IsSuccessStatusCode)
            {
                string token = message.Content.ReadFromJsonAsync<TokenDTO>().Result.Token;
                log.LogInformation(token);
            }
            // TODO Implement what you want
        }
    }
}
