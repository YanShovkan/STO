using BusinessLogic.BindingModels;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Logic
{
    public class PaymentLogic
    {
        private readonly IPaymentStorage paymentStorage;

        public PaymentLogic(IPaymentStorage paymentStorage)
        {
            this.paymentStorage = paymentStorage;
        }

        public List<PaymentViewModel> Read(PaymentBindingModel model)
        {
            if (model == null)
            {
                return paymentStorage.GetFullList();
            }
            return paymentStorage.GetFilteredList(model);
        }

        public void Create(PaymentBindingModel model)
        {
            model.PaymentDate = DateTime.Now;
            paymentStorage.Insert(model);
        }
    }
}
