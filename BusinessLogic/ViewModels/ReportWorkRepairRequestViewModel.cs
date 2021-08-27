using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BusinessLogic.ViewModels
{
    public class ReportWorkRepairRequestViewModel
    {
        [DisplayName("Название работы")]
        public string WorkName { get; set; }

        [DisplayName("Общее кол-во")]
        public int TotalCount { get; set; }

        public List<DateTime> RepairRequests { get; set; }
    }
}
