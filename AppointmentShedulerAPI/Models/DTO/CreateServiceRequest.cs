namespace AppointmentShedulerAPI.Models.DTO
{
    public class CreateServiceRequest
    {
        public string? Name { get; set; }
        public int Duration { get; set; }
        public int Price { get; set; }
    }
}
