using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AF.Demo.Models;
using Microsoft.Azure.Functions.Worker;
using System.Net.Http.Json;


namespace AF.Demo
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([TimerTrigger("0 /5 * * * * ")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                using HttpClient client = new HttpClient();

                ConfigurationBuilder builder = new ConfigurationBuilder();
                IConfigurationRoot config = builder.Build();

                HttpResponseMessage message = client.PostAsJsonAsync(
                    config["apiUrl"] + "/api/login", new { Username = config["username"], Password = config["password"] }).Result;
                if (message.IsSuccessStatusCode)
                {
                    TokenDTO? tokenDto = message.Content.ReadFromJsonAsync<TokenDTO>().Result;
                    if (tokenDto != null)
                    {
                        log.LogInformation(tokenDto.Token);
                    }
                    else
                    {
                        log.LogError("Failed to deserialize response.");
                    }
                }
                else
                {
                    log.LogError("Error");
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
            // TODO implement what you want
        }
    }
}
