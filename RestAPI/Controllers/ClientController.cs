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
    public class ClientController : Controller
    {
        private readonly ClientLogic clientLogic;
        private readonly RepairRequestLogic repairRequestLogic;
        private readonly WorkLogic workLogic;
        private readonly ReportLogic reportLogic;
        private readonly MailLogic mailLogic;
        public ClientController(ClientLogic clientLogic, RepairRequestLogic repairRequestLogic, WorkLogic workLogic, ReportLogic reportLogic, MailLogic mailLogic)
        {
            this.clientLogic = clientLogic;
            this.repairRequestLogic = repairRequestLogic;
            this.workLogic = workLogic;
            this.reportLogic = reportLogic;
            this.mailLogic = mailLogic;
        }
        [HttpGet]
        public ClientViewModel Login(string clientLogin, string clientPassword)
            => clientLogic.Read(new ClientBindingModel { ClientLogin = clientLogin, ClientPassword = clientPassword })?[0];

        [HttpPost]
        public void Register(ClientBindingModel model)
        {
            CheckData(model);
            clientLogic.CreateOrUpdate(model);
        }
        private void CheckDate(CreateRepairRequestBindingModel model)
        {
            if (model.RepairRequestDate <= DateTime.Now)
            {
                throw new Exception("Вы пытаетесь оставить заявку на уже прошедшее время");
            }
        }
        private void CheckData(ClientBindingModel model)
        {
            if (!Regex.IsMatch(model.ClientLogin, @"^[A-Za-z0-9]+(?:[._%+-])?[A-Za-z0-9._-]+[A-Za-z0-9]@[A-Za-z0-9]+(?:[.-])?[A-Za-z0-9._-]+\.[A-Za-z]{2,6}$"))
            {
                throw new Exception("В качестве логина должна быть указана почта");
            }
        }
        [HttpGet]
        public List<RepairRequestViewModel> GetClientRepairRequests(int clientId)
            => repairRequestLogic.Read(new RepairRequestBindingModel { ClientId = clientId })?.ToList();

        [HttpGet]
        public List<WorkViewModel> GetWorks()
            => workLogic.Read(null)?.ToList();

        [HttpPost]
        public void CreateRepairRequest(CreateRepairRequestBindingModel model)
        {
            CheckDate(model);
            repairRequestLogic.Create(model);
        }

        [HttpGet]
        public List<RepairRequestWorkViewModel> GetRepairRequestWorks(int clientId)
            => repairRequestLogic.GetListForPayment(new RepairRequestBindingModel { ClientId = clientId });

        [HttpGet]
        public RepairRequestWorkViewModel GetWork(int workId)
            => repairRequestLogic.GetWorkForPayment(new WorkBindingModel { Id = workId });

        [HttpPost]
        public void AddPayment(AddPaymentToRepairRequestBindingModel model)
            => repairRequestLogic.AddPayment(model);

        [HttpGet]
        public List<ReportRepairRequestWorkViewModel> GetRepairRequestsWorks(string repairRequestsIds)
            => reportLogic.GetRepairRequestsWorks(repairRequestsIds);

        [HttpPost]
        public void ToWord(ReportBindingModel model)
            => reportLogic.SaveWorksRepairRequestsToWordFile(model);

        [HttpPost]
        public void ToExcel(ReportBindingModel model)
            => reportLogic.SaveWorksRepairRequestsToExcelFile(model);

        [HttpGet]
        public List<ReportCostItemViewModel> GetCostItems(string model)
            => reportLogic.GetCostItems(model);

        [HttpPost]
        public void ToPdf(ReportBindingModel model)
            => reportLogic.SaveCostItemsToPdfFile(model);

        [HttpPost]
        public void SendToMail(ClientBindingModel model)
            => mailLogic.sendMailToClient(model);
    }
}
