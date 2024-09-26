namespace AppointmentShedulerAPI.Models.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int Counter { get; set; }
        public int Edited { get; set; }
        public int Cancelled { get; set; }
    }
}
