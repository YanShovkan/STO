using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BusinessLogic.ViewModels
{
    public class ReportRepairRequestWorkViewModel
    {
        [DisplayName("Дата ремонта")]
        public DateTime RepairDate { get; set; }
        [DisplayName("Общее количество")]
        public int TotalCount { get; set; }
        public List<Tuple<string, int>> Works { get; set; }
    }
}
