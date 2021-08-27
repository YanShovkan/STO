using BusinessLogic.BindingModels;
using BusinessLogic.ViewModels;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IWorkerStorage
    {
        List<WorkerViewModel> GetFullList();
        List<WorkerViewModel> GetFilteredList(WorkerBindingModel model);
        WorkerViewModel GetElement(WorkerBindingModel model);
        void Insert(WorkerBindingModel model);
        void Update(WorkerBindingModel model);
        void Delete(WorkerBindingModel model);
    }
}
