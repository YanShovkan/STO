using BusinessLogic.BindingModels;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Logic
{
    public class CostItemLogic
    {
        private readonly ICostItemStorage costItemStorage;

        public CostItemLogic(ICostItemStorage costItemStorage)
        {
            this.costItemStorage = costItemStorage;
        }

        public List<CostItemViewModel> Read(CostItemBindingModel model)
        {
            if (model == null)
            {
                return costItemStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<CostItemViewModel> { costItemStorage.GetElement(model) };
            }
            return costItemStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(CostItemBindingModel model)
        {
            var element = costItemStorage.GetElement(new CostItemBindingModel { CostItemName = model.CostItemName });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть статья затрат с таким названием");
            }
            if (model.Id.HasValue)
            {
                costItemStorage.Update(model);
            }
            else
            {
                costItemStorage.Insert(model);
            }
        }
        public void Delete(CostItemBindingModel model)
        {
            var element = costItemStorage.GetElement(new CostItemBindingModel { Id = model.Id });
            if (element == null)
            {
                throw new Exception("Статья затрат не найдена");
            }
            costItemStorage.Delete(model);
        }
    }
}