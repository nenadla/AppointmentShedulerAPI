namespace AppointmentShedulerAPI.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int Counter { get; set; }
        public int Edited { get; set; }
        public int Cancelled { get; set; }

        // Navigacijska svojstva
        public ICollection<Appointment>? Appointments { get; set; }

    }
}
