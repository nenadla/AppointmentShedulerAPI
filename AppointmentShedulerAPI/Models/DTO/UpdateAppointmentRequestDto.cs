﻿namespace AppointmentShedulerAPI.Models.DTO
{
    public class UpdateAppointmentRequestDto
    {
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string? Worker { get; set; }

        public Guid ServiceId { get; set; }
        public Guid UserId { get; set; }
    }
}
