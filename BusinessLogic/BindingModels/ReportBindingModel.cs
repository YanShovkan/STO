using BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BusinessLogic.BindingModels
{
    [DataContract]
    public class ReportBindingModel
    {
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public int? ClientId { get; set; }
        [DataMember]
        public int? WorkerId { get; set; }
        [DataMember]
        public DateTime? DateFrom { get; set; }
        [DataMember]
        public DateTime? DateTo { get; set; }
        [DataMember]
        public List<ReportWorkRepairRequestViewModel> listRepairRequestsWorks { get; set; }
        [DataMember]
        public List<ReportRepairRequestWorkViewModel> listWorksRepairRequests { get; set; }
        [DataMember]
        public List<ReportCostItemViewModel> listCostItems { get; set; }
        [DataMember]
        public List<ReportPaymentViewModel> listPayments { get; set; }
    }
}