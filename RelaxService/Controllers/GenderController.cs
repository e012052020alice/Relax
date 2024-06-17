using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RelaxService.Data;
using RelaxService.DTOs.Analysis;
using RelaxService.Models;

namespace RelaxService.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class GenderController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public GenderController(SpotDbContext context)
        {
            _context = context;
        }

        //POST: api/Gender/SearchAll
        [HttpPost("SearchAll")]
        public async Task<IEnumerable<GenderSearchDTO>> GenderSearch([FromBody] DateRangeRequest request)
        {
            var dateStart = request.DateStart;
            var dateEnd = request.DateEnd;
            var query = _context.GenderSearch
                        .Where(gS => gS.DataDate >= dateStart && gS.DataDate <= dateEnd)
                        .GroupBy(gS => new { gS.Gender, gS.DataDate })
                        .Select(g => new GenderSearchDTO
                        {
                            Gender = g.Key.Gender,
                            Age18 = g.Sum(x => x.Age18),
                            Age25 = g.Sum(x => x.Age25),
                            Age35 = g.Sum(x => x.Age35),
                            Age45 = g.Sum(x => x.Age45),
                            Age65 = g.Sum(x => x.Age65),
                            DataDate = g.Key.DataDate
                        })
                        .OrderByDescending(g => g.DataDate)
                        .AsEnumerable();  // This will defer the execution of the query until it is enumerated.

            return query;
        }

        //POST: api/Gender/SumAll
        [HttpPost("SumAll")]
        public async Task<GenderSumDTO> GenderSum([FromBody] DateRangeRequest request)
        {
            var dateStart = request.DateStart;
            var dateEnd = request.DateEnd;

            // Query GenderSearch table
            var searchResults = await _context.GenderSearch
                .Where(g => g.DataDate >= dateStart && g.DataDate <= dateEnd)
                .GroupBy(g => g.Gender)
                .Select(g => new
                {
                    Gender = g.Key,
                    Count = g.Sum(x => (x.Age18 ?? 0) + (x.Age25 ?? 0) + (x.Age35 ?? 0) + (x.Age45 ?? 0) + (x.Age65 ?? 0))
                })
                .ToListAsync(); // Asynchronously materialize to List

            // Query GenderAds table
            var adsResults = await _context.GenderAds
                .Where(g => g.DataDate >= dateStart && g.DataDate <= dateEnd)
                .GroupBy(g => g.Gender)
                .Select(g => new
                {
                    Gender = g.Key,
                    Count = g.Sum(x => (x.Age18 ?? 0) + (x.Age25 ?? 0) + (x.Age35 ?? 0) + (x.Age45 ?? 0) + (x.Age65 ?? 0))
                })
                .ToListAsync(); // Asynchronously materialize to List

            // Query GenderAdsLike table
            var likeResults = await _context.GenderAdsLike
                .Where(g => g.DataDate >= dateStart && g.DataDate <= dateEnd)
                .GroupBy(g => g.Gender)
                .Select(g => new
                {
                    Gender = g.Key,
                    Count = g.Sum(x => (x.Age18 ?? 0) + (x.Age25 ?? 0) + (x.Age35 ?? 0) + (x.Age45 ?? 0) + (x.Age65 ?? 0))
                })
                .ToListAsync(); // Asynchronously materialize to List

            // Aggregate results
            var genderSumDto = new GenderSumDTO
            {
                SearchM = searchResults.Where(x => x.Gender == "男").Sum(x => (int?)x.Count) ?? 0,
                SearchF = searchResults.Where(x => x.Gender == "女").Sum(x => (int?)x.Count) ?? 0,
                AdsM = adsResults.Where(x => x.Gender == "男").Sum(x => (int?)x.Count) ?? 0,
                AdsF = adsResults.Where(x => x.Gender == "女").Sum(x => (int?)x.Count) ?? 0,
                LikeM = likeResults.Where(x => x.Gender == "男").Sum(x => (int?)x.Count) ?? 0,
                LikeF = likeResults.Where(x => x.Gender == "女").Sum(x => (int?)x.Count) ?? 0
            };

            return genderSumDto;
        }

        //POST: api/Gender/KeywordSum
        [HttpPost("KeywordSum")]
        public async Task<KeywordDTO> KeywordSum([FromBody] DateRangeRequest request)
        {
            var dateStart = request.DateStart;
            var dateEnd = request.DateEnd;

            var keywordResults = await _context.Keyword
                .Where(k => k.DataDate >= dateStart && k.DataDate <= dateEnd)
                .OrderByDescending(k => k.Value)
                .ThenByDescending(k => k.DataDate)
                .ToListAsync();

            var topKeywords = keywordResults.Take(6).Select(k => k.Keyname).ToList();

            var keywordDto = new KeywordDTO
            {
                KeynameA = topKeywords.ElementAtOrDefault(0),
                KeynameB = topKeywords.ElementAtOrDefault(1),
                KeynameC = topKeywords.ElementAtOrDefault(2),
                KeynameD = topKeywords.ElementAtOrDefault(3),
                KeynameE = topKeywords.ElementAtOrDefault(4),
                KeynameF = topKeywords.ElementAtOrDefault(5)
            };

            return keywordDto;
        }

        //POST: api/Gender/EnterRoute

        [HttpPost("EnterRoute")]
        public async Task<IEnumerable<EnterRouteDTO>> EnterRoute([FromBody] DateRangeRequest request)
        {
            var dateStart = request.DateStart;
            var dateEnd = request.DateEnd;

            return _context.EnterRoute
                .Where(er => er.DataDate >= dateStart && er.DataDate <= dateEnd)
                .GroupBy(er => new { er.RouteName, er.Device })
                .Select(g => new EnterRouteDTO
                {
                    RouteName = g.Key.RouteName,
                    Device = g.Key.Device,
                    Value = g.Sum(er => er.Value)
                });

        }
    }
}
