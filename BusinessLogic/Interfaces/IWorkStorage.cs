using BusinessLogic.BindingModels;
using BusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IWorkStorage
    {
        List<WorkViewModel> GetFullList();
        List<WorkViewModel> GetFilteredList(WorkBindingModel model);
        WorkViewModel GetElement(WorkBindingModel model);
        void Insert(WorkBindingModel model);
        void Update(WorkBindingModel model);
        void Delete(WorkBindingModel model);
    }
}