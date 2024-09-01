namespace AppointmentShedulerAPI.Models.Domain
{
    public class Service
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Duration { get; set; }
        public int Price { get; set; }

        // Navigacijska svojstva
        public ICollection<Appointment> Appointments { get; set; }

    }
}
