using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace WorkerApp
{
    public class Program
    {
        public static WorkerViewModel Worker { get; set; }
        public static string worksIds { get; set; }
        public static List<ReportPaymentViewModel> payments { get; set; }

        public static DateTime DateFrom { get; set; }
        public static DateTime DateTo { get; set; }
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
