using DemoChatApp.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DemoChatApp.Options;
using DemoChatApp.Interfaces;
using DemoChatApp.Services;
using DemoChatApp.ViewModels;

namespace DemoChatApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var host = CreateHostBuilder().Build();
            Application.Run(new Form1(host.Services));
        }

        static IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .Build();

                    services.AddWindowsFormsBlazorWebView();
                    services.AddDevExpressBlazor(options =>
                    {
                        options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
                        options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
                    });

                    services.Configure<OpenAIOptions>(options => configuration.GetSection(nameof(OpenAIOptions)).Bind(options));

                    services.AddScoped<IChatService, ChatService>();
                    services.AddScoped<IOpenAIService, OpenAIService>();
                    services.AddScoped<IMessageService, MessageService>();
                    services.AddSingleton<ChatViewModel>();

                    services.AddDbContext<ChatDbContext>(options =>
                    
                        options.UseSqlServer(context.Configuration.GetConnectionString("MyDbContext"))
                    );
                    //services.AddSingleton<WeatherForecastService>();
                });
            return builder;
        }
    }
}