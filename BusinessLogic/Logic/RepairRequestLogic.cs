using BusinessLogic.BindingModels;
using BusinessLogic.Enums;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Logic
{
    public class RepairRequestLogic
    {
        private readonly IRepairRequestStorage repairRequestStorage;

        public RepairRequestLogic(IRepairRequestStorage repairRequestStorage)
        {
            this.repairRequestStorage = repairRequestStorage;
        }

        public List<RepairRequestViewModel> Read(RepairRequestBindingModel model)
        {
            if (model == null)
            {
                return repairRequestStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<RepairRequestViewModel> { repairRequestStorage.GetElement(model) };
            }
            return repairRequestStorage.GetFilteredList(model);
        }

        public void Create(CreateRepairRequestBindingModel model)
        {
            Dictionary<int, (string, int, int)> RepairRequestWorksDictionary = new Dictionary<int, (string, int, int)>();


            foreach (var work in model.RepairRequestWorks)
            {
                RepairRequestWorksDictionary.Add(work, (null, 0, 0));
            }

            repairRequestStorage.Insert(new RepairRequestBindingModel
            {
                ClientId = model.ClientId,
                RepairRequestDate = model.RepairRequestDate,
                RepairRequestStatus = PaymentStatus.Принят,
                RepairRequestWorks = RepairRequestWorksDictionary,
                RepairRequestCostItems = null
            });
        }

        public List<RepairRequestWorkViewModel> GetListForPayment(RepairRequestBindingModel model)
        {
            return repairRequestStorage.GetFullListWithWorks(model);
        }

        public RepairRequestWorkViewModel GetWorkForPayment(WorkBindingModel model)
        {
            return repairRequestStorage.GetWorkForPayment(model);
        }

        public void Replenish(AddCostItemToRepairRequestBindingModel model)
        {
            Dictionary<int, (string, int)> RepairRequestCostItems = new Dictionary<int, (string, int)>();


            foreach (var ci in model.RepairRequestCostItems)
            {
                RepairRequestCostItems.Add(ci, (null, model.Count));
            }

            repairRequestStorage.Update(new RepairRequestBindingModel
            {
                Id = model.RepairRequestId,
                RepairRequestStatus = PaymentStatus.Состоялся,
                RepairRequestCostItems = RepairRequestCostItems,
                RepairRequestWorks = new Dictionary<int, (string, int, int)>()
            });
        }

        public void AddPayment(AddPaymentToRepairRequestBindingModel model)
        {
            repairRequestStorage.AddPayment(model);
        }
    }
}