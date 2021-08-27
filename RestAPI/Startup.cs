using BusinessLogic.Logic;
using BusinessLogic.Interfaces;
using DatabaseImplement.Implements;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RestAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IClientStorage, ClientStorage>();
            services.AddTransient<IWorkerStorage, WorkerStorage>();
            services.AddTransient<IWorkStorage, WorkStorage>();
            services.AddTransient<ICostItemStorage, CostItemStorage>();
            services.AddTransient<IPaymentStorage, PaymentStorage>();
            services.AddTransient<IRepairRequestStorage, RepairRequestStorage>();

            services.AddTransient<WorkLogic>();
            services.AddTransient<ClientLogic>();
            services.AddTransient<WorkerLogic>();
            services.AddTransient<CostItemLogic>();
            services.AddTransient<PaymentStorage>();
            services.AddTransient<RepairRequestLogic>();
            services.AddTransient<MailLogic>();
            services.AddTransient<ReportLogic>();

            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
