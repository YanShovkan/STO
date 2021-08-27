using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BusinessLogic.BindingModels
{
    [DataContract]
    public class CostItemBindingModel
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string CostItemName { get; set; }

        [DataMember]
        public Dictionary<int, string> WorkCostItems { get; set; }
    }
}
