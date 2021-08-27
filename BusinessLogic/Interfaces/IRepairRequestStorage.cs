using BusinessLogic.BindingModels;
using BusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IRepairRequestStorage
    {
        List<RepairRequestViewModel> GetFullList();
        List<RepairRequestWorkViewModel> GetFullListWithWorks(RepairRequestBindingModel model);
        List<RepairRequestViewModel> GetFilteredList(RepairRequestBindingModel model);
        RepairRequestViewModel GetElement(RepairRequestBindingModel model);
        RepairRequestWorkViewModel GetWorkForPayment(WorkBindingModel model);
        void Insert(RepairRequestBindingModel model);
        void Update(RepairRequestBindingModel model);
        void AddPayment(AddPaymentToRepairRequestBindingModel model);
    }
}