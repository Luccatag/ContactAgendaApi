using System.ComponentModel.DataAnnotations;

namespace ContactAgendaApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        // Add more fields as needed (e.g., Email)
    }
}
