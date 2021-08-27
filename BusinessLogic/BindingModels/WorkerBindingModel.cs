using System.Runtime.Serialization;

namespace BusinessLogic.BindingModels
{
    [DataContract]
    public class WorkerBindingModel
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string WorkerName { get; set; }

        [DataMember]
        public string WorkerLogin { get; set; }

        [DataMember]
        public string WorkerPassword { get; set; }
    }
}
