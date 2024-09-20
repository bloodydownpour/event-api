
using System.ComponentModel.DataAnnotations;

namespace EventList.Domain.Data
{
    public class User
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

