using Microsoft.EntityFrameworkCore;
using Relax.Models;

namespace Relax.Data
{
    public class RelaxDbContext : DbContext
    {
        public RelaxDbContext(DbContextOptions<RelaxDbContext> options) : base (options)
        {
            
        }
        public DbSet<AdsDetail> AdsDetail { get; set; }
        public DbSet<Advertisements> Advertisements { get; set; }
        public DbSet<Analysis> Analysis { get; set; }
        public DbSet<AnalysisDetail> AnalysisDetail { get; set; }
        public DbSet<Attractions> Attractions { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<Points> Points { get; set; }
        public DbSet<RandomTripHistory> RandomTripHistory { get; set; }
        public DbSet<ScheduleDetails> ScheduleDetails { get; set; }
        public DbSet<Schedules> Schedules { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<Trips> Trips { get; set; }
    }
}
