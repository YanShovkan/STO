using System.Runtime.Serialization;

namespace BusinessLogic.BindingModels
{
    [DataContract]
    public class ClientBindingModel
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string ClientName { get; set; }

        [DataMember]
        public string ClientLogin { get; set; }

        [DataMember]
        public string ClientPassword { get; set; }
    }
}
