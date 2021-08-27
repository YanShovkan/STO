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
    public class PaymentStorage : IPaymentStorage
    {
        public List<PaymentViewModel> GetFilteredList(PaymentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new Database())
            {
                return context.Payments
                    .Where(rec => rec.PaymentDate >= model.DateFrom && rec.PaymentDate <= model.DateTo
                    && (model.Ids != null && model.Ids.Contains(rec.WorkId)))
                    .Include(rec => rec.Client)
                    .Include(rec => rec.Work)
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public List<PaymentViewModel> GetFullList()
        {
            using (var context = new Database())
            {
                return context.Payments
                    .Include(rec => rec.Client)
                    .Include(rec => rec.Work)
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public void Insert(PaymentBindingModel model)
        {
            using (var context = new Database())
            {
                context.Payments.Add(CreateModel(model, new Payment()));

                var exactWork = context.RepairRequestWorks.FirstOrDefault(rec => rec.Id == model.WorkInRepairRequestId);
                if (exactWork == null)
                {
                    throw new Exception("Услуга не найдена");
                }
                else
                {
                    exactWork.PaidSum += model.Sum;
                }
                context.SaveChanges();
            }
        }

        private Payment CreateModel(PaymentBindingModel model, Payment payment)
        {
            payment.ClientId = model.ClientId;
            payment.WorkId = model.WorkId;
            payment.PaymentDate = model.PaymentDate;
            payment.Sum = model.Sum;
            return payment;
        }
        private PaymentViewModel CreateModel(Payment payment)
        {
            return new PaymentViewModel
            {
                Id = payment.Id,
                ClientId = payment.ClientId,
                WorkId = payment.WorkId,
                ClientName = payment.Client.ClientName,
                WorkName = payment.Work.WorkName,
                PaymentDate = payment.PaymentDate,
                Sum = payment.Sum
            };
        }
    }
}