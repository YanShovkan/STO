using System.ComponentModel.DataAnnotations;

namespace DatabaseImplement.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string ClientName { get; set; }

        [Required]
        public string ClientLogin { get; set; }

        [Required]
        public string ClientPassword { get; set; }
    }
}
