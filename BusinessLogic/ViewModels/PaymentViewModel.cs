using System;
using System.ComponentModel;

namespace BusinessLogic.ViewModels
{
    public class PaymentViewModel
    { 
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int WorkId { get; set; }

        [DisplayName("Имя клиента")]
        public string ClientName { get; set; }
        [DisplayName("Название работы")]
        public string WorkName { get; set; }

        [DisplayName("Дата оплаты")]
        public DateTime PaymentDate { get; set; }

        [DisplayName("Сумма оплаты")]
        public decimal Sum { get; set; }
    }
}
