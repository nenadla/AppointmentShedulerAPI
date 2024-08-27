namespace AppointmentShedulerAPI.Models.Domain
{
    public class Appointment
    {
        public Guid Id {get; set;}
        public string Username { get; set;} 
        public string? Email { get; set;}
        public string? Phone { get; set;}
        public DateTime StartTime { get; set;}
        public DateTime EndTime { get; set;}
        
        public string? Service { get; set;}
        public string? Worker { get; set;}
        public int? Counter {  get; set;}
        public int? Cancelled {  get; set;}

    }
}
