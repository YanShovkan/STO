using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseImplement.Models
{
    public class CostItem
    {
        public int Id { get; set; }

        [Required]
        public string CostItemName { get; set; }

        [ForeignKey("CostItemId")]
        public virtual List<RepairRequestCostItem> RepairRequestCostItems { get; set; }
    }
}
