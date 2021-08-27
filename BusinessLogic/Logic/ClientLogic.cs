using BusinessLogic.BindingModels;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Logic
{
    public class ClientLogic
    {
        private readonly IClientStorage clientStorage;
        public ClientLogic(IClientStorage clientStorage)
        {
            this.clientStorage = clientStorage;
        }

        public List<ClientViewModel> Read(ClientBindingModel model)
        {
            if (model == null)
            {
                return clientStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<ClientViewModel> { clientStorage.GetElement(model) };
            }
            return clientStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(ClientBindingModel model)
        {
            var client = clientStorage.GetElement(new ClientBindingModel
            {
                ClientLogin = model.ClientLogin
            });
            if (client != null && client.Id != model.Id)
            {
                throw new Exception("Уже есть такой пользователь");
            }
            if (model.Id.HasValue)
            {
                clientStorage.Update(model);
            }
            else
            {
                clientStorage.Insert(model);
            }
        }
        public void Delete(ClientBindingModel model)

        {
            var client = clientStorage.GetElement(new ClientBindingModel
            {
                Id = model.Id
            });
            if (client == null)
            {
                throw new Exception("Пользователь не найден");
            }
            clientStorage.Delete(model);
        }
        public int CheckPassword(string login, string password)
        {
            var client = clientStorage.GetElement(new ClientBindingModel
            {
                ClientLogin = login
            });
            if (client == null)
            {
                throw new Exception("Нет такого пользователя");
            }
            if (client.ClientPassword != password)
            {
                throw new Exception("Неверный пароль");
            }
            return client.Id;
        }
    }
}
