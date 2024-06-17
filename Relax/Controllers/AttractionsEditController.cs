using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Relax.Data;
using Relax.DTO;
using Relax.Models;
using System;
using System.Linq;

namespace Relax.Controllers
{
    [Authorize]
    [EnableCors("AllowMy")]

    public class AttractionsEditController : Controller
    {
        private readonly RelaxDbContext _context;
        private readonly ILogger<AttractionsEditController> _logger; // 日誌記錄器
        private readonly ApplicationDbContext _user;
        private readonly UserManager<ApplicationUser> _userManager;
        public AttractionsEditController(RelaxDbContext context, ILogger<AttractionsEditController> logger, ApplicationDbContext user, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _user = user;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Addattractions()
        {
            var tags = await _context.Tags.ToListAsync();
            ViewBag.Tags = new SelectList(tags, "TagName", "TagName");
            return View();
        }

        public async Task<IActionResult> Addtrips()
        {
            var tags = await _context.Tags.ToListAsync();
            ViewBag.Tags = new SelectList(tags, "TagName", "TagName");
            return View();
        }

        public IActionResult Edittrips()
        {
            //how to send data to view
            return View();
        }
        
        public IActionResult Randomtrips()
        {
            return View();
        }

        //GET: AttractionsEdit/Show
        [HttpGet("Show")]
        public async Task<IActionResult> ShowTrips(TripDTO tripDTO) // Change the return type to IActionResult
        {
            var memberId = _userManager.GetUserId(User); 

            var newTrips = from trips in _context.Trips
                           where trips.memberId == memberId
                           join schedules in _context.Schedules
                           on trips.TripId equals schedules.tripID
                           join scheduleDetails in _context.ScheduleDetails
                           on schedules.ScheduleId equals scheduleDetails.scheduleId
                           join attractions in _context.Attractions
                           on scheduleDetails.attractionId equals attractions.AttractionId
                           select new TripDTO
                           {
                               TripId = trips.TripId,
                               memberId = trips.memberId,
                               TripName = trips.TripName,
                               TotalDays = trips.TotalDays,
                               CreatedAt = trips.CreatedAt,
                               ScheduleId = schedules.ScheduleId,
                               DayNumber = schedules.DayNumber,
                               TotalStayTime = schedules.TotalStayTime,
                               ScheduleDetailsId = scheduleDetails.ScheduleDetailsId,
                               AttractionId = scheduleDetails.attractionId,
                               DailyTripNumber = scheduleDetails.DailyTripNumber,
                               AttractionName = attractions.AttractionName,
                               Description = attractions.Description,
                               Tag = attractions.Tag,
                               EstimatedStayTime = attractions.EstimatedStayTime,
                               TimeCategory = attractions.TimeCategory
                           };

            var mytrips = await newTrips.ToListAsync();
            return Ok(mytrips);
        }
        //Delete: AttractionsEdit/DeleteTrip
        [HttpDelete("DeleteTrip")]
        public async Task<IActionResult> DeleteTrip(int tripId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var trip = await _context.Trips.FindAsync(tripId);
                    if (trip == null)
                    {
                        return NotFound("未找到行程");
                    }

                    var schedules = await _context.Schedules.Where(s => s.tripID == tripId).ToListAsync();
                    _context.Schedules.RemoveRange(schedules);

                    foreach (var schedule in schedules)
                    {
                        var details = await _context.ScheduleDetails.Where(d => d.scheduleId == schedule.ScheduleId).ToListAsync();
                        _context.ScheduleDetails.RemoveRange(details);
                    }

                    _context.Trips.Remove(trip);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return Ok("行程及相關資料刪除成功");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "刪除行程時出現錯誤");
                    return StatusCode(500, "伺服器錯誤");
                }
            }
        }
        //Route: AttractionsEdit/PostAttractions
        [HttpPost]
        public async Task<IActionResult> PostAttractions([FromBody] AttractionDTO aDTO)
        {
            if (aDTO == null)
            {
                _logger.LogError("Received null aDTO");
                return BadRequest("Invalid data");
            }

            _logger.LogInformation("Received aDTO: {@aDTO}", aDTO);

            Attractions attractions = new Attractions
            {
                AttractionName = aDTO.AttractionName,
                memberId = _userManager.GetUserId(User),
                Description = aDTO.Description,
                City = aDTO.City,
                Tag = aDTO.Tag,
                TimeCategory = aDTO.TimeCategory,
                EstimatedStayTime = aDTO.EstimatedStayTime,
                XCoordinate = aDTO.XCoordinate,
                YCoordinate = aDTO.YCoordinate,
                IsApproved = aDTO.IsApproved,
            };

            _context.Attractions.Add(attractions);
            await _context.SaveChangesAsync();

            return Ok(attractions);
        }

        [HttpGet]
        public async Task<IActionResult> SearchAttractionByName(string name)
        {
            var attractions = await _context.Attractions
                                    .Where(a => a.AttractionName.Contains(name))
                                    .Select(a => new {
                                        a.AttractionId,
                                        a.AttractionName,
                                        a.Description,
                                        a.City,
                                        a.Tag,
                                        a.EstimatedStayTime,
                                        a.TimeCategory,
                                        a.XCoordinate,
                                        a.YCoordinate
                                    }).ToListAsync();

            return Ok(attractions);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTrip([FromBody] TripSaveModel tripSaveModel)
        {
            if (tripSaveModel == null || string.IsNullOrEmpty(tripSaveModel.TripName) || tripSaveModel.Days == null || !tripSaveModel.Days.Any())
            {
                return BadRequest("Invalid trip data");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var trip = new Trips
                    {
                        memberId = _userManager.GetUserId(User),
                        TripName = tripSaveModel.TripName,
                        TotalDays = tripSaveModel.Days.Count,
                        CreatedAt = DateTime.Now
                    };

                    _context.Trips.Add(trip);
                    await _context.SaveChangesAsync();

                    foreach (var day in tripSaveModel.Days)
                    {
                        var schedule = new Schedules
                        {
                            tripID = trip.TripId,
                            DayNumber = day.DayNumber,
                            TotalStayTime = day.Attrs.Sum(a => a.EstimatedStayTime)
                        };

                        _context.Schedules.Add(schedule);
                        await _context.SaveChangesAsync();

                        foreach (var attr in day.Attrs)
                        {
                            _logger.LogInformation("Received dailyTripNumber: {0}", attr.DailyTripNumber); // 打印 dailyTripNumber

                            var scheduleDetail = new ScheduleDetails
                            {
                                scheduleId = schedule.ScheduleId,
                                attractionId = attr.AttractionId,
                                DailyTripNumber = attr.DailyTripNumber // 確保這裡的值是自動生成的
                            };

                            _context.ScheduleDetails.Add(scheduleDetail);
                        }

                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    return Ok("行程儲存成功");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "儲存行程時出現錯誤");
                    return StatusCode(500, "伺服器錯誤");
                }
            }
        }
        // GET: AttractionsEdit/GetRandomTrips
        [HttpGet("GetRandomTrips")]
        public async Task<IActionResult> GetRandomTrips()
        {
            var memberId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(memberId))
            {
                return Unauthorized("User is not logged in.");
            }

            _logger.LogInformation("GetRandomTrips called by memberId: {MemberId}", memberId);

            try
            {
                var randomTrips = await _context.RandomTripHistory
                                                .AsNoTracking()
                                                .Where(rt => rt.memberID == memberId)
                                                .OrderBy(rt => rt.RandomId)
                                                .ToListAsync();
                return Ok(randomTrips);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting random trips for memberId: {MemberId}", memberId);
                return StatusCode(500, "Server error");
            }
        }

        // DELETE: AttractionsEdit/DeleteRandomTrips
        [HttpDelete("DeleteRandomTrips/{randomName}")]
        public async Task<IActionResult> DeleteRandomTrips(string randomName)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var tripsToDelete = await _context.RandomTripHistory
                                                      .Where(rt => rt.RandomName == randomName)
                                                      .ToListAsync();

                    if (!tripsToDelete.Any())
                    {
                        return NotFound($"未找到名稱為 {randomName} 的行程");
                    }

                    _context.RandomTripHistory.RemoveRange(tripsToDelete);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return Ok($"已刪除名稱為 {randomName} 的所有行程");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "刪除隨機行程時出現錯誤");
                    return StatusCode(500, "伺服器錯誤");
                }
            }
        }
        
        [HttpGet("SearchAttractionByNameAddAttr")]
        public async Task<IActionResult> SearchAttractionByNameAddAttr(string name)
        {
            var attractions = await _context.Attractions
                .Where(a => a.AttractionName.Contains(name))
                .Select(a => new {
                    a.AttractionId,
                    a.AttractionName,
                    a.Description,
                    a.City,
                    a.Tag,
                    a.EstimatedStayTime,
                    a.TimeCategory,
                    a.XCoordinate,
                    a.YCoordinate
                }).ToListAsync();

            return Ok(attractions);
        }

    }
}
