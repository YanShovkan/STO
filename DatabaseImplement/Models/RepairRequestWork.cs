using System.ComponentModel.DataAnnotations;

namespace DatabaseImplement.Models
{
    public class RepairRequestWork
    {
        public int Id { get; set; }

        public int RepairRequestId { get; set; }

        public int WorkId { get; set; }

        [Required]
        public int WorkCost { get; set; }
        [Required]
        public int PaidSum { get; set; }

        public virtual RepairRequest RepairRequest { get; set; }

        public virtual Work Work { get; set; }
    }
}
