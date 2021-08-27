using BusinessLogic.BindingModels;
using BusinessLogic.Logic;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RestAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkerController : Controller
    {
        private readonly WorkerLogic workerLogic;
        private readonly WorkLogic workLogic;
        private readonly CostItemLogic costItemLogic;
        private readonly RepairRequestLogic repairRequestLogic;
        private readonly ReportLogic reportLogic;
        private readonly MailLogic mailLogic;
        public WorkerController(MailLogic mailLogic, ReportLogic reportLogic, WorkerLogic workerLogic, WorkLogic workLogic, CostItemLogic costItemLogic, RepairRequestLogic repairRequestLogic)
        {
            this.workerLogic = workerLogic;
            this.workLogic = workLogic;
            this.costItemLogic = costItemLogic;
            this.repairRequestLogic = repairRequestLogic;
            this.reportLogic = reportLogic;
            this.mailLogic = mailLogic;
        }
        [HttpGet]
        public WorkerViewModel Login(string workerLogin, string workerPassword)
            => workerLogic.Read(new WorkerBindingModel { WorkerLogin = workerLogin, WorkerPassword = workerPassword })?[0];

        [HttpPost]
        public void Register(WorkerBindingModel model)
        {
            CheckData(model);
            workerLogic.CreateOrUpdate(model);
        }

        private void CheckData(WorkerBindingModel model)
        {
            if (!Regex.IsMatch(model.WorkerLogin, @"^[A-Za-z0-9]+(?:[._%+-])?[A-Za-z0-9._-]+[A-Za-z0-9]@[A-Za-z0-9]+(?:[.-])?[A-Za-z0-9._-]+\.[A-Za-z]{2,6}$"))
            {
                throw new Exception("В качестве логина должна быть указана почта");
            }
        }
        [HttpGet]
        public List<WorkViewModel> getWorkerWorks(int workerId)
           => workLogic.Read(new WorkBindingModel { WorkerId = workerId })?.ToList();
        
        [HttpGet]
        public WorkViewModel GetWork(int workId)
            => workLogic.Read(new WorkBindingModel { Id = workId })?[0];

        [HttpPost]
        public void CreateOrUpdateWork(WorkBindingModel model)
            => workLogic.CreateOrUpdate(model);

        [HttpPost]
        public void DeleteWork(WorkBindingModel model)
            => workLogic.Delete(model);

        [HttpGet]
        public List<CostItemViewModel> GetCostItems()
            => costItemLogic.Read(null);

        [HttpGet]
        public CostItemViewModel GetCostItem(int costItemId)
            => costItemLogic.Read(new CostItemBindingModel { Id = costItemId })?[0];

        [HttpPost]
        public void CreateOrUpdateCostItem(CostItemBindingModel model)
            => costItemLogic.CreateOrUpdate(model);

        [HttpPost]
        public void DeleteCostItem(CostItemBindingModel model)
            => costItemLogic.Delete(model);

        [HttpGet]
        public List<RepairRequestViewModel> GetRepairRequests()
            => repairRequestLogic.Read(null)?.ToList();

        [HttpPost]
        public void ReplenishRepairRequest(AddCostItemToRepairRequestBindingModel model)
            => repairRequestLogic.Replenish(model);
    
        [HttpGet]
        public List<ReportWorkRepairRequestViewModel> GetWorksRepairRequests(string worksIds)
            => reportLogic.GetWorksRepairRequests(worksIds);

        [HttpPost]
        public void ToWord(ReportBindingModel model)
            => reportLogic.SaveRepairRequestsWorksToWordFile(model);

        [HttpPost]
        public void ToExcel(ReportBindingModel model)
            => reportLogic.SaveRepairRequestsWorksToExcelFile(model);

        [HttpGet]
        public List<ReportPaymentViewModel> GetPayments(string model)
            => reportLogic.GetPayments(model);

        [HttpPost]
        public void ToPdf(ReportBindingModel model)
            => reportLogic.SavePaymentsToPdfFile(model);

        [HttpPost]
        public void SendToMail(WorkerBindingModel model)
            => mailLogic.sendMailToWorker(model);
    }
}
