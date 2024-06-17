using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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
    public class AdvertisementsController : ControllerBase
    {
        private readonly RelaxDbContext _context;
        private readonly ApplicationDbContext _user;


        public AdvertisementsController(RelaxDbContext context, ApplicationDbContext user)
        {
            _context = context;
            _user = user;

        }

        // GET: api/Advertisements
        [HttpGet]
        public async Task<IEnumerable<ADDTO>> GetAdvertisements()
        {
            var ads = await _context.Advertisements.ToListAsync();
            var users = await _user.Users.ToListAsync();
            //var trips = await _context.Trips.ToListAsync();

            var result = from ad in ads
                         join user in users on ad.memberId equals user.Id
                         join trip in _context.Trips on ad.tripId equals trip.TripId
                         orderby ad.AdId
                         select new ADDTO
                         {
                             AdId = ad.AdId,
                             MemberId = ad.memberId,
                             UserName = user.UserName,
                             TripId = ad.tripId,
                             TripName = trip.TripName,
                             Detail = ad.Detail,
                             StartTime = ad.StartTime,
                             EndTime = ad.EndTime,
                             Image = ad.Image,
                             Clicks = ad.Clicks,
                             AdLoved = ad.AdLoved,
                         };

            return result;
        }
        // POST: api/Advertisements/Fliter
        [HttpPost("Fliter")]
        public async Task<IEnumerable<ADDTO>> FliterAdvertisements(ADDTO adDTO)
        {
            var ads = await _context.Advertisements.ToListAsync();
            var users = await _user.Users.ToListAsync();

            var result = from ad in ads
                         join user in users on ad.memberId equals user.Id
                         join trip in _context.Trips on ad.tripId equals trip.TripId
                         where !(adDTO.StartTime > ad.EndTime) && !(adDTO.EndTime < ad.StartTime)
                         select new ADDTO
                         {
                             AdId = ad.AdId,
                             MemberId = ad.memberId,
                             UserName = user.UserName,
                             TripId = ad.tripId,
                             TripName = trip.TripName,
                             StartTime = ad.StartTime,
                             EndTime = ad.EndTime,
                             Clicks = ad.Clicks,
                             AdLoved = ad.AdLoved,

                         };

            return result;
        }

        // POST: api/Advertisements/Trip
        [HttpPost("Trip")]
        public async Task<IEnumerable<ADmodalDTO>> Flitertrip(string memberId)
        {
            var result = from trip in _context.Trips
                         where trip.memberId == memberId
                         select new ADmodalDTO
                         {
                             TripId = trip.TripId,
                             TripName = trip.TripName,
                         };

            return result;
        }

        // GET: api/Advertisements/5
        [HttpGet("{id}")]
        public async Task<ADDTO> GetAdvertisements(int id)
        {
            var ad = await _context.Advertisements.FindAsync(id);
            var user = await _user.Users.FirstOrDefaultAsync(u => u.Id == ad.memberId);
            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.TripId == ad.tripId);

            ADDTO adDTO = null;

            adDTO = new ADDTO
            {
                AdId = ad.AdId,
                MemberId = ad.memberId,
                UserName = user.UserName,
                TripId = ad.tripId,
                TripName = trip.TripName,
                StartTime = ad.StartTime,
                EndTime = ad.EndTime,
                Clicks = ad.Clicks,
                AdLoved = ad.AdLoved,
            };

            return adDTO;
        }

        // PUT: api/Advertisements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutAdvertisements(int id, ADDTO adDTO)
        {
            if (id != adDTO.AdId)
            {
                return "修改失敗";
            }

            Advertisements advertisements = await _context.Advertisements.FindAsync(id);
            Trips trips = await _context.Trips.FirstOrDefaultAsync(t => t.TripId == advertisements.tripId);

            trips.TripName = adDTO.TripName;
            advertisements.StartTime = adDTO.StartTime;
            advertisements.EndTime = adDTO.EndTime;

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

        // POST: api/Advertisements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<string> PostAdvertisements(Advertisements advertisements)
        {
            if(advertisements.tripId !=0 && advertisements.StartTime!=null && advertisements.EndTime != null&& advertisements.Image != ""&& 
              advertisements.Detail!="" && advertisements.memberId !="")
            {
                Advertisements ad = new Advertisements
                {
                    tripId = advertisements.tripId,
                    StartTime = advertisements.StartTime,
                    EndTime = advertisements.EndTime,
                    Image = advertisements.Image,
                    Detail = advertisements.Detail,
                    memberId = advertisements.memberId,

                };

                _context.Advertisements.Add(ad);
                await _context.SaveChangesAsync();

                return "新增成功";
            }

            return "新增失敗，資料不完整";
        }

        // DELETE: api/Advertisements/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteAdvertisements(int id)
        {
            var advertisements = await _context.Advertisements.FindAsync(id);
            if (advertisements == null)
            {
                return "刪除失敗";
            }

            _context.Advertisements.Remove(advertisements);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return "刪除關聯紀錄失敗";
            }
            return "刪除成功";
        }

        private bool AdvertisementsExists(int id)
        {
            return _context.Advertisements.Any(e => e.AdId == id);
        }
    }
}
