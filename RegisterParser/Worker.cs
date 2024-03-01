using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;
namespace RegisterParser
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    using (HttpClient client = new HttpClient())
                    {
                        // Устанавливаем заголовки
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                        client.DefaultRequestHeaders.Add("Origin", "https://dom.gosuslugi.ru");
                        client.DefaultRequestHeaders.Add("Accept-Language", "ru,en;q=0.9");
                        client.DefaultRequestHeaders.Connection.Add("keep-alive");
                        client.DefaultRequestHeaders.Add("Host", "dom.gosuslugi.ru");
                        client.DefaultRequestHeaders.Referrer=new Uri("https://dom.gosuslugi.ru/");
                        client.DefaultRequestHeaders.Add("Request-Guid", "005af609-b9a1-45ba-a07d-6cc3ac7d516a");
                        client.DefaultRequestHeaders.Add("Session-Guid", "4146e522-c218-42b9-a208-4ba92865f0e8");
                        client.DefaultRequestHeaders.Add("State-Guid", "/rp");
                        
                        // Создаем объект для отправки
                        //var data = new { key1 = "value1", key2 = "value2" };

                        var postData = new Dictionary<string, object>
                        {
                            { "numberOrUriNumber", null },
                            { "typeList", new object[]{} },
                            { "examStartFrom", "2024-01-31T21:00:00.000Z" },
                            { "examStartTo", "2024-02-27T21:00:00.000Z" },
                            { "orderNumber", null },
                            { "statusList", new object[]{} },
                            { "isAssigned", null },
                            { "hasOffences", new object[] { } },
                            { "preceptsMade", new object[] { } },
                            { "formList", new object[] { } },
                            { "oversightActivitiesRefList", new object[] { } }
                        };




                        String json = JsonSerializer.Serialize(postData);
                        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                        // Отправляем POST запрос
                        //HttpResponseMessage response = await client.PostAsync("http://example.com/api/data", content);
                        
                        HttpResponseMessage response = await client.PostAsync("https://dom.gosuslugi.ru/inspection/api/rest/services/examinations/public/search?page=1&itemsPerPage=100", content);

                        // Обрабатываем ответ
                        if (response.IsSuccessStatusCode)
                        {
                            string responseJson = await response.Content.ReadAsStringAsync();
                            //var responseData = JsonConvert.DeserializeObject<dynamic>(responseJson);
                            //var jsonString = "{\"key1\":\"value1\",\"key2\":\"value2\"}";
                            JsonElement deserializedData = JsonSerializer.Deserialize<JsonElement>(responseJson);
                            Console.WriteLine("Десериализованные данные:");
                            //Console.WriteLine(deserializedData.items[0].examObjective);


                            //foreach (var item in deserializedData["items"])
                            //{
                            //    string guid = item["guid"];
                            //    string number = item["number"];
                            //    string date = item["date"];

                            //    Console.WriteLine(guid+" "+number+" "+date);
                            //    // и так далее...
                            //}

                            int count = 0;
                            foreach (JsonElement item in deserializedData.GetProperty("items").EnumerateArray())
                            {
                                
                                string guid = item.GetProperty("guid").GetString();
                                string number = item.GetProperty("number").GetString();
                                string date = item.GetProperty("date").GetString();
                                Console.WriteLine(count++.ToString()+" "+ guid + " " + number + " " + date);
                                // итак далее...
                            }

                            //Console.WriteLine(deserializedData);
                            //Console.WriteLine(deserializedData.key2);

                            //Console.WriteLine("Ответ:");
                            //Console.WriteLine(responseData);
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: " + response.StatusCode);
                        }
                    }

                }
                await Task.Delay(100000, stoppingToken);
            }
        }
    }
}
