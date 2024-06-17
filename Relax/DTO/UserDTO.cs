namespace Relax.DTO
{
	public class UserDTO
	{
		public string Id { get; set; }
		public string? UserName { get; set; }
        public string? RoleName { get; set; }
		public string? Name { get; set; }
        public string? Gender { get; set; }
        public DateOnly? BirthDay { get; set; }
		public string? Email { get; set; }
		//public int? Points { get; set;}

    }
}
