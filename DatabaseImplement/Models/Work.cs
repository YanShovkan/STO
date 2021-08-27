using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseImplement.Models
{
    public class Work
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }

        [Required]
        public string WorkName { get; set; }

        [Required]
        public decimal WorkPrice { get; set; }

        public virtual Worker Worker { get; set; }

        [ForeignKey("WorkId")]
        public virtual List<RepairRequestWork> RepairRequestWorks { get; set; }
    }
}
