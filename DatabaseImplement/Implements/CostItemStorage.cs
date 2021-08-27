using BusinessLogic.BindingModels;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseImplement.Implements
{
    public class CostItemStorage : ICostItemStorage
    {
        public CostItemViewModel GetElement(CostItemBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new Database())
            {
                var costItem = context.CostItems
                    .FirstOrDefault(rec => rec.CostItemName == model.CostItemName || rec.Id == model.Id);
                return costItem != null ? CreateModel(costItem) : null;
            }
        }

        public List<CostItemViewModel> GetFilteredList(CostItemBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new Database())
            {
                return context.CostItems
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public List<CostItemViewModel> GetFullList()
        {
            using (var context = new Database())
            {
                return context.CostItems
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public void Insert(CostItemBindingModel model)
        {
            using (var context = new Database())
            {
                context.CostItems.Add(CreateModel(model, new CostItem()));
                context.SaveChanges();
            }
        }

        public void Update(CostItemBindingModel model)
        {
            using (var context = new Database())
            {
                var costItem = context.CostItems.FirstOrDefault(rec => rec.Id == model.Id);
                if (costItem == null)
                {
                    throw new Exception("Статья затрат не найдена");
                }
                CreateModel(model, costItem);
                context.SaveChanges();
            }
        }

        public void Delete(CostItemBindingModel model)
        {
            using (var context = new Database())
            {
                var costItem = context.CostItems.FirstOrDefault(rec => rec.Id == model.Id);
                if (costItem != null)
                {
                    context.CostItems.Remove(costItem);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Статья затрат не найдена");
                }
            }
        }

        private CostItem CreateModel(CostItemBindingModel model, CostItem costItem)
        {
            costItem.CostItemName = model.CostItemName;
            return costItem;
        }
        private CostItemViewModel CreateModel(CostItem costItem)
        {
            var context = new Database();

            var CostItemsCount = context.RepairRequestCostItems.Where(rec => rec.CostItemId == costItem.Id).ToList();

            int TotCount = 0;

            foreach (var ci in CostItemsCount)
            {
                TotCount += ci.CostItemCount;
            }

            return new CostItemViewModel
            {
                Id = costItem.Id,
                CostItemName = costItem.CostItemName,
                CostItemCount = TotCount
            };
        }
    }
}