using AutoMapper;
using AppointmentShedulerAPI.Models.Domain;
using AppointmentShedulerAPI.Models.DTO;

namespace AppointmentShedulerAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<User, UserDto>();
            CreateMap<CreateUserRequestDto, User>();
            CreateMap<UpdateUserRequestDto, User>();

            CreateMap<Service, ServiceDto>();
            CreateMap<CreateServiceRequest, Service>();
            CreateMap<UpdateServiceRequestDto, Service>();

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.Username : null))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User != null ? src.User.Phone : null))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
                .ForMember(dest => dest.Counter, opt => opt.MapFrom(src => src.User != null ? src.User.Counter : 0))
                .ForMember(dest => dest.Edited, opt => opt.MapFrom(src => src.User != null ? src.User.Edited : 0))
                .ForMember(dest => dest.Cancelled, opt => opt.MapFrom(src => src.User != null ? src.User.Cancelled : 0))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service != null ? src.Service.Name : null))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Service != null ? src.Service.Price : 0))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Service != null ? src.Service.Duration : 0)); 

            CreateMap<CreateAppointmentRequestDto, Appointment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.StartTime.AddMinutes(src.Duration)));

            CreateMap<UpdateAppointmentRequestDto, Appointment>()
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.StartTime.AddMinutes(src.Duration)));
        }
    }
}

