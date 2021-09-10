using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizDesigner.Application;
using QuizDesigner.Application.Messaging;
using QuizDesigner.AzureServiceBus;
using QuizDesigner.Persistence;

namespace QuizDesigner.Blazor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazorise(options =>
            {
                options.ChangeTextOnKeyPress = true;
                options.DelayTextOnKeyPress = true;
                options.DelayTextOnKeyPressInterval = 1000;
            })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddApplicationServices();

            services.Configure<AzureServiceBusOptions>(this.Configuration.GetSection(nameof(AzureServiceBusOptions)));
            services.AddScoped<IMessagePublisher, AzureServiceBusPublisher>();

            services.AddPersistence(this.Configuration.GetConnectionString("DefaultConnection"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
