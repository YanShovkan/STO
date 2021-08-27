using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BusinessLogic.BindingModels
{
    [DataContract]
    public class AddCostItemToRepairRequestBindingModel
    {
        [DataMember]
        public int RepairRequestId { get; set; }
        
        [DataMember]
        public List<int> RepairRequestCostItems { get; set; }
        
        [DataMember]
        public int Count { get; set; }
    }
}