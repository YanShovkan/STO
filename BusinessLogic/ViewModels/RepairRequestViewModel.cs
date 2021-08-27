using System;
using System.Collections.Generic;
using System.ComponentModel;
using BusinessLogic.Enums;

namespace BusinessLogic.ViewModels
{
    public class RepairRequestViewModel
    {
        public int Id { get; set; }
       
        public int ClientId { get; set; }

        [DisplayName("Имя создателя")]
        public string RepairRequestClientName { get; set; }

        [DisplayName("Название")]
        public string RepairRequestName { get; set; }

        [DisplayName("Итоговая цена")]
        public decimal RepairRequestTotalPrice { get; set; }

        [DisplayName("Оплаченная сумма")]
        public decimal RepairRequestTotalPaidSum { get; set; }

        [DisplayName("Дата")]
        public DateTime RepairRequestDate { get; set; }

        [DisplayName("Статус записи")]
        public PaymentStatus RepairRequesStatus { get; set; }

        public Dictionary<int, (string, int, int)> RepairRequestWorks { get; set; }

        public Dictionary<int, (string, int)> RepairRequestCostItems { get; set; }
    }
}
