using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
using Relax.Data;
using Relax.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace Relax.Controllers
{
    [EnableCors("AllowMy")]
    public class HomeController : Controller

    {
        private readonly ILogger<HomeController> _logger;
        RelaxDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        int pointsDistance = 5;
        Random rand = new Random();
        int randIndex = 0;
        double totalTime = 0;
        bool isContainsTags = true;
        dynamic lastpoint;

        public HomeController(ILogger<HomeController> logger, RelaxDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetLodging(string location, string key)//附近住宿
        {
            int resultCount = 0;
            int radius = 1500;
            string Data = "";
            var attempt = 0;
            JObject jsonData;
            do
            {
                attempt++;
                Data = await searchNearby(location, key, radius);
                jsonData = JObject.Parse(Data);
                //JArray results = (JArray)jsonData["results"];
                resultCount = jsonData["results"].Count();
                radius += 500;
            } while (resultCount < 5 && attempt < 10);
            return Json(Data);
        }

        public async Task<string> searchNearby(string location, string key, int radius)
        {
            string Url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={location}&radius={radius}&keyword=hotel&language=zh-TW&key={key}";
            HttpClient Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            HttpResponseMessage Response = await Client.GetAsync(Url);
            Response.EnsureSuccessStatusCode();
            string Data = await Response.Content.ReadAsStringAsync();
            return Data;
        }

        [HttpGet]
        public async Task<JsonResult> GetTags()
        {
            var AllTags = _context.Tags.Select(p => new
            {
                TagName = p.TagName
            });
            return Json(AllTags);
        }
        public class SearchTagsRequest
        {
            public string[] selectedtags { get; set; }
            public int distance { get; set; }
            public int valueDay { get; set; }
            public int numbers { get; set; }
        }

        [HttpPost]
        public async Task<JsonResult> SearchTags([FromBody] SearchTagsRequest TagsRequest)//搜尋標籤的景點
        {
            string[] tagsToSearch = TagsRequest.selectedtags;
            pointsDistance = TagsRequest.distance;
            int onedayNum = TagsRequest.numbers;
            var tags = Enumerable.Empty<dynamic>();
            var excludeTags = Enumerable.Empty<dynamic>();
            if (tagsToSearch.Length == 0)
            {
                tags = _context.Attractions.Select(s => new
                {
                    positionA = s.AttractionName,
                    AX = Convert.ToDouble(s.XCoordinate) / 100000,
                    AY = Convert.ToDouble(s.YCoordinate) / 100000,
                    StayTime = s.EstimatedStayTime,
                }).ToList();
            }
            else
            {
                tags = _context.Attractions.Where(p => tagsToSearch.Any(tag => p.Tag.Contains(tag))).Select(s => new
                {
                    positionA = s.AttractionName,
                    AX = Convert.ToDouble(s.XCoordinate) / 100000,
                    AY = Convert.ToDouble(s.YCoordinate) / 100000,
                    StayTime = s.EstimatedStayTime,
                }).ToList();
                excludeTags = _context.Attractions.Where(p => tagsToSearch.Any(tag => !p.Tag.Contains(tag))).Select(s => new
                {
                    positionA = s.AttractionName,
                    AX = Convert.ToDouble(s.XCoordinate) / 100000,
                    AY = Convert.ToDouble(s.YCoordinate) / 100000,
                    StayTime = s.EstimatedStayTime,
                }).ToList();

            }
            List<dynamic> allData = new List<dynamic>();
            int maxAttempts = 10;
            //dynamic lastpoint = null;
            for (int i = 0; i < TagsRequest.valueDay; i++)
            {
                totalTime = 0;
                var onedayData = await EnsruePositions(tags, excludeTags, onedayNum, i + 1);

                var ran = onedayData.Value as List<dynamic>;
                lastpoint = ran[ran.Count - 2];
                int attempts = 0;
                while (onedayData == null && attempts < maxAttempts)
                {
                    onedayData = await EnsruePositions(tags, excludeTags, onedayNum, TagsRequest.valueDay);
                    attempts++;
                }
                if (attempts == 10)//嘗試超過10次，警告訊息
                {

                }
                allData.Add(onedayData);
            }
            return Json(allData);
        }

        public async Task<JsonResult> EnsruePositions(dynamic tags, dynamic excludeTags, int onedayNum, int valueDay)
        {
            List<dynamic> positions = new List<dynamic>();
            dynamic random1;
            if (tags.Count > 0 && valueDay == 1)
            {
                randIndex = rand.Next(tags.Count);
                random1 = tags[randIndex];//經緯度為整數
                tags.RemoveAt(randIndex);
            }
            else if (valueDay == 1)
            {
                randIndex = rand.Next(excludeTags.Count);
                random1 = excludeTags[randIndex];
                excludeTags.RemoveAt(randIndex);
            }
            else /*if (valueDay > 1)*/
            {
                //沒選標籤，前面使用tags，但應改使用excludeTags
                random1 = NextPoint(tags, excludeTags, lastpoint);//Day1 lastpoint
                if (isContainsTags)
                {
                    for (int i = 0; i < tags.Count; i++)
                    {
                        if (random1.positionA == tags[i].positionA)
                        {
                            tags.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < excludeTags.Count; i++)
                    {
                        if (random1.positionA == excludeTags[i].positionA)
                        {
                            excludeTags.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            dynamic newRandomA = new
            {
                positionA = random1.positionA,
                AX = random1.AX,
                AY = random1.AY, // 如果需要，也可以修改 AY 屬性的值
                StayTime = random1.StayTime
            };

            if (onedayNum == 1)
            {
                positions.Add(newRandomA);
                totalTime = (newRandomA.StayTime) * 0.5;
                positions.Add(totalTime);
                return Json(positions);
            }
            var random2 = NextPoint(tags, excludeTags, newRandomA);//經緯度為double													 
            randIndex = -1;
            if (isContainsTags)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    if (random2.positionA == tags[i].positionA)
                    {
                        tags.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < excludeTags.Count; i++)
                {
                    if (random2.positionA == excludeTags[i].positionA)
                    {
                        excludeTags.RemoveAt(i);
                        break;
                    }
                }
            }

            //int pointStayTime = await RouteTime($"{newRandomA.AY},{newRandomA.AX}", $"{random2.AY},{random2.AX}", "DRIVING");
            //totalTime = ((newRandomA.StayTime + random2.StayTime) / 2.0) + pointStayTime;
            totalTime = (newRandomA.StayTime + random2.StayTime) * 0.5;
            positions.Add(newRandomA);
            positions.Add(random2);
            int numbers = onedayNum;
            for (int i = 2; i < numbers; i++)
            {
                if (i > 2)
                {
                    random2 = positions[i - 1];
                    newRandomA = positions[i - 2];
                }
                double x = newRandomA.AX - random2.AX;//B-A
                x = Math.Sign(x);
                double y = newRandomA.AY - random2.AY;
                y = Math.Sign(y);

                var random3 = NextPoint(tags, excludeTags, random2);//選取完，從tags remove

                var confirmC = QuadrantPercent(tags, excludeTags, random2, random3, x, y);
                randIndex = -1;
                if (isContainsTags)
                {
                    for (int index = 0; i < tags.Count; index++)
                    {
                        if (confirmC.positionA == tags[index].positionA)
                        {
                            tags.RemoveAt(index);
                            break;
                        }
                    }
                }
                else
                {
                    for (int index = 0; i < excludeTags.Count; index++)
                    {
                        if (confirmC.positionA == excludeTags[index].positionA)
                        {
                            excludeTags.RemoveAt(index);
                            break;
                        }
                    }
                }
                //pointStayTime = await RouteTime($"{random2.AY},{random2.AX}", $"{confirmC.AY},{confirmC.AX}", "DRIVING");
                totalTime += (confirmC.StayTime) * 0.5;

                positions.Add(confirmC);
            }
            positions.Add(totalTime);
            return Json(positions);
        }

        public dynamic NextPoint(dynamic tags, dynamic excludeTags, dynamic randomA)//根據距離和標籤條件選擇下一個景點 
        {
            List<(string AttractionName, double AX, double AY, int StayTime)> pointWithRadius = new List<(string AttractionName, double AX, double AY, int StayTime)>();
            isContainsTags = true;

            pointWithRadius = CalculateDistance(tags, randomA);
            if (pointWithRadius.Count == 0)//如果10公里以下符合標籤的景點為0,則使用不在標籤的景點替代
            {
                isContainsTags = false;
                pointWithRadius = CalculateDistance(excludeTags, randomA);
            }
            //Random rand=new Random();
            randIndex = rand.Next(pointWithRadius.Count);
            //var pointB = pointWithRadius[randIndex];
            var pointB = new
            {
                positionA = pointWithRadius[randIndex].AttractionName,
                AX = pointWithRadius[randIndex].AX,
                AY = pointWithRadius[randIndex].AY,
                StayTime = pointWithRadius[randIndex].StayTime
            };
            return pointB;

        }

        public dynamic CalculateDistance(dynamic tags, dynamic randomA)//計算兩個經緯度之間的距離，並過濾符合距離條件的景點 
        {
            const double R = 6371.0; // 地球半徑（單位：公里）
            List<(string AttractionName, double AX, double AY, int StayTime)> pointWithRadius = new List<(string AttractionName, double AX, double AY, int StayTime)>();
            List<(string AttractionName, double AX, double AY, int StayTime)> pointWithRadius10 = new List<(string AttractionName, double AX, double AY, int StayTime)>();
            //double centerLon = 120.30190;//需換成上一個找到的點
            //double centerLat = 22.62326;
            double centerLon = randomA.AX;//需換成上一個找到的點
            double centerLat = randomA.AY;
            List<(string AttractionName, double AX, double AY, int StayTime)> points = new List<(string AttractionName, double AX, double AY, int StayTime)>();
            foreach (var tag in tags)
            {
                double ax = tag.AX;
                double ay = tag.AY;
                points.Add((tag.positionA, ax, ay, tag.StayTime));
            }
            foreach (var point in points)
            {
                double lon = point.AX;
                double lat = point.AY;
                // 將緯度和經度從度轉換為弧度
                double lonRad = ToRadians(lon);
                double latRad = ToRadians(lat);
                double centerLonRad = ToRadians(centerLon);
                double centerLatRad = ToRadians(centerLat);

                // 計算座標之間的距離
                double dLon = lonRad - centerLonRad;
                double dLat = latRad - centerLatRad;

                // 應用球面余弦定理
                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                           Math.Cos(centerLatRad) * Math.Cos(latRad) *
                           Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                double distance = R * c;
                if (distance <= pointsDistance)
                {
                    pointWithRadius.Add(point);
                }
                else if (distance > pointsDistance && distance <= pointsDistance + 5)
                {
                    pointWithRadius10.Add(point);
                }

            }
            if (pointWithRadius.Count == 0)
            {
                pointWithRadius = pointWithRadius10;
            }
            return pointWithRadius;
        }

        public double ToRadians(double angle)// 度轉弧度的轉換工具函數 
        {
            return angle * (Math.PI / 180);
        }

        public dynamic QuadrantPercent(dynamic tags, dynamic excludeTags, dynamic randomB, dynamic randomC, double x, double y)
        {//根據象限概率選擇下一個景點
            int cx = Convert.ToInt32((randomC.AX) * 100000) - Convert.ToInt32((randomB.AX) * 100000);//C-B
            cx = Math.Sign(cx);
            int cy = Convert.ToInt32((randomC.AY) * 100000) - Convert.ToInt32((randomB.AY) * 100000);
            cy = Math.Sign(cy);
            if (cx == -x && cy == -y)//如果是相反象限，需在加跑一次1/10的機率
            {
                randIndex = rand.Next(1, 101);
                if (randIndex > 10)//100隨機數只有小於10，才會只接回傳randomC，否則再重跑一次選C的流程
                {
                    randomC = NextPoint(tags, excludeTags, randomC);
                    randomC = QuadrantPercent(tags, excludeTags, randomB, randomC, x, y);
                }
            }
            return randomC;
        }

        public async Task<int> RouteTime(string origin, string destination)
        {
            string apiKey = "myApiKey";
            string travelMode = "DRIVING"; // 旅行模式
            string apiUrl = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={origin}&destinations={destination}&mode={travelMode}&key={apiKey}";
            HttpClient Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            HttpResponseMessage Response = await Client.GetAsync(apiUrl);
            Response.EnsureSuccessStatusCode();
            var Data = await Response.Content.ReadAsStringAsync();
            var distanceMatrix = JObject.Parse(Data);
            int duration = distanceMatrix["rows"][0]["elements"][0]["duration"]["value"].Value<int>();

            return (duration / 60);
        }
        public class RandomTripRequest
        {
            public string? RandName { get; set; }
            public List<List<Location>>? Locations { get; set; }
        }

        public class Location
        {

            public string? PositionA { get; set; }
        }
        [HttpPost]
        public void GoogleMapUrl([FromBody] RandomTripRequest request)
        {
            var googleMapUrl = new StringBuilder("https://www.google.com/maps/dir/");
            var memberId = _userManager.GetUserId(User);
            for (int i = 0; i < request.Locations.Count; i++)
            {
                List<string> points = new List<string>();
                googleMapUrl = new StringBuilder("https://www.google.com/maps/dir/");
                for (int j = 0; j < request.Locations[i].Count; j++)
                {
                    points.Add(request.Locations[i][j].PositionA);
                }
                var origin = Uri.EscapeDataString(points[0]);
                var destination = Uri.EscapeDataString(points[points.Count - 1]);

                googleMapUrl.Append(origin).Append("/");
                for (int index = 1; index < points.Count - 1; index++)
                {
                    googleMapUrl.Append(Uri.EscapeDataString(points[index])).Append("/");
                }
                googleMapUrl.Append(destination);
                RandomTripHistory myRandomTripHistory = new RandomTripHistory()
                {
                    memberID = memberId,
                    RandomName = request.RandName,
                    GoogleMapResultLink = googleMapUrl.ToString(),
                };
                _context.RandomTripHistory.Add(myRandomTripHistory);
                _context.SaveChanges();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return base.View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
