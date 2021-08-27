using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using BusinessLogic.Enums;
namespace BusinessLogic.BindingModels
{
    [DataContract]
    public class RepairRequestBindingModel
    {
        [DataMember]
        public int? Id { get; set; }
       
        [DataMember]
        public int? ClientId { get; set; }

        [DataMember]
        public string RepairRequestName { get; set; }

        [DataMember]
        public decimal RepairRequestTotalPrice { get; set; }

        [DataMember]
        public DateTime RepairRequestDate { get; set; }

        [DataMember]
        public PaymentStatus RepairRequestStatus { get; set; }
        [DataMember]
        public DateTime? DateFrom { get; set; }
        [DataMember]
        public DateTime? DateTo { get; set; }
        [DataMember]
        public List<int> IdToFilter { get; set; }

        [DataMember]
        public Dictionary<int, (string, int, int)> RepairRequestWorks { get; set; }

        [DataMember]
        public Dictionary<int, (string, int)> RepairRequestCostItems { get; set; }
    }
}
