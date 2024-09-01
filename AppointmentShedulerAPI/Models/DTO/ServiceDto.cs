namespace AppointmentShedulerAPI.Models.DTO
{
    public class ServiceDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Duration { get; set; }
        public int Price { get; set; }

    }
}
