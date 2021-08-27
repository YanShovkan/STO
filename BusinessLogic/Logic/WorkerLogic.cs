using BusinessLogic.BindingModels;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Logic
{
    public class WorkerLogic
    {
        private readonly IWorkerStorage workerStorage;
        public WorkerLogic(IWorkerStorage workerStorage)
        {
            this.workerStorage = workerStorage;
        }

        public List<WorkerViewModel> Read(WorkerBindingModel model)
        {
            if (model == null)
            {
                return workerStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<WorkerViewModel> { workerStorage.GetElement(model) };
            }
            return workerStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(WorkerBindingModel model)
        {
            var worker = workerStorage.GetElement(new WorkerBindingModel
            {
                WorkerLogin = model.WorkerLogin
            });
            if (worker != null && worker.Id != model.Id)
            {
                throw new Exception("Уже есть такой пользователь");
            }
            if (model.Id.HasValue)
            {
                workerStorage.Update(model);
            }
            else
            {
                workerStorage.Insert(model);
            }
        }
        public void Delete(WorkerBindingModel model)

        {
            var client = workerStorage.GetElement(new WorkerBindingModel
            {
                Id = model.Id
            });
            if (client == null)
            {
                throw new Exception("Пользователь не найден");
            }
            workerStorage.Delete(model);
        }
        public int CheckPassword(string login, string password)
        {
            var client = workerStorage.GetElement(new WorkerBindingModel
            {
                WorkerLogin = login
            });
            if (client == null)
            {
                throw new Exception("Нет такого пользователя");
            }
            if (client.WorkerPassword != password)
            {
                throw new Exception("Неверный пароль");
            }
            return client.Id;
        }
    }
}
