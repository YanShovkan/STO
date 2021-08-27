using BusinessLogic.BindingModels;
using BusinessLogic.HelperModels;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Logic
{
    public class ReportLogic
    {
        private readonly IRepairRequestStorage repairRequestStorage;
        private readonly IWorkStorage workStorage;
        private readonly IPaymentStorage paymentStorage;

        public ReportLogic(IRepairRequestStorage repairRequestStorage, IWorkStorage workStorage, IPaymentStorage paymentStorage)
        {
            this.repairRequestStorage = repairRequestStorage;
            this.workStorage = workStorage;
            this.paymentStorage = paymentStorage;
        }

        // Получение списка запросов с работами, которые там содержатся (Клиент)
        public List<ReportRepairRequestWorkViewModel> GetRepairRequestsWorks(string repairrequestsIDs)
        {
            List<int> visitsID = new List<int>();

            if (repairrequestsIDs != null)
            {
                string[] ids = repairrequestsIDs.Split(' ');

                foreach (var id in ids)
                {
                    visitsID.Add(Convert.ToInt32(id));
                }
            }

            var repairRequests = repairRequestStorage.GetFilteredList(new RepairRequestBindingModel { IdToFilter = visitsID });

            var works = workStorage.GetFullList();

            var list = new List<ReportRepairRequestWorkViewModel>();

            foreach (var repairRequest in repairRequests)
            {
                var rec = new ReportRepairRequestWorkViewModel
                {
                    RepairDate = repairRequest.RepairRequestDate,
                    Works = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var work in works)
                {
                    if (repairRequest.RepairRequestWorks.ContainsKey(work.Id))
                    {
                        rec.Works.Add(new Tuple<string, int>(work.WorkName, repairRequest.RepairRequestWorks[work.Id].Item2));
                        rec.TotalCount++;
                    }
                }
                list.Add(rec);
            }

            return list;
        }

        // Получение списка работ с указанием, в каких заявках содержатся (сотрудник)
        public List<ReportWorkRepairRequestViewModel> GetWorksRepairRequests(string worksIDs)
        {
            List<int> worksID = new List<int>();

            if (worksIDs != null)
            {
                string[] ids = worksIDs.Split(' ');

                foreach (var id in ids)
                {
                    worksID.Add(Convert.ToInt32(id));
                }
            }

            var repairRequests = repairRequestStorage.GetFullList();

            var works = workStorage.GetFilteredList(new WorkBindingModel { IdToFilter = worksID });

            var list = new List<ReportWorkRepairRequestViewModel>();

            foreach (var work in works)
            {
                var rec = new ReportWorkRepairRequestViewModel
                {
                    WorkName = work.WorkName,
                    TotalCount = 0,
                    RepairRequests = new List<DateTime>()
                };
                foreach (var repairRequest in repairRequests)
                {
                    if (repairRequest.RepairRequestWorks.ContainsKey(work.Id))
                    {
                        rec.RepairRequests.Add(repairRequest.RepairRequestDate);
                        rec.TotalCount++;
                    }
                }
                list.Add(rec);
            }

            return list;
        }

        // Получение списка оплат за определенный период (сотрудник)
        public List<ReportPaymentViewModel> GetPayments(string model)
        {
            string[] strs = model.Split('!');

            int workerId = Convert.ToInt32(strs[0]);
            DateTime DateFrom = Convert.ToDateTime(strs[1]);
            DateTime DateTo = Convert.ToDateTime(strs[2]);

            List<WorkViewModel> list = workStorage.GetFilteredList(new WorkBindingModel { WorkerId = workerId });
            List<int> ids = list.Select(rec => rec.Id).ToList();

            return paymentStorage.GetFilteredList(new PaymentBindingModel
            {
                DateFrom = DateFrom,
                DateTo = DateTo,
                Ids = ids
            })
            .Select(x => new ReportPaymentViewModel
            {
                PaymentDate = x.PaymentDate,
                WorkName = x.WorkName,
                ClientName = x.ClientName,
                Sum = x.Sum,
            })
           .ToList();
        }

        // Получение списка статей затрат на визиты за определенный период (клиент)
        public List<ReportCostItemViewModel> GetCostItems(string model)
        {
            string[] strs = model.Split('!');
            List<RepairRequestViewModel> repairRequests = repairRequestStorage.GetFilteredList(new RepairRequestBindingModel
            {
                ClientId = Convert.ToInt32(strs[0]),
                DateFrom = Convert.ToDateTime(strs[1]),
                DateTo = Convert.ToDateTime(strs[2])
            });

            List<ReportCostItemViewModel> costItems = new List<ReportCostItemViewModel>();

            foreach (var repairRequest in repairRequests)
            {
                foreach (var costItem in repairRequest.RepairRequestCostItems)
                {
                    costItems.Add(new ReportCostItemViewModel
                    {
                        CostItemName = costItem.Value.Item1,
                        ClientName = repairRequest.RepairRequestClientName,
                        RepairDate = repairRequest.RepairRequestDate,
                        Count = costItem.Value.Item2
                    });
                }
            }

            return costItems;
        }

        // Сохранение работ по запросам в файл-doc
        public void SaveWorksRepairRequestsToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateClientDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список работ по выбранным запросам",
                RepairRequests = model.listWorksRepairRequests
            });
        }

        // Сохранение запросов по работам в файл-doc
        public void SaveRepairRequestsWorksToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateWorkerDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список запросов по выбранным работам",
                Works = model.listRepairRequestsWorks
            });
        }

        // Сохранение работ по запросам в файл-xls
        public void SaveWorksRepairRequestsToExcelFile(ReportBindingModel model)
        {
            SaveToExсel.CreateClientDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список услуг по выбранным визитам",
                RepairRequests = model.listWorksRepairRequests
            });
        }

        // Сохранение запросов по работам в файл-xls
        public void SaveRepairRequestsWorksToExcelFile(ReportBindingModel model)
        {
            SaveToExсel.CreateWorkerDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список запросов по выбранным работам",
                Works = model.listRepairRequestsWorks
            });
        }

        // Сохранение оплат за период в файл-Pdf

        public void SavePaymentsToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateWorkerDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список оплат за период",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Payments = model.listPayments
            });
        }

        // Сохранение затрат по визитам за период в файл-Pdf
        public void SaveCostItemsToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateClientDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список статей затрат по запросам за период",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                CostItems = model.listCostItems
            });
        }
    }
}