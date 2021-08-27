using BusinessLogic.BindingModels;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseImplement.Implements
{
    public class WorkStorage : IWorkStorage
    {
        public WorkViewModel GetElement(WorkBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new Database())
            {
                var work = context.Works
                    .Include(rec => rec.Worker)
                    .FirstOrDefault(rec => rec.WorkName == model.WorkName || rec.Id == model.Id);
                return work != null ? CreateModel(work) : null;
            }
        }

        public List<WorkViewModel> GetFilteredList(WorkBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new Database())
            {
                return context.Works
                     .Where(rec => (rec.WorkerId == model.WorkerId) || (model.IdToFilter != null && model.IdToFilter.Contains(rec.Id)))
                    .Include(rec => rec.Worker)
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public List<WorkViewModel> GetFullList()
        {
            using (var context = new Database())
            {
                return context.Works
                    .Include(rec => rec.Worker)
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public void Insert(WorkBindingModel model)
        {
            using (var context = new Database())
            {
                context.Works.Add(CreateModel(model, new Work()));
                context.SaveChanges();
            }
        }

        public void Update(WorkBindingModel model)
        {
            using (var context = new Database())
            {
                var work = context.Works.FirstOrDefault(rec => rec.Id == model.Id);
                if (work == null)
                {
                    throw new Exception("Работа не найдена");
                }
                CreateModel(model, work);
                context.SaveChanges();
            }
        }
        public void Delete(WorkBindingModel model)
        {
            using (var context = new Database())
            {
                var work = context.Works.FirstOrDefault(rec => rec.Id == model.Id);
                if (work != null)
                {
                    context.Works.Remove(work);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Работа не найдена");
                }
            }
        }

        private Work CreateModel(WorkBindingModel model, Work work)
        {
            work.WorkerId = model.WorkerId;
            work.WorkName = model.WorkName;
            work.WorkPrice = model.WorkPrice;
            return work;
        }
        private WorkViewModel CreateModel(Work work)
        {
            return new WorkViewModel
            {
                Id = work.Id,
                WorkName = work.WorkName,
                WorkPrice = work.WorkPrice,
                WorkerId = work.WorkerId
            };
        }
    }
}
