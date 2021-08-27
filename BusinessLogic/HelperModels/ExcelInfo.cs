using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.ViewModels;

namespace BusinessLogic.HelperModels
{
    class ExcelInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportRepairRequestWorkViewModel> RepairRequests { get; set; }
        public List<ReportWorkRepairRequestViewModel> Works { get; set; }

    }
}
