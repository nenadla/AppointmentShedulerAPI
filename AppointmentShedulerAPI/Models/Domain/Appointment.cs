namespace AppointmentShedulerAPI.Models.Domain
{
    public class Appointment
    {

        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Worker { get; set; }

        // Strani ključevi
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
