using BusinessLogic.BindingModels;
using BusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface ICostItemStorage
    {
        List<CostItemViewModel> GetFullList();
        List<CostItemViewModel> GetFilteredList(CostItemBindingModel model);
        CostItemViewModel GetElement(CostItemBindingModel model);
        void Insert(CostItemBindingModel model);
        void Update(CostItemBindingModel model);
        void Delete(CostItemBindingModel model);
    }
}