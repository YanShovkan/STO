using BusinessLogic.BindingModels;
using BusinessLogic.ViewModels;
using WorkerApp.Models;
using DatabaseImplement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WorkerApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            if (Program.Worker == null)
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
            Program.Worker = null;
            return View();
        }

        [HttpPost]
        public void Enter(string WorkerLogin, string WorkerPassword)
        {
            if (!string.IsNullOrEmpty(WorkerLogin) && !string.IsNullOrEmpty(WorkerPassword))
            {
                Program.Worker = APIWorker.GetRequest<WorkerViewModel>($"api/worker/login?workerLogin={WorkerLogin}&workerPassword={WorkerPassword}");
                if (Program.Worker == null)
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
        public void Register(string WorkerName, string WorkerLogin, string WorkerPassword)
        {
            if (!string.IsNullOrEmpty(WorkerName) && !string.IsNullOrEmpty(WorkerLogin) && !string.IsNullOrEmpty(WorkerPassword))
            {
                APIWorker.PostRequest("api/worker/register", new WorkerBindingModel
                {
                    WorkerName = WorkerName,
                    WorkerPassword = WorkerPassword,
                    WorkerLogin = WorkerLogin
                });
                Response.Redirect("Enter");
                return;
            }
            throw new Exception("Введите логин, пароль и имя");
        }

        [HttpGet]
        public IActionResult Works()
        {
            if (Program.Worker == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Works = APIWorker.GetRequest<List<WorkViewModel>>($"api/worker/getworkerworks?workerId={Program.Worker.Id}");
            return View(APIWorker.GetRequest<List<WorkViewModel>>($"api/worker/getworkerworks?workerId={Program.Worker.Id}"));
        }

        [HttpPost]
        public void Works(List<int> works)
        {
            string temp = null;

            int i = 0;
            foreach (var elem in works)
            {
                temp += elem + " ";
                i++;
            }

            Program.worksIds = temp;
            Response.Redirect("WorksRepairRequests");
        }

        [HttpGet]
        public IActionResult Work(int Id, string ToDo)
        {
            if (ToDo == null)
            {
                return Redirect("~/Home/Works");
            }
            ViewBag.ToDo = ToDo;
            if (ToDo == "создать")
            {
                return View();
            }
            else
            {
                var work = APIWorker.GetRequest<WorkViewModel>($"api/worker/GetWork?workId={Id}");
                ViewBag.WorkName = work.WorkName;
                ViewBag.WorkPrice = work.WorkPrice;
                return View();
            }
        }

        [HttpPost]
        public void Work(int id, string workName, string workPrice, string todo)
        {
            if (!string.IsNullOrEmpty(workName) && !string.IsNullOrEmpty(workPrice))
            {
                if (todo == "создать")
                {
                    APIWorker.PostRequest("api/worker/CreateOrUpdateWork", new WorkBindingModel
                    {
                        WorkName = workName,
                        WorkPrice = Convert.ToDecimal(workPrice),
                        WorkerId = Program.Worker.Id
                    });
                }
                else
                {
                    var work = APIWorker.GetRequest<WorkViewModel>($"api/worker/GetWork?workId={id}");
                    if (todo == "удалить")
                    {
                        APIWorker.PostRequest("api/worker/DeleteWork", new WorkerBindingModel
                        {
                            Id = id
                        });
                    }
                    else if (todo == "изменить")
                    {
                        APIWorker.PostRequest("api/worker/CreateOrUpdateWork", new WorkBindingModel
                        {
                            WorkName = workName,
                            WorkPrice = Convert.ToDecimal(workPrice),
                            Id = work.Id,
                            WorkerId = work.WorkerId
                        });
                    }
                }
            }
            Response.Redirect("Works");
        }

        [HttpGet]
        public IActionResult CostItems()
        {
            if (Program.Worker == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIWorker.GetRequest<List<CostItemViewModel>>($"api/worker/GetCostItems"));
        }

        [HttpGet]
        public IActionResult CostItem(int Id, string ToDo)
        {
            if (ToDo == null)
            {
                return Redirect("~/Home/CostItems");
            }
            ViewBag.ToDo = ToDo;
            if (ToDo == "создать")
            {
                return View();
            }
            else
            {
                var work = APIWorker.GetRequest<CostItemViewModel>($"api/worker/GetCostItem?costItemId={Id}");
                ViewBag.CostItemName = work.CostItemName;
                return View();
            }
        }

        [HttpPost]
        public void CostItem(int id, string costItemName, string todo)
        {
            if (!string.IsNullOrEmpty(costItemName))
            {
                if (todo == "создать")
                {
                    APIWorker.PostRequest("api/worker/CreateOrUpdateCostItem", new CostItemBindingModel
                    {
                        CostItemName = costItemName
                    });
                }
                else
                {
                    var work = APIWorker.GetRequest<WorkViewModel>($"api/worker/GetCostItem?costItemId={id}");
                    if (todo == "удалить")
                    {
                        APIWorker.PostRequest("api/worker/DeleteCostItem", new CostItemBindingModel
                        {
                            Id = id
                        });
                    }
                    else if (todo == "изменить")
                    {
                        APIWorker.PostRequest("api/worker/CreateOrUpdateCostItem", new CostItemBindingModel
                        {
                            CostItemName = costItemName,
                            Id = work.Id
                        });
                    }
                }
            }
            Response.Redirect("CostItems");
        }

        [HttpGet]
        public IActionResult CreateRepairRequestCostItem()
        {
            ViewBag.CostItems = APIWorker.GetRequest<List<CostItemViewModel>>($"api/worker/getCostItems");
            ViewBag.RepairRequests = APIWorker.GetRequest<List<RepairRequestViewModel>>($"api/worker/getRepairRequests");
            return View();
        }


        [HttpPost]
        public void CreateRepairRequestCostItem(int costItem, int repairRequest, int count)
        {
            List<int> costItems = new List<int>();
            costItems.Add(costItem);
            APIWorker.PostRequest("api/worker/replenishRepairRequest", new AddCostItemToRepairRequestBindingModel
            {
                RepairRequestId = repairRequest,
                RepairRequestCostItems = costItems,
                Count = count
            });
            Response.Redirect("CostItems");
        }
        [HttpGet]
        public IActionResult WorksRepairRequests()
        {
            return View(APIWorker.GetRequest<List<ReportWorkRepairRequestViewModel>>($"api/worker/getworksrepairrequests?worksIds={Program.worksIds}"));
        }

        [HttpPost]
        public IActionResult WorksRepairRequests(string todo)
        {
            var list = APIWorker.GetRequest<List<ReportWorkRepairRequestViewModel>>($"api/worker/getworksrepairrequests?worksIds={Program.worksIds}");

            if (todo == "импорт в doc")
            {
                APIWorker.PostRequest("api/worker/toword", new ReportBindingModel
                {
                    FileName = "C:/kurs/workerReport.doc",
                    listRepairRequestsWorks = list
                });
                var fileName = "RepairRequestsList.doc";
                var filePath = "C:/kurs/workerReport.doc";
                return PhysicalFile(filePath, "application/docx", fileName);
            }
            if (todo == "импорт в xls")
            {
                APIWorker.PostRequest("api/worker/toexcel", new ReportBindingModel
                {
                    FileName = "C:/kurs/workerReport.xlsx",
                    listRepairRequestsWorks = list
                });
                var fileName = "RepairRequestsList.xlsx";
                var filePath = "C:/kurs/workerReport.xlsx";
                return PhysicalFile(filePath, "application/xlsx", fileName);
            }
            return Redirect("Index");
        }
        [HttpGet]
        public IActionResult WorksPaymentsReport()
        {
            if (Program.Worker == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        [HttpPost]
        public void WorksPaymentsReport(DateTime DateFrom, DateTime DateTo, string todo)
        {
            Program.DateFrom = DateFrom;
            Program.DateTo = DateTo;

            if (todo == "Показать на экране")
            {
                string data = Program.Worker.Id.ToString() + "!" + DateFrom.ToString() + "!" + DateTo.ToString();
                Program.payments = APIWorker.GetRequest<List<ReportPaymentViewModel>>($"api/worker/getpayments?model={data}");
                Response.Redirect("Report");
            }
        }

        [HttpGet]
        public IActionResult Report()
        {
            return View(Program.payments);
        }

        [HttpPost]
        public void Report(string todo)
        {
            var list = Program.payments;

            APIWorker.PostRequest("api/worker/topdf", new ReportBindingModel
            {
                DateFrom = Program.DateFrom,
                DateTo = Program.DateTo,
                FileName = "c:/kurs/workerReport.pdf",
                listPayments = list
            });

            APIWorker.PostRequest("api/worker/sendToMail", new WorkerBindingModel { WorkerLogin = Program.Worker.WorkerLogin });

            Response.Redirect("Index");
        }
    }
}
