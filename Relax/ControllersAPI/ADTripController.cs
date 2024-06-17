using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relax.Data;
using Relax.DTO;
using Relax.Models;

namespace Relax.ControllersAPI
{
    [EnableCors("AllowMy")]

    [Route("api/[controller]")]
    [ApiController]
    public class ADTripController : ControllerBase
    {
        private readonly RelaxDbContext _context;
        private readonly ApplicationDbContext _user;
		private readonly UserManager<ApplicationUser> _userManager;


		public ADTripController(RelaxDbContext context, ApplicationDbContext user, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _user = user;
            _userManager = userManager;
        }

        // GET: api/ADTrip
        [HttpGet]
        public async Task<IEnumerable<ADTripDTO>> GetTrips()
        {
            var ads = await _context.Advertisements.ToListAsync();
            var users = await _user.Users.ToListAsync();

            var ADTrip = from ad in ads
                         join user in users on ad.memberId equals user.Id
                         join trip in _context.Trips on ad.tripId equals trip.TripId
                         orderby Guid.NewGuid()
                         select new ADTripDTO
                         {
                             AdId = ad.AdId,
                             MemberId = ad.memberId,
                             Name = user.Name,
                             AdImg = ad.Image,
                             TripId = trip.TripId,
                             TripName = trip.TripName,
                             TotalDays = trip.TotalDays,
                             Detail = ad.Detail,
                         };
            if (ADTrip.Count() >= 6)
            {
                var random = ADTrip.Take(6);
                return random;
            }
            else
            {
                return ADTrip.OrderBy(trip => trip.AdId);
            };
        }

        //GET: api/ADTrip/Detail
        [HttpGet("Detail")]
        public async Task<IEnumerable<ADTripDetailDTO>> GetDetail(int TripId)
        {

            var ADTrip = from ad in _context.Advertisements
                         join trip in _context.Trips on ad.tripId equals trip.TripId
                         join schedules in _context.Schedules on trip.TripId equals schedules.tripID
                         join details in _context.ScheduleDetails on schedules.ScheduleId equals details.scheduleId
                         join attractions in _context.Attractions on details.attractionId equals attractions.AttractionId
                         select new
                         {
                             AdImg = ad.Image,
                             Detail = ad.Detail,
                             TripId = trip.TripId,
                             TripName = trip.TripName,
                             TotalDays = trip.TotalDays,
                             DayNumber = schedules.DayNumber,
                             TotalTime = schedules.TotalStayTime,
                             DailyNumber = details.DailyTripNumber,
                             AttractionId = attractions.AttractionId,
                             AttractionName = attractions.AttractionName,
                             Description = attractions.Description,
                             Tag = attractions.Tag,
                             TimeCategory = attractions.TimeCategory,
                             StayTime = attractions.EstimatedStayTime,
                         };

            return ADTrip.Where(
                atd => TripId == atd.TripId
                ).Select(
                adt => new ADTripDetailDTO
                {
                    AdImg = adt.AdImg,
                    Detail = adt.Detail,
                    TripId = adt.TripId,
                    TripName = adt.TripName,
                    TotalDays = adt.TotalDays,
                    DayNumber = adt.DayNumber,
                    TotalTime = adt.TotalTime,
                    DailyNumber = adt.DailyNumber,
                    AttractionId = adt.AttractionId,
                    AttractionName = adt.AttractionName,
                    Description = adt.Description,
                    Tag = adt.Tag,
                    TimeCategory = adt.TimeCategory,
                    StayTime = adt.StayTime,

                }
                );
        }

		//POST: api/ADTrip/User
		[HttpPost("User")]
		public async Task<string> GetUser()
		{
			var userId = _userManager.GetUserId(User);
            if(userId == null)
            {
                return "未登入";
            }
            return userId;

		}
	}
}
