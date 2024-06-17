namespace Relax.DTO
{
    public class TripSaveModel
    {
        public string TripName { get; set; }
        public List<DaySaveModel> Days { get; set; }
    }
}
