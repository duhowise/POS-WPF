using System;
using System.ComponentModel.Composition.Hosting;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Reflection;
using Microsoft.Practices.Prism.MefExtensions;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Magentix.Infrastructure.Settings;
using Magentix.Localization.Engine;
using Magentix.Presentation.Common;
using Magentix.Presentation.Common.ModelBase;
using Magentix.Presentation.Common.Services;
using Magentix.Presentation.Properties;
using Magentix.Presentation.Services;
using Magentix.Presentation.Services.Common;
using Magentix.Presentation.Services.Common.DataGeneration;
using Magentix.Services;
using Magentix.Services.Common;
using Magentix.QLicense;
using Magentix.License;

namespace Magentix.Presentation
{
    public class Bootstrapper : MefBootstrapper
    {
        static readonly Mutex Mutex = new Mutex(true, "{aa77c8c4-b8c1-4e61-908f-48c6fb65227d}");

        protected override DependencyObject CreateShell()
        {
            return Container.GetExportedValue<Shell>();
        }

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            var path = System.IO.Path.GetDirectoryName(Application.ResourceAssembly.Location);
            if (path != null)
            {
                AggregateCatalog.Catalogs.Add(new DirectoryCatalog(path, "Magentix.Persistance.dll"));
                AggregateCatalog.Catalogs.Add(new DirectoryCatalog(path, "Magentix.Modules*"));
                AggregateCatalog.Catalogs.Add(new DirectoryCatalog(path, "Magentix.Presentation*"));
                AggregateCatalog.Catalogs.Add(new DirectoryCatalog(path, "Magentix.Services*"));

            }
            LocalSettings.AppPath = path;
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            var moduleInitializationService = ServiceLocator.Current.GetInstance<IModuleInitializationService>();
            moduleInitializationService.Initialize();
        }

        protected override void InitializeShell()
        {
            //Check License
            if (!CheckLicense())
            {
                LicenseActivationWindow activteWindow = new LicenseActivationWindow();
                activteWindow.ShowDialog();

                if (!activteWindow.DialogResult.HasValue || !activteWindow.DialogResult.Value)
                    System.Windows.Application.Current.Shutdown();
            }
            if(File.Exists(LocalSettings.DocumentPath + "\\restore.txt"))
            {
                string[] line = System.IO.File.ReadAllLines(LocalSettings.DocumentPath + "\\restore.txt");
                if(line.Length == 2)
                {
                    if (File.Exists(line[0]))
                        File.Delete(line[0]);
                    File.Move(line[1], line[0]);
                    File.Delete(LocalSettings.DocumentPath + "\\restore.txt");
                }
            }
#if DEBUG
            // Bypass Singleton check
#else
            if (!Mutex.WaitOne(TimeSpan.Zero, true) && !LocalSettings.AllowMultipleClients)
            {
                NativeWin32.PostMessage((IntPtr)NativeWin32.HWND_BROADCAST, NativeWin32.WM_SHOWMagentixPOS, IntPtr.Zero, IntPtr.Zero);
                Environment.Exit(1);
            }
#endif
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            System.Net.ServicePointManager.Expect100Continue = false;

            LocalizeDictionary.ChangeLanguage(LocalSettings.CurrentLanguage);

            InteractionService.UserIntraction = ServiceLocator.Current.GetInstance<IUserInteraction>();
            InteractionService.UserIntraction.ToggleSplashScreen();

            ServiceLocator.Current.GetInstance<IApplicationState>().MainDispatcher = Application.Current.Dispatcher;
            var logger = ServiceLocator.Current.GetInstance<ILogService>();

            var messagingService = ServiceLocator.Current.GetInstance<IMessagingService>();
            messagingService.RegisterMessageListener(new MessageListener());

            if (LocalSettings.StartMessagingClient)
                messagingService.StartMessagingClient();

            PresentationServices.Initialize();

            base.InitializeShell();

            try
            {
                var creationService = new DataCreationService();
                creationService.CreateData();
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(LocalSettings.ConnectionString))
                {
                    var connectionString =
                        InteractionService.UserIntraction.GetStringFromUser(
                        "Connection String",
                        string.Format(Resources.ConnectionStringError, e.Message),
                        LocalSettings.ConnectionString);

                    var cs = String.Join(" ", connectionString);

                    if (!string.IsNullOrEmpty(cs))
                        LocalSettings.ConnectionString = cs.Trim();

                    logger.LogError(e, Resources.RestartAppError);
                }
                else
                {
                    logger.LogError(e);
                    LocalSettings.ConnectionString = "";
                }
                LocalSettings.SaveSettings();
                Environment.Exit(1);
            }

            var rm = Container.GetExportedValue<IRegionManager>();
            rm.RegisterViewWithRegion("MessageRegion", typeof(WorkPeriodStatusView));
            rm.RegisterViewWithRegion("MessageRegion", typeof(MessageClientStatusView));

            Application.Current.MainWindow = (Shell)Shell;

            if (LocalizeDictionary.Instance.Culture.TextInfo.IsRightToLeft)
            {
                Application.Current.MainWindow.FlowDirection = FlowDirection.RightToLeft;
            }

            ServiceLocator.Current.GetInstance<ITriggerService>().UpdateCronObjects();
            Thread.Sleep(3000);
            InteractionService.UserIntraction.ToggleSplashScreen();
            EntityCollectionSortManager.Load(LocalSettings.DocumentPath + "\\CollectionSort.txt");
            
            if (!string.IsNullOrEmpty(LocalSettings.CallerIdDeviceName))
            {
                ServiceLocator.Current.GetInstance<IDeviceService>().InitializeDevice(LocalSettings.CallerIdDeviceName);                
            }

            Application.Current.MainWindow.Show();
            ServiceLocator.Current.GetInstance<IDeviceService>().InitializeDevices();
            EventServiceFactory.EventService.PublishEvent(EventTopicNames.ShellInitialized);
            Mouse.UpdateCursor();
        }

        private bool CheckLicense()
        {
            //Initialize variables with default values
            MagentixLicense _lic = null;
        	byte[] _certPubicKeyData;
            string _msg = string.Empty;
            LicenseStatus _status = LicenseStatus.UNDEFINED;

            //Read public key from assembly
            Assembly _assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream _mem = new MemoryStream())
            {
                Stream stream = _assembly.GetManifestResourceStream("Magentix.Presentation.LicenseVerify.cer");
                stream.CopyTo(_mem);

                _certPubicKeyData = _mem.ToArray();
            }

            //Check if the XML license file exists
            if (File.Exists("license.lic"))
            {
                _lic = (MagentixLicense)LicenseHandler.ParseLicenseFromBASE64String(
                    typeof(MagentixLicense),
                    File.ReadAllText("license.lic"),
                    _certPubicKeyData,
                    out _status,
                    out _msg);
                if(_lic.TrialVersion == true && _lic.TrialDate < DateTime.Now)
                {
                    MessageBox.Show("Your trial version is expired. Please buy full version.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            else
            {
                _status = LicenseStatus.INVALID;
                _msg = "Your copy of this application is not activated";
            }

            switch (_status)
            {
                case LicenseStatus.VALID:
                    return true;

                default:
                    return false;
            }
        }
    }
}
