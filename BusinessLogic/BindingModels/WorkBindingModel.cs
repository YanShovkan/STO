using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BusinessLogic.BindingModels
{
    [DataContract]
    public class WorkBindingModel
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public int WorkerId { get; set; }

        [DataMember]
        public string WorkName { get; set; }

        [DataMember]
        public decimal WorkPrice { get; set; }

        [DataMember]
        public Dictionary<int, string> RepairRequestWorks { get; set; }

        [DataMember]
        public List<int> IdToFilter { get; set; }
    }
}
