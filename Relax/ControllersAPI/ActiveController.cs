using Microsoft.AspNetCore.Cors;
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
    public class ActiveController : ControllerBase
    {
        private readonly RelaxDbContext _context;
        private readonly ApplicationDbContext _user;

        public ActiveController(RelaxDbContext context, ApplicationDbContext user)
        {
            _context = context;
            _user = user;

        }

        // GET: api/Active
        [HttpGet]
        public async Task<IEnumerable<ActiveDTO>> GetActive()
        {
            var acs = await _context.Activity.ToListAsync();
            var users = await _user.Users.ToListAsync();

            var result = from ac in acs
                         join user in users on ac.memberId equals user.Id
                         orderby ac.ActivityId
                         select new ActiveDTO
                         {
                             AcId = ac.ActivityId,
                             MemberId = ac.memberId,
                             UserName = user.UserName,
                             Title = ac.Title,
                             Detail = ac.Detail,
                             StartTime = ac.StartTime,
                             EndTime = ac.EndTime,
                             Image = ac.Image,
                             Position = ac.Position,
                         };

            return result;
        }

        // GET: api/Active/User
        [HttpGet("User")]
        public async Task<IEnumerable<SelectUserDTO>> GetUser()
        {
            var users = await _user.Users.ToListAsync();

            var result = from user in users
                         orderby user.UserName
                         select new SelectUserDTO
                         {
                             MemberId = user.Id,
                             UserName = user.UserName,
                         };

            return result;
        }

        // GET: api/Active
        [HttpGet("Go")]
        public async Task<IEnumerable<ActiveDTO>> GoActive()
        {
            var acs = await _context.Activity.ToListAsync();
            var users = await _user.Users.ToListAsync();
            var today = DateOnly.FromDateTime(DateTime.Now);

            var goArray = from ac in acs
                          join user in users on ac.memberId equals user.Id
                          where (today >= ac.StartTime) && (today <= ac.EndTime)
                          orderby Guid.NewGuid()
                          select new ActiveDTO
                          {
                              AcId = ac.ActivityId,
                              MemberId = ac.memberId,
                              UserName = user.UserName,
                              Title = ac.Title,
                              Detail = ac.Detail,
                              StartTime = ac.StartTime,
                              EndTime = ac.EndTime,
                              Image = ac.Image,
                              Position = ac.Position,
                          };
            if (goArray.Count() >= 3)
            {
                var random = goArray.Take(3).OrderBy(ac => ac.Position);
                return random;
            }
            else
            {
                return goArray.OrderBy(ac => ac.Position);
            };
        }

        // POST: api/Active
        [HttpPost]
        public async Task<string> PostActive(Activity activity)
        {
            if (activity.memberId != "" && activity.Title != "" && activity.StartTime != null && activity.EndTime != null && activity.Image != "" && activity.Detail != "" && activity.Position != "")
            {
                Activity ac = new Activity
                {
                    memberId = activity.memberId,
                    Title = activity.Title,
                    StartTime = activity.StartTime,
                    EndTime = activity.EndTime,
                    Image = activity.Image,
                    Detail = activity.Detail,
                    Position = activity.Position,
                };

                _context.Activity.Add(ac);
                await _context.SaveChangesAsync();

                return "新增成功";
            }

            return "新增失敗，資料不完整";

        }

        // DELETE: api/Active/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteActive(int id)
        {
            var Active = await _context.Activity.FindAsync(id);
            if (Active == null)
            {
                return "刪除失敗";
            }

            _context.Activity.Remove(Active);
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

        // PUT: api/Active/5
        [HttpPut("{id}")]
        public async Task<string> PutAdvertisements(int id, ActiveDTO acDTO)
        {
            if (id != acDTO.AcId)
            {
                return "修改失敗";
            }

            Activity activity = await _context.Activity.FindAsync(id);

            activity.memberId = acDTO.MemberId;
            activity.Title = acDTO.Title;
            activity.Detail = acDTO.Detail;
            activity.StartTime = acDTO.StartTime;
            activity.EndTime = acDTO.EndTime;
            activity.Image = acDTO.Image;
            activity.Position = acDTO.Position;

            _context.Entry(activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActiveExists(id))
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

        private bool ActiveExists(int id)
        {
            return _context.Activity.Any(e => e.ActivityId == id);
        }
    }
}
