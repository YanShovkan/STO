using BusinessLogic.BindingModels;
using BusinessLogic.ViewModels;
using ClientApp.Models;
using DatabaseImplement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClientApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            if (Program.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Enter()
        {
            Program.Client = null;
            return View();
        }

        [HttpPost]
        public void Enter(string ClientLogin, string ClientPassword)
        {
            if (!string.IsNullOrEmpty(ClientLogin) && !string.IsNullOrEmpty(ClientPassword))
            {
                Program.Client = APIClient.GetRequest<ClientViewModel>($"api/client/login?clientLogin={ClientLogin}&clientPassword={ClientPassword}");
                if (Program.Client == null)
                {
                    throw new Exception("Неверный логин/пароль");
                }
                Response.Redirect("Index");
                return;
            }
            throw new Exception("Введите логин, пароль");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public void Register(string ClientName, string ClientLogin, string ClientPassword)
        {
            if (!string.IsNullOrEmpty(ClientName) && !string.IsNullOrEmpty(ClientLogin) && !string.IsNullOrEmpty(ClientPassword))
            {
                APIClient.PostRequest("api/client/register", new ClientBindingModel
                {
                    ClientName = ClientName,
                    ClientPassword = ClientPassword,
                    ClientLogin = ClientLogin
                });
                Response.Redirect("Enter");
                return;
            }
            throw new Exception("Введите логин, пароль и имя");
        }

        [HttpGet]
        public IActionResult RepairRequests()
        {
            if (Program.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.RepairRequests = APIClient.GetRequest<List<RepairRequestViewModel>>($"api/client/getclientrepairrequests?clientId={Program.Client.Id}");
            return View(APIClient.GetRequest<List<RepairRequestViewModel>>($"api/client/getclientrepairrequests?clientId={Program.Client.Id}"));
        }


        [HttpPost]
        public void RepairRequests(List<int> repairRequests)
        {
            string temp = null;

            int i = 0;
            foreach (var elem in repairRequests)
            {
                temp += elem + " ";
                i++;
            }

            Program.repairRequestsIds = temp;
            Response.Redirect("RepairRequestsWorks");
        }

        [HttpGet]
        public IActionResult CreateRepairRequest()
        {
            ViewBag.Works = APIClient.GetRequest<List<WorkViewModel>>($"api/client/getworks?");
            return View(APIClient.GetRequest<List<WorkViewModel>>($"api/client/getworks?"));
        }

        [HttpPost]
        public void CreateRepairRequest(List<int> works, DateTime date)
        {
            APIClient.PostRequest("api/client/createrepairrequest", new CreateRepairRequestBindingModel
            {
                ClientId = Program.Client.Id,
                RepairRequestWorks = works,
                RepairRequestDate = date
            });
            Response.Redirect("RepairRequests");
        }

        [HttpGet]
        public IActionResult CreatePayment()
        {
            if (Program.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Works = APIClient.GetRequest<List<RepairRequestWorkViewModel>>($"api/client/getrepairrequestworks?clientId={Program.Client.Id}");
            return View();
        }

        [HttpPost]
        public int[] Calc(int work)
        {
            int[] results = new int[2];

            RepairRequestWorkViewModel exactWork = APIClient.GetRequest<RepairRequestWorkViewModel>($"api/client/getwork?workId={work}");
            results[0] = exactWork.RepairRequestPaidSum;
            results[1] = exactWork.WorkCost - exactWork.RepairRequestPaidSum;

            return results;
        }


        [HttpPost]
        public void CreatePayment(int work, int leftToPay, int sum, string todo)
        {
            if (todo == "Оплатить")
            {
                if ((sum == 0) || (sum > leftToPay))
                {
                    throw new Exception("Вы пытаетесь оплатить уже оплаченную работу или сумма вашей оплаты превышает долг");
                }

                var model = new AddPaymentToRepairRequestBindingModel
                {
                    WorkId = work,
                    Sum = sum
                };

                APIClient.PostRequest("api/client/AddPayment", model);

                Response.Redirect("Index");
            }
        }

        [HttpGet]
        public IActionResult RepairRequestsWorks()
        {
            return View(APIClient.GetRequest<List<ReportRepairRequestWorkViewModel>>($"api/client/GetRepairRequestsWorks?repairRequestsIds={Program.repairRequestsIds}"));
        }
        [HttpPost]
        public IActionResult RepairRequestsWorks(string todo)
        {
            var list = APIClient.GetRequest<List<ReportRepairRequestWorkViewModel>>($"api/client/GetRepairRequestsWorks?repairRequestsIds={Program.repairRequestsIds}");

            if (todo == "импорт в doc")
            {
                APIClient.PostRequest("api/client/toword", new ReportBindingModel
                {
                    FileName = "C:/kurs/clientReport.doc",
                    listWorksRepairRequests = list
                });

                var fileName = "WorksList.doc";
                var filePath = "C:/kurs/clientReport.doc";
                return PhysicalFile(filePath, "application/docx", fileName);
            }
            if (todo == "импорт в xls")
            {
                APIClient.PostRequest("api/client/toexcel", new ReportBindingModel
                {
                    FileName = "C:/kurs/clientReport.xlsx",
                    listWorksRepairRequests = list
                });

                var fileName = "WorksList.xlsx";
                var filePath = "C:/kurs/clientReport.xlsx";
                return PhysicalFile(filePath, "application/xlsx", fileName);
            }
            return Redirect("Index");
        }
        [HttpGet]
        public IActionResult RepairRequestsCostItemsReport()
        {
            if (Program.Client == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        [HttpPost]
        public void RepairRequestsCostItemsReport(DateTime DateFrom, DateTime DateTo, string todo)
        {
            Program.DateFrom = DateFrom;
            Program.DateTo = DateTo;

            if (todo == "Показать на экране")
            {
                string data = Program.Client.Id.ToString() + "!" + DateFrom.ToString() + "!" + DateTo.ToString();
                Program.costItem = APIClient.GetRequest<List<ReportCostItemViewModel>>($"api/client/getcostitems?model={data}");
                Response.Redirect("Report");
            }
        }

        [HttpGet]
        public IActionResult Report()
        {
            return View(Program.costItem);
        }

        [HttpPost]
        public void Report(string todo)
        {
            var list = Program.costItem;

            APIClient.PostRequest("api/client/topdf", new ReportBindingModel
            {
                DateFrom = Program.DateFrom,
                DateTo = Program.DateTo,
                FileName = "C:/kurs/clientReport.pdf",
                listCostItems = list
            });

            APIClient.PostRequest("api/client/sendToMail", new ClientBindingModel
            {
                ClientLogin = Program.Client.ClientLogin
            });

            Response.Redirect("Index");
        }
    }
}
