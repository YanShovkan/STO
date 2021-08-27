using System.Collections.Generic;
using BusinessLogic.ViewModels;

namespace BusinessLogic.HelperModels
{
    class WordInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportRepairRequestWorkViewModel> RepairRequests { get; set; }
        public List<ReportWorkRepairRequestViewModel> Works { get; set; }

    }
}
