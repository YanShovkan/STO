using BusinessLogic.BindingModels;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseImplement.Implements
{
    public class WorkerStorage : IWorkerStorage
    {
        public List<WorkerViewModel> GetFullList()
        {
            using (var context = new Database())
            {
                return context.Workers
                .Select(CreateModel).ToList();
            }
        }

        public List<WorkerViewModel> GetFilteredList(WorkerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new Database())
            {
                return context.Workers
                    .Where(rec =>
                    rec.WorkerName.Contains(model.WorkerName) || (rec.WorkerLogin.Equals(model.WorkerLogin) && rec.WorkerPassword.Equals(model.WorkerPassword)))
                    .Select(CreateModel).ToList();
            }
        }

        public WorkerViewModel GetElement(WorkerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new Database())
            {
                var worker = context.Workers
                .FirstOrDefault(rec => rec.WorkerLogin.Equals(model.WorkerLogin) || rec.Id == model.Id);
                return worker != null ?
                CreateModel(worker) : null;
            }
        }

        public void Insert(WorkerBindingModel model)
        {
            using (var context = new Database())
            {
                context.Workers.Add(CreateModel(model, new Worker()));
                context.SaveChanges();
            }
        }

        public void Update(WorkerBindingModel model)
        {
            using (var context = new Database())
            {
                var element = context.Workers.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Работник не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }

        public void Delete(WorkerBindingModel model)
        {
            using (var context = new Database())
            {
                Worker element = context.Workers.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Workers.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Работник не найден");
                }
            }
        }

        private Worker CreateModel(WorkerBindingModel model, Worker worker)
        {
            worker.WorkerName = model.WorkerName;
            worker.WorkerLogin = model.WorkerLogin;
            worker.WorkerPassword = model.WorkerPassword;
            return worker;
        }

        private WorkerViewModel CreateModel(Worker worker)
        {
            return new WorkerViewModel
            {
                Id = worker.Id,
                WorkerName = worker.WorkerName,
                WorkerLogin = worker.WorkerLogin,
                WorkerPassword = worker.WorkerPassword
            };
        }
    }
}
