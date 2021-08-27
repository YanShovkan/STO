using System.ComponentModel.DataAnnotations;

namespace DatabaseImplement.Models
{
    public class Worker
    {
        public int Id { get; set; }

        [Required]
        public string WorkerName { get; set; }

        [Required]
        public string WorkerLogin { get; set; }

        [Required]
        public string WorkerPassword { get; set; }
    }
}
