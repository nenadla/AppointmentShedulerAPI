namespace AppointmentShedulerAPI.Models.DTO
{
    public class AppointmentDto
    {
        // Polja iz Appointment modela
        public Guid AppointmentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }  // Ovo polje iz Appointment modela
        public string Worker { get; set; }

        // Polja iz User modela
        public Guid UserId { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int Counter { get; set; }
        public int Edited { get; set; }
        public int Cancelled { get; set; }

        // Polja iz Service modela
        public Guid ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public int Duration { get; set; }  // Ostavljen Duration iz Service modela
        public int Price { get; set; }
    }
}
