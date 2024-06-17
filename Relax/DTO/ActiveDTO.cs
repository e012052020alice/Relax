namespace Relax.DTO
{
    public class ActiveDTO
    {
        public int AcId { get; set; }
        public string? MemberId { get; set; }
        public string? UserName { get; set; }

        public string? Title { get; set; }
        public string? Detail { get; set; }
        public DateOnly? StartTime { get; set; }
        public DateOnly? EndTime { get; set; }
        public string? Image { get; set; }
        public string? Position { get; set; }

    }
}
