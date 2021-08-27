using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessLogic.Enums;

namespace DatabaseImplement.Models
{
    public class RepairRequest
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        [Required]
        public decimal RepairRequestPrice { get; set; }

        [Required]
        public DateTime RepairRequestDate { get; set; }

        [Required]
        public PaymentStatus RepairRequestStatus { get; set; }
        public virtual Client Client { get; set; }

        [ForeignKey("RepairRequestId")]
        public virtual List<RepairRequestWork> RepairRequestWorks { get; set; }

        [ForeignKey("RepairRequestId")]
        public virtual List<RepairRequestCostItem> RepairRequestCostItems { get; set; }
    }
}
