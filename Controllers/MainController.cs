using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Newtonsoft;
using Microsoft.AspNetCore.Html;

namespace TravelPal.Controllers;

public class MainController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Tubes(string name, int numTimes = 1)
    {
        List<string> stops = new List<string>();
        stops.Add("940GZZLUACT");
        stops.Add("940GZZLUCTN");
        stops.Add("940GZZLUDBN");
        stops.Add("940GZZLUFYR");
        stops.Add("940GZZLUKSX");
        stops.Add("940GZZLULNB");
        stops.Add("940GZZLUOXC");
        stops.Add("940GZZLUPCC");
        stops.Add("940GZZLUTBY");
        stops.Add("940GZZLUWHM");
        stops.Add("940GZZLUCPS");
        stops.Add("940GZZLUGFD");
        stops.Add("940GZZLUHR5");
        stops.Add("940GZZLUKNB");
        stops.Add("940GZZLUOVL");
        stops.Add("940GZZLURYO");
        stops.Add("940GZZLUSTD");
        stops.Add("940GZZLUTMH");
        stops.Add("940GZZLUWPL");
        stops.Add("940GZZLUWIM");

        Random rng = new Random();
        int n = stops.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            string value = stops[k];  
            stops[k] = stops[n];  
            stops[n] = value;  
        }  
        stops = stops.Take(10).ToList();
        ViewData["res"] = new HtmlString(trafficUpdates(stops));
        return View();
    }

    static string trafficUpdates(List<string> stops)
    {
        List<Loc> locList = new List<Loc>();
        foreach(var stop in stops) {
            string lat = "";
            string lon = "";
            string name = "";
            List<TrafficUpdate> trafficUpdates = new List<TrafficUpdate>();
            using (HttpClient client = new HttpClient())
                {
                    // Construct the API URL
                    string apiUrl = $"https://api.tfl.gov.uk/StopPoint/{stop}";

                    // Make the API request
                    var task = Task.Run(() => client.GetAsync(apiUrl)); 
                    task.Wait();
                    HttpResponseMessage response = task.Result;

                    var task2 = response.Content.ReadAsStringAsync();
                    task2.Wait();
                    string res = task2.Result;

                    dynamic jsd = Newtonsoft.Json.JsonConvert.DeserializeObject(res);
                    lat = jsd["lat"].ToString();
                    lon = jsd["lon"].ToString();
                    name = jsd["commonName"].ToString();
                    name = name.Replace("\'", "");
                }


            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Construct the API URL
                    string apiUrl = $"https://api.tfl.gov.uk/StopPoint/{stop}/Arrivals";

                    // Make the API request
                    var task = Task.Run(() => client.GetAsync(apiUrl)); 
                    task.Wait();
                    HttpResponseMessage response = task.Result;

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var task2 = response.Content.ReadAsStringAsync();
                        task2.Wait();
                        string responseContent = task2.Result;

                        // Parse the JSON response and extract relevant information
                        // Adjust this code based on the structure of the API response

                        dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
                        foreach (var road in jsonData)
                        {
                            TrafficUpdate update = new TrafficUpdate
                            {
                                LineName = road["lineName"].ToString(),
                                ExpectedArrival = road["expectedArrival"].ToString(),
                                PlatformName = road["platformName"].ToString(),
                            };
                            trafficUpdates.Add(update);
                        }
                        Loc loc = new Loc();
                        loc.desc = trafficUpdates;
                        loc.lat = lat;
                        loc.lon = lon;
                        loc.name = name;
                        locList.Add(loc);
                    }
                    else
                    {
                        Console.WriteLine("API request failed. Status code: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(locList);
        return json;
    }

    // 
    // GET: /Buses/ 
    public IActionResult Buses(string name, int numTimes = 1)
    {
        List<string> stops = new List<string>();
        stops.Add("LTZ1063");
        stops.Add("YY15OYT");
        stops.Add("LF67EXE");
        stops.Add("LG71DPU");
        stops.Add("LJ18FKF");
        stops.Add("LK62DVP");
        stops.Add("LK64EDJ");
        stops.Add("LK17DDO");
        stops.Add("LX61DFG");
        stops.Add("LTZ1140");
        stops.Add("LK17DBU");
        stops.Add("LK17DDA");
        stops.Add("YY14WEA");
        stops.Add("YX68UNN");
        stops.Add("SK20BGF");
        stops.Add("LJ59LYA");
        stops.Add("LJ59LZB");
        stops.Add("LK17AHO");
        stops.Add("LB69JNL");
        stops.Add("YJ19HVF");

        Random rng = new Random();
        int n = stops.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            string value = stops[k];  
            stops[k] = stops[n];  
            stops[n] = value;  
        }  
        stops = stops.Take(15).ToList();
        ViewData["res"] = new HtmlString(getres(stops));
        return View();
    }

    static string getres(List<string> stops)
    {
        List<Loc> locList = new List<Loc>();
        foreach(var stop in stops) {
            string lat = "";
            string lon = "";
            string name = "";
            List<TrafficUpdate> trafficUpdates = new List<TrafficUpdate>();
            using (HttpClient client = new HttpClient())
                {
                    // Construct the API URL
                    string apiUrl = $"https://api.tfl.gov.uk/Vehicle/{stop}/Arrivals";

                    // Make the API request
                    var task = Task.Run(() => client.GetAsync(apiUrl)); 
                    task.Wait();
                    HttpResponseMessage response = task.Result;

                    var task2 = response.Content.ReadAsStringAsync();
                    task2.Wait();
                    string res = task2.Result;

                    dynamic jsd = Newtonsoft.Json.JsonConvert.DeserializeObject(res);
                    Console.WriteLine(jsd);
                    try {
                    name = jsd[0]["stationName"].ToString();
                    name = name.Replace("\'", "");


                    apiUrl = $"https://api.tfl.gov.uk/StopPoint/Search/{name}";
                    task = Task.Run(() => client.GetAsync(apiUrl)); 
                    task.Wait();
                    response = task.Result;

                    task2 = response.Content.ReadAsStringAsync();
                    task2.Wait();
                    res = task2.Result;
                    dynamic jsd2 = Newtonsoft.Json.JsonConvert.DeserializeObject(res);
                    Console.WriteLine(jsd2);
                    lat = jsd2["matches"][0]["lat"].ToString();
                    lon = jsd2["matches"][0]["lon"].ToString();

                    foreach (var road in jsd)
                        {
                            var lN = road["stationName"].ToString();
                            lN = lN.Replace("\'", "");
                            TrafficUpdate update = new TrafficUpdate
                            {
                                LineName = lN,
                                ExpectedArrival = road["expectedArrival"].ToString(),
                                PlatformName = road["platformName"].ToString(),
                            };
                            trafficUpdates.Add(update);
                        }
                        Loc loc = new Loc();
                        loc.desc = trafficUpdates;
                        loc.lat = lat;
                        loc.lon = lon;
                        loc.name = name;
                        locList.Add(loc);   
                    }
                    catch (System.ArgumentOutOfRangeException) {
                        continue;
                    }         

                }
        }
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(locList);
        return json;
    }


    public IActionResult TubeDelays(string name, int numTimes = 1)
    {
        List<string> stops = new List<string>();

        ViewData["close"] = new HtmlString(getDel("4", "Planned Closure"));
        ViewData["seve"] = new HtmlString(getDel("6", "Severe Delays"));
        ViewData["mino"] = new HtmlString(getDel("9", "Minor Delays"));
        return View();
    }

    static string getDel(string stop, string reas)
    {
        List<TrafficUpdate> trafficUpdates = new List<TrafficUpdate>();
        using (HttpClient client = new HttpClient())
            {
                // Construct the API URL
                string apiUrl = $"https://api.tfl.gov.uk/Line/Status/{stop}";

                // Make the API request
                var task = Task.Run(() => client.GetAsync(apiUrl)); 
                task.Wait();
                HttpResponseMessage response = task.Result;

                var task2 = response.Content.ReadAsStringAsync();
                task2.Wait();
                string res = task2.Result;

                dynamic jsd = Newtonsoft.Json.JsonConvert.DeserializeObject(res);
                Console.WriteLine(jsd);

                foreach (var road in jsd)
                    {
                        var lN1 = road["name"].ToString();
                        lN1 = lN1.Replace("\'", "");
                        var lN2 = reas;
                        var lN3 = road["lineStatuses"][0]["reason"].ToString();
                        lN3 = lN3.Replace("\'", "");
                        TrafficUpdate update = new TrafficUpdate
                        {
                            LineName = lN1,
                            ExpectedArrival = lN2,
                            PlatformName = lN3,
                        };
                        trafficUpdates.Add(update);
                    }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(trafficUpdates);
                return json;
            }   
    }

    public IActionResult RoadDelays(string name, int numTimes = 1)
    {
        List<string> stops = new List<string>();

        ViewData["res"] = new HtmlString(getRoadDel());
        return View();
    }

    static string getRoadDel()
    {
        List<RoadDelay> trafficUpdates = new List<RoadDelay>();
        using (HttpClient client = new HttpClient())
            {
                // Construct the API URL
                string apiUrl = $"https://api.tfl.gov.uk/Road/all/Disruption";

                // Make the API request
                var task = Task.Run(() => client.GetAsync(apiUrl)); 
                task.Wait();
                HttpResponseMessage response = task.Result;

                var task2 = response.Content.ReadAsStringAsync();
                task2.Wait();
                string res = task2.Result;

                dynamic jsd = Newtonsoft.Json.JsonConvert.DeserializeObject(res);
                Console.WriteLine(jsd);

                for(var i=0; i<10; i++)
                    {
                        var lN1 = jsd[i]["category"].ToString();
                        lN1 = lN1.Replace("\'", "");
                        var lN2 = jsd[i]["subCategory"].ToString();
                        lN1 = lN2.Replace("\'", "");
                        var lN3 = jsd[i]["comments"].ToString();
                        lN3 = lN3.Replace("\'", "");
                        lN3 = lN3.Replace("\r", "");
                        lN3 = lN3.Replace("\n", "");
                        RoadDelay update = new RoadDelay
                        {
                            Category = lN1,
                            SubCategory = lN2,
                            Comments = lN3,
                        };
                        trafficUpdates.Add(update);
                    }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(trafficUpdates);
                return json;
            }   
    }
}

class TrafficUpdate
{
    public string LineName { get; set; }
    public string ExpectedArrival { get; set; }
    public string PlatformName { get; set; }
}

class RoadDelay
{
    public string Category { get; set; }
    public string SubCategory { get; set; }
    public string Comments { get; set; }
}

class Loc
{
    public List<TrafficUpdate> desc { get; set; }
    public string lat { get; set; }
    public string lon { get; set; }
    public string name { get; set; }
}