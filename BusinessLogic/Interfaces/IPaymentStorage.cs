using BusinessLogic.BindingModels;
using BusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IPaymentStorage
    {
        List<PaymentViewModel> GetFullList();
        List<PaymentViewModel> GetFilteredList(PaymentBindingModel model);
        void Insert(PaymentBindingModel model);
    }
}