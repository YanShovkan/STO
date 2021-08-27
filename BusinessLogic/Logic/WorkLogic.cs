using BusinessLogic.BindingModels;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Logic
{
    public class WorkLogic
    {
        private readonly IWorkStorage workStorage;

        public WorkLogic(IWorkStorage workStorage)
        {
            this.workStorage = workStorage;
        }

        public List<WorkViewModel> Read(WorkBindingModel model)
        {
            if (model == null)
            {
                return workStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<WorkViewModel> { workStorage.GetElement(model) };
            }
            return workStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(WorkBindingModel model)
        {
            var element = workStorage.GetElement(new WorkBindingModel { WorkName = model.WorkName });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть услуга с таким названием");
            }
            if (model.Id.HasValue)
            {
                workStorage.Update(model);
            }
            else
            {
                workStorage.Insert(model);
            }
        }
        public void Delete(WorkBindingModel model)
        {
            var element = workStorage.GetElement(new WorkBindingModel { Id = model.Id });
            if (element == null)
            {
                throw new Exception("Услуга не найдена");
            }
            workStorage.Delete(model);
        }
    }
}