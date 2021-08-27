using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace ClientApp
{
    public class Program
    {
        public static ClientViewModel Client { get; set; }
        public static string repairRequestsIds { get; set; }

        public static List<ReportCostItemViewModel> costItem { get; set; }
        public static DateTime DateFrom { get; set; }
        public static DateTime DateTo { get; set; }

        public static int work { get; set; }
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
