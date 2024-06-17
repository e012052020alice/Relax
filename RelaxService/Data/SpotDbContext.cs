using Microsoft.EntityFrameworkCore;
using RelaxService.Models;

namespace RelaxService.Data
{
    public class SpotDbContext : DbContext
    {
        public SpotDbContext(DbContextOptions<SpotDbContext> options) : base(options)
        {

        }
        public DbSet<Attractions> Attractions { get; set; }
        public DbSet<EnterHome> EnterHome { get; set; }
        public DbSet<Search> Search { get; set; }
        public DbSet<AdsClick> AdsClick { get; set; }
        public DbSet<Analysis> Analysis { get; set; }
        public DbSet<AnalysisDetailM> AnalysisDetailM { get; set; }
        public DbSet<AnalysisDetailN> AnalysisDetailN { get; set; }
        public DbSet<AdsLike> AdsLike { get; set; }
        public DbSet<AdsDetailM> AdsDetailM { get; set; }
        public DbSet<AdsDetailN> AdsDetailN { get; set; }
        public DbSet<Advertisements> Advertisements { get; set;}
        public DbSet<GenderSearch> GenderSearch { get; set; }
        public DbSet<GenderAds> GenderAds { get; set; }
        public DbSet<GenderAdsLike> GenderAdsLike { get; set;}
        public DbSet<Keyword> Keyword { get; set; }
        public DbSet<EnterRoute> EnterRoute { get; set; }
    }
}
