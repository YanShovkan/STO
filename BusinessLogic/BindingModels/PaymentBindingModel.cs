using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BusinessLogic.BindingModels
{
    [DataContract]
    public class PaymentBindingModel
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int WorkId { get; set; }
        [DataMember]
        public int WorkInRepairRequestId { get; set; }
        [DataMember]
        public DateTime PaymentDate { get; set; }
        [DataMember]
        public int Sum { get; set; }
        [DataMember]
        public DateTime? DateFrom { get; set; }
        [DataMember]
        public DateTime? DateTo { get; set; }
        [DataMember]
        public List<int> Ids { get; set; }
    }
}
