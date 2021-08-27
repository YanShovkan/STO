using System.Runtime.Serialization;

namespace BusinessLogic.BindingModels
    {
        [DataContract]
        public class AddPaymentToRepairRequestBindingModel
        {
            [DataMember]
            public int WorkId { get; set; }

            [DataMember]
            public int Sum { get; set; }
        }
    }
