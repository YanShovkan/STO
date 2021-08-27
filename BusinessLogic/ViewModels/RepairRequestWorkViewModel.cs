using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BusinessLogic.ViewModels
{
    [DataContract]
    public class RepairRequestWorkViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int RepairRequestId { get; set; }

        [DataMember]
        public int WorkId { get; set; }

        [DataMember]
        public string RepairRequestWorkName { get; set; }

        [DataMember]
        public int RepairRequestPaidSum { get; set; }

        [DataMember]
        public int WorkCost { get; set; }
    }
}