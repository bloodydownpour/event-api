using System.ComponentModel.DataAnnotations;
using EventList.Domain.Interfaces;
namespace EventList.API.DTO
{
    public class UserDTO : IUserDTO
    {
        public Guid UserId { get; set; }
        public string _Name { get; set; }
        public string _Surname { get; set; }
        public DateOnly _DateOfBirth { get; set; }
        public string PfpName { get; set; }
    }
}
