using System.Net.Http.Headers;
using System.Text.Json;
using RosKvartalTestTask.Models;
using RosKvartalTestTask.Data;
using Microsoft.EntityFrameworkCore;



using (HttpClient client = new HttpClient())
{

    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Add("Origin", "https://dom.gosuslugi.ru");
    client.DefaultRequestHeaders.Add("Accept-Language", "ru,en;q=0.9");
    client.DefaultRequestHeaders.Connection.Add("keep-alive");
    client.DefaultRequestHeaders.Add("Host", "dom.gosuslugi.ru");
    client.DefaultRequestHeaders.Referrer = new Uri("https://dom.gosuslugi.ru/");
    client.DefaultRequestHeaders.Add("Request-Guid", "005af609-b9a1-45ba-a07d-6cc3ac7d516a");
    client.DefaultRequestHeaders.Add("Session-Guid", "4146e522-c218-42b9-a208-4ba92865f0e8");
    client.DefaultRequestHeaders.Add("State-Guid", "/rp");


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


    HttpResponseMessage response = await client.PostAsync("https://dom.gosuslugi.ru/inspection/api/rest/services/examinations/public/search?page=1&itemsPerPage=100", content);


    if (response.IsSuccessStatusCode)
    {
        string responseJson = await response.Content.ReadAsStringAsync();
        JsonElement deserializedData = JsonSerializer.Deserialize<JsonElement>(responseJson);
        Console.WriteLine("Десериализованные данные:");
        var options = new DbContextOptionsBuilder<RosKvartalTestTaskContext>()
        .UseSqlServer("Data Source = DESKTOP-MUA8FEB\\SQLEXPRESS; Initial Catalog = Register; Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true;")
        .Options;

        int count = 0;
        using (var context = new RosKvartalTestTaskContext(options))
        {

            foreach (JsonElement item in deserializedData.GetProperty("items").EnumerateArray())
            {

                var purpose = item.GetProperty("examObjective").GetString(); // Purpose
                int id = int.Parse(item.GetProperty("number").GetString()); //id
                var date = item.GetProperty("date").GetString(); // CheckDate
                var status = item.GetProperty("status").GetString(); // status
                string result = item.GetProperty("hasOffence").GetRawText(); // result


                var subject = "Default";
                try
                {
                    subject = item.GetProperty("subject")
                    .GetProperty("organizationInfoEnriched")
                    .GetProperty("registryOrganizationCommonDetailWithNsi")
                    .GetProperty("shortName")
                    .GetRawText();

                }
                catch 
                {
                    Console.WriteLine("Отсутствует информация о субъекте");
                    subject = "Default";
                }
                var ogrn = "Default";
                try
                {
                     ogrn = item.GetProperty("subject")
                    .GetProperty("organizationInfoEnriched")
                    .GetProperty("registryOrganizationCommonDetailWithNsi")
                    .GetProperty("ogrn")
                    .GetString();

                }
                catch
                {
                    Console.WriteLine("Отсутствует огрн");
                    ogrn = "Default";
                }
                Console.WriteLine(id.ToString() + " " + date + " " + status + " " + subject + " " + ogrn + " " + result);

                ResultInspection res_inspect = new ResultInspection();
                if (result == "null")
                {
                    res_inspect = ResultInspection.Unknown;

                }
                else if (result == "true")
                {
                    res_inspect = ResultInspection.Success;

                }
                else if (result == "false")
                {
                    res_inspect = ResultInspection.Failed;
                }
                else
                {
                    throw new Exception("Неизвестное значение");
                }
                DateTime check_date;
                if(!DateTime.TryParse(date, out check_date))
                {
                    throw new Exception("Неверный формат даты");
                }

                var entity = new InspectionsRegister { SubjectName = subject,Status=status,SubjectNumber=ogrn,Result=res_inspect,CheckDate=check_date,Purpose=purpose };
                Console.WriteLine(count++.ToString());
                context.InspectionsRegister.Add(entity);


            }

            context.SaveChanges();

        }

    }
    else
    {
        Console.WriteLine("Ошибка: " + response.StatusCode);
    }


   
}