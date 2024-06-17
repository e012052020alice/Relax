using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relax.Data;
using Relax.DTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Relax.Models;
using Microsoft.AspNetCore.Cors;

namespace Relax.ControllersAPI
{
    [EnableCors("AllowMy")]

    [Route("api/[controller]")]
    [ApiController]
    public class ShareController : ControllerBase
    {
        private readonly RelaxDbContext _context;
        private readonly ApplicationDbContext _user;
        private readonly UserManager<ApplicationUser> _userManager;


        public ShareController(RelaxDbContext context, ApplicationDbContext user, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _user = user;
            _userManager = userManager;

        }

        // GET: api/Share
        [HttpGet]
        public async Task<IEnumerable<ShareDTO>> GetTrips()
        {
            var trips = await _context.Trips.ToListAsync();
            var users = await _user.Users.ToListAsync();

            var userId = _userManager.GetUserId(User);

            var shares = from trip in trips
                         where trip.memberId == userId
                         join user in users on trip.memberId equals user.Id
                         join ad in _context.Advertisements on trip.TripId equals ad.tripId into adsNull
                         from ads in adsNull.DefaultIfEmpty()
                         select new ShareDTO
                         {
                             TripId = trip.TripId,
                             TripName = trip.TripName,
                             MemberId = trip.memberId,
                             TotalDays = trip.TotalDays,
                             Name = user.Name,
                             AdId = ads?.AdId,
                             StartTime = ads?.StartTime,
                             EndTime = ads?.EndTime,
                             AdImg = ads?.Image,
                             Detail = ads?.Detail,
                         };
            return shares;
        }

        // POST: api/Share
        [HttpPost]
        public async Task<string> PostShare(ShareDTO shareDTO)
        {
            if (shareDTO.TripId != 0 && shareDTO.AdImg != "" && shareDTO.StartTime != null && shareDTO.EndTime != null && shareDTO.Detail != "")
            {
                Advertisements ad = new Advertisements
                {
                    tripId = shareDTO.TripId,
                    StartTime = shareDTO.StartTime,
                    EndTime = shareDTO.EndTime,
                    Image = shareDTO.AdImg,
                    Detail = shareDTO.Detail,
                    memberId = shareDTO.MemberId,
                };

                _context.Advertisements.Add(ad);
                await _context.SaveChangesAsync();

                return "新增成功";
            }

            return "新增失敗，資料不完整";
        }

        //PUT: api/Share/5
        [HttpPut("{id}")]
        public async Task<string> putShare(int id, ShareDTO shareDTO)
        {
            if (id != shareDTO.AdId)
            {
                return "修改失敗";
            }

            Advertisements advertisements = await _context.Advertisements.FindAsync(id);

            if (shareDTO.AdId != null && shareDTO.AdImg != null && shareDTO.StartTime != null && shareDTO.EndTime != null && shareDTO.Detail != null)
            {
                advertisements.StartTime = shareDTO.StartTime;
                advertisements.EndTime = shareDTO.EndTime;
                advertisements.Image = shareDTO.AdImg;
                advertisements.Detail = shareDTO.Detail;

                _context.Entry(advertisements).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertisementsExists(id))
                    {
                        return "修改失敗";
                    }
                    else
                    {
                        throw;
                    }
                }
                return "修改成功";
            }
            return "修改失敗，資料不完整";
        }

        //PUT: api/Share/Stop
        [HttpPut("Stop")]
        public async Task<string> stopShare(ShareDTO shareDTO)
        {
            var id = shareDTO.AdId;

            if (id == null)
            {
                return "unstopShare";
            }

            Advertisements advertisements = await _context.Advertisements.FindAsync(id);

            if (shareDTO.AdId != null && shareDTO.EndTime != null)
            {
                advertisements.EndTime = shareDTO.EndTime;

                _context.Entry(advertisements).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertisementsExists(id.Value))
                    {
                        return "stopShare";
                    }
                    else
                    {
                        throw;
                    }
                }
                return "stopShare";
            }
            return "unstopShare";
        }

        private bool AdvertisementsExists(int id)
        {
            return _context.Advertisements.Any(e => e.AdId == id);
        }
    }
}

