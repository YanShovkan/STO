using BusinessLogic.BindingModels;
using BusinessLogic.Logic;
using BusinessLogic.Enums;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseImplement.Implements
{
    public class RepairRequestStorage : IRepairRequestStorage
    {
        public RepairRequestViewModel GetElement(RepairRequestBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new Database())
            {
                var repairRequest = context.RepairRequests
                    .Include(rec => rec.Client)
                    .Include(rec => rec.RepairRequestWorks)
                    .ThenInclude(rec => rec.Work)
                    .Include(rec => rec.RepairRequestCostItems)
                    .ThenInclude(rec => rec.CostItem)
                    .FirstOrDefault(rec => rec.RepairRequestDate == model.RepairRequestDate || rec.Id == model.Id);

                return repairRequest != null ? CreateModel(repairRequest) : null;
            }
        }

        public List<RepairRequestViewModel> GetFilteredList(RepairRequestBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new Database())
            {
                return context.RepairRequests
                  .Include(rec => rec.Client)
                   .Include(rec => rec.RepairRequestWorks)
                   .ThenInclude(rec => rec.Work)
                   .Include(rec => rec.RepairRequestCostItems)
                   .ThenInclude(rec => rec.CostItem)
                   .Where(rec => (model.IdToFilter != null && model.IdToFilter.Contains(rec.Id))
                   || (model.DateFrom.HasValue && model.DateTo.HasValue && rec.RepairRequestDate >= model.DateFrom && rec.RepairRequestDate <= model.DateTo)
                            && rec.ClientId == model.ClientId
                        || (!model.DateFrom.HasValue && !model.DateTo.HasValue && model.ClientId.HasValue && rec.ClientId == model.ClientId))
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public List<RepairRequestViewModel> GetFullList()
        {
            using (var context = new Database())
            {
                return context.RepairRequests
                    .Include(rec => rec.Client)
                    .Include(rec => rec.RepairRequestWorks)
                    .ThenInclude(rec => rec.Work)
                    .Include(rec => rec.RepairRequestCostItems)
                    .ThenInclude(rec => rec.CostItem)
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public void Insert(RepairRequestBindingModel model)
        {
            using (var context = new Database())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new RepairRequest(), context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(RepairRequestBindingModel model)
        {
            //только для смены статуса, не выносим кнопку на веб для изменения услуг

            using (var context = new Database())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var repairRequest = context.RepairRequests.FirstOrDefault(rec => rec.Id == model.Id);
                        if (repairRequest == null)
                        {
                            throw new Exception("Запрос не найден");
                        }
                        CreateModel(model, repairRequest, context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private RepairRequest CreateModel(RepairRequestBindingModel model, RepairRequest repairRequest, Database context)
        {
            if (model.ClientId != null)
            {
                repairRequest.ClientId = model.ClientId.Value;
            }
            if(model.RepairRequestDate != DateTime.MinValue)
            {
                repairRequest.RepairRequestDate = model.RepairRequestDate;
            }

            if (repairRequest.RepairRequestStatus != PaymentStatus.Оплачен || repairRequest.RepairRequestStatus != PaymentStatus.ЧастичноОплачен)
            {
                repairRequest.RepairRequestStatus = model.RepairRequestStatus;
            }

            if (repairRequest.Id == 0)
            {
                context.RepairRequests.Add(repairRequest);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var RepairRequestWorks = context.RepairRequestWorks
                    .Where(rec => rec.RepairRequestId == model.Id)
                    .ToList();

                foreach (var updWork in RepairRequestWorks)
                {
                    if (model.RepairRequestWorks.ContainsKey(updWork.WorkId))
                    {
                        updWork.PaidSum += model.RepairRequestWorks[updWork.WorkId].Item2;
                        model.RepairRequestWorks.Remove(updWork.WorkId);
                    }
                }
                context.SaveChanges();

                var RepairRequestCostItems = context.RepairRequestCostItems
                    .Where(rec => rec.RepairRequestId == model.Id)
                    .ToList();

                context.SaveChanges();

                foreach (var updCostItem in RepairRequestCostItems)
                {
                    if (model.RepairRequestCostItems.ContainsKey(updCostItem.CostItemId))
                    {
                        updCostItem.CostItemCount += model.RepairRequestCostItems[updCostItem.CostItemId].Item2;
                        model.RepairRequestCostItems.Remove(updCostItem.CostItemId);
                    }
                }
                context.SaveChanges();

            }

            int TotCost = 0;

            foreach (var work in model.RepairRequestWorks)
            {
                context.RepairRequestWorks.Add(new RepairRequestWork
                {
                    RepairRequestId = repairRequest.Id,
                    WorkId = work.Key,
                    WorkCost = (int)context.Works.Where(rec => rec.Id == work.Key).ToList()?[0].WorkPrice,
                    PaidSum = work.Value.Item3
                });
                context.SaveChanges();
                TotCost += (int)context.Works.Where(rec => rec.Id == work.Key).ToList()?[0].WorkPrice;
            }

            if (model.RepairRequestCostItems != null)
            {
                foreach (var ci in model.RepairRequestCostItems)
                {
                    context.RepairRequestCostItems.Add(new RepairRequestCostItem
                    {
                        RepairRequestId = model.Id.Value,
                        CostItemId = ci.Key,
                        CostItemCount = ci.Value.Item2,
                    });
                    context.SaveChanges();
                }
            }

            return repairRequest;
        }
        private RepairRequestViewModel CreateModel(RepairRequest repairRequest)
        {
            int Cost = 0;
            int PaidMoney = 0;

            foreach (var work in repairRequest.RepairRequestWorks)
            {
                Cost += work.WorkCost;
                PaidMoney += work.PaidSum;
            }

            return new RepairRequestViewModel
            {
                Id = repairRequest.Id,
                ClientId = repairRequest.ClientId,
                RepairRequestClientName = repairRequest.Client.ClientName,
                RepairRequestDate = repairRequest.RepairRequestDate,
                RepairRequesStatus = repairRequest.RepairRequestStatus,
                RepairRequestWorks = repairRequest.RepairRequestWorks.ToDictionary(work => work.WorkId, work => (work.Work.WorkName, work.WorkCost, work.PaidSum)),
                RepairRequestCostItems = repairRequest.RepairRequestCostItems.ToDictionary(work => work.CostItemId, work => (work.CostItem.CostItemName, work.CostItemCount)),
                RepairRequestTotalPrice = Cost,
                RepairRequestTotalPaidSum = PaidMoney
            };
        }

        public List<RepairRequestWorkViewModel> GetFullListWithWorks(RepairRequestBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new Database())
            {
                return context.RepairRequestWorks
                    .Include(rec => rec.RepairRequest)
                    .Include(rec => rec.Work)
                    .Where(rec => (model.ClientId.HasValue && rec.RepairRequest.ClientId == model.ClientId)
                        || (!model.ClientId.HasValue && model.Id == rec.RepairRequestId))
                    .Select(CreateAddModel)
                    .ToList();
            }
        }

        private RepairRequestWorkViewModel CreateAddModel(RepairRequestWork repairRequestCostItem)
        {
            return new RepairRequestWorkViewModel
            {
                Id = repairRequestCostItem.Id,
                RepairRequestId = repairRequestCostItem.RepairRequestId,
                WorkId = repairRequestCostItem.WorkId,
                RepairRequestWorkName = $"Работа - {repairRequestCostItem.Work.WorkName} запланированная на {repairRequestCostItem.RepairRequest.RepairRequestDate}",
                RepairRequestPaidSum = repairRequestCostItem.PaidSum,
                WorkCost = repairRequestCostItem.WorkCost
            };
        }

        public RepairRequestWorkViewModel GetWorkForPayment(WorkBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new Database())
            {
                return context.RepairRequestWorks
                    .Include(rec => rec.RepairRequest)
                    .Include(rec => rec.Work)
                    .Where(rec => (rec.Id == model.Id))
                    .Select(CreateAddedModel).ToList()?[0];
            }
        }

        private RepairRequestWorkViewModel CreateAddedModel(RepairRequestWork repairRequestWork)
        {
            return new RepairRequestWorkViewModel
            {
                Id = repairRequestWork.Id,
                RepairRequestId = repairRequestWork.RepairRequestId,
                WorkId = repairRequestWork.WorkId,
                RepairRequestPaidSum = repairRequestWork.PaidSum,
                WorkCost = repairRequestWork.WorkCost
            };
        }

        public void AddPayment(AddPaymentToRepairRequestBindingModel model)
        {
            IPaymentStorage paymentStorage = new PaymentStorage();

            var context = new Database();

            RepairRequestWork work = context.RepairRequestWorks
                .Include(rec => rec.RepairRequest)
                .Include(rec => rec.Work)
                .Where(rec => rec.Id == model.WorkId).ToList()?[0];

            if (work != null)
            {
                paymentStorage.Insert(new PaymentBindingModel
                {
                    ClientId = work.RepairRequest.ClientId,
                    WorkId = work.Work.Id,
                    WorkInRepairRequestId = model.WorkId,
                    PaymentDate = DateTime.Now,
                    Sum = model.Sum
                });
            }

            int TotalPaidSum = 0;
            int TotalCost = 0;

            var listofworks = context.RepairRequestWorks
                .Include(rec => rec.RepairRequest)
                .Include(rec => rec.Work)
                .Where(rec => rec.RepairRequestId == work.RepairRequest.Id).ToList();

            foreach (var updWork in listofworks)
            {
                TotalPaidSum += updWork.PaidSum;
                TotalCost += updWork.WorkCost;
            }

            TotalPaidSum += model.Sum;

            context.SaveChanges();

            if (TotalPaidSum == TotalCost)
            {
                work.RepairRequest.RepairRequestStatus = PaymentStatus.Оплачен;
            }
            else
            {
                work.RepairRequest.RepairRequestStatus = PaymentStatus.ЧастичноОплачен;
            }
            context.SaveChanges();
        }

    }
}