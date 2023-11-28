using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManager.SLAModels
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public int? Fine { get; set; }
    }
}