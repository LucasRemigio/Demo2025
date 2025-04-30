// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Globalization;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Emails;
using engimatrix.ResponseMessages;
using engimatrix.Utils;
using static engimatrix.Utils.Cache;
using Exception = System.Exception;
using Engimatrix.Processes;
using engimatrix.Models.Orquestration;

namespace engimatrix
{
    public class Program
    {
        public static async Task ServerStartAsync()
        {
            ConfigManager.LoadConfigs();

            if (ConfigManager.isProduction)
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
            }
            else
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            }

            SqlConn.ConfigureConnection(
                ConfigManager.sqlServer,
                ConfigManager.sqlPort,
                ConfigManager.sqlDatabase,
                ConfigManager.sqlUser,
                ConfigManager.sqlPassword
                );

            await ConfigManager.LoadDinamicConfigsAsync();
            await ConfigManager.LoadPlatformSettingsAsync();
            OrderCache.InitializeCache();
            OrderCache.SaveCacheOnShutdown();
            EmailService.StartEmailService();
            ResponseMessage.LoadResponseMessages();
            ScriptsModel.InitializeTimer();
        }

        private static System.Threading.Timer checkEmails = null;
        public static bool checkEmailsInitialized = false;

        public static async Task Main(string[] args)
        {
            // Usando a cultura Invariant para garantir que o ponto seja usado como separador decimal
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");

            await ServerStartAsync();

            ProcessEmailCategorization();

            ExtractProductsFromPendingRequests();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(o =>
                    {
                        o.ConfigureHttpsDefaults(o => o.ClientCertificateMode = ClientCertificateMode.NoCertificate);
                        // 2.5 GB
                        o.Limits.MaxRequestBodySize = 2500000000;
                    });
                });

        public static async Task ProcessEmailCategorization()
        {
            Log.Debug("#Proccess - ProccessEmailCategorization started successfully");
            while (true)
            {
                try
                {
                    foreach (KeyValuePair<string, string> emailAcc in ConfigManager.MailboxCredentials)
                    {
                        // Key - account address / Value - acccount password
                        await MasterFerro.CategorizeFolderAsync(emailAcc.Key, ConfigManager.InboxFolder);
                    }
                }
                catch (Exception e)
                {
                    Log.Error("CRITICAL ERROR -" + e);
                }

                await Task.Delay(TimeSpan.FromSeconds(15));
            }
        }

        public static async Task ExtractProductsFromPendingRequests()
        {
            Log.Debug("#Proccess - ExtractProductsFromPendingRequests started successfully");
            Log.Warning("\n\n\n#WARNING! - IF PRICING ALGORITHM IS NOT FINISHED, TURN OFF IMMEDIATELY\n\n\n");
            while (true)
            {
                try
                {
                    /*
                    * WARNING: This line should be commented out in production
                    */
                    await ProcessOrders.CreateOrderFromPendingRequests();
                    await ProcessOrders.SendEmailToOrdersConfirmed();
                }
                catch (Exception e)
                {
                    Log.Error("CRITICAL ERROR -" + e);
                }

                await Task.Delay(TimeSpan.FromSeconds(15));
            }
        }
    }
}
