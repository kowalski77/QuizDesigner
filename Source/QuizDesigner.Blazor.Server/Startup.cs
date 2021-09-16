using System;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizDesigner.Application;
using QuizDesigner.Blazor.App.Services;
using QuizDesigner.Blazor.Server.Support;
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
            services.AddPersistence(this.Configuration.GetConnectionString("DefaultConnection"));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt => opt.ExpireTimeSpan = TimeSpan.FromMinutes(10));

            services.AddScoped<TokenProvider>();

            services.AddAzureQueueStorage(this.Configuration);

            services.ConfigureApplicationUser();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}