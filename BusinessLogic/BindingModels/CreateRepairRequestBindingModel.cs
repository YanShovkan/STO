using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BusinessLogic.BindingModels
{
    [DataContract]
    public class CreateRepairRequestBindingModel
    {
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public DateTime RepairRequestDate { get; set; }
        [DataMember]
        public List<int> RepairRequestWorks { get; set; }
    }
}