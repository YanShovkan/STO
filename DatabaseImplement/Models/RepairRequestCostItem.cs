using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseImplement.Models
{
    public class RepairRequestCostItem
    {
        public int Id { get; set; }

        public int RepairRequestId { get; set; }

        public int CostItemId { get; set; }

        [Required]
        public int CostItemCount { get; set; }

        public virtual RepairRequest RepairRequest { get; set; }

        public virtual CostItem CostItem { get; set; }
    }
}
