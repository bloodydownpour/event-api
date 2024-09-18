using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Structure.Interfaces;

namespace WebApplication1.Structure.Data
{
    public class User : IUser
    {
        
        [Required]
        public Guid UserId { get; set; }
        public string _Name { get; set; }
        public string _Surname { get; set; }
        public DateOnly _DateOfBirth { get; set; }
        public DateOnly _RegisterDate { get; set; }
        [EmailAddress]
        public string _Email { get; set; }
        public string _Password { get; set; }
        public bool IsAdmin { get; set; }
        public string PfpName { get; set; }
    }
    
}

