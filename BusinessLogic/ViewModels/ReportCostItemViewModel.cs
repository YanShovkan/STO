using System;
using System.ComponentModel;

namespace BusinessLogic.ViewModels
{
    public class ReportCostItemViewModel
    {
        [DisplayName("Стаья затрат")]
        public string CostItemName { get; set; }

        [DisplayName("Имя клиента")]
        public string ClientName { get; set; }

        [DisplayName("Дата визита")]
        public DateTime RepairDate { get; set; }

        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}