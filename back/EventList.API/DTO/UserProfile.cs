using AutoMapper;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;
namespace EventList.API.DTO
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<User, UserDTO>();
            CreateMap<Event, EventDTO>();
        }
    }
}
