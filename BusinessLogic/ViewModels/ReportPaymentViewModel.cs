using System;
using System.ComponentModel;

namespace BusinessLogic.ViewModels
{
    public class ReportPaymentViewModel
    {
        [DisplayName("Дата платежа")]
        public DateTime PaymentDate { get; set; }

        [DisplayName("Имя клиента")]
        public string ClientName { get; set; }

        [DisplayName("Название работы")]
        public string WorkName { get; set; }

        [DisplayName("Сумма")]
        public decimal Sum { get; set; }
    }
}