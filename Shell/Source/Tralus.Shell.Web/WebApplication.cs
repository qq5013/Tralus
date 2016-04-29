using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.Web;
using System.Collections.Generic;
using DevExpress.ExpressApp.EF;
using System.Data.Common;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl.EF;
using Tralus.Framework.BusinessModel.Entities.StateMachines;
using Tralus.Framework.Data;
using Tralus.Shell.Module.Utility;

namespace Tralus.Shell.Web
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/DevExpressExpressAppWebWebApplicationMembersTopicAll
    public partial class ShellAspNetApplication : WebApplication
    {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule module2;
        private Tralus.Shell.Module.ShellModule module3;
        private Tralus.Shell.Module.Web.ShellAspNetModule module4;
        private DevExpress.ExpressApp.Validation.ValidationModule validationModule1;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule;
        private DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule conditionalAppearanceModule1;
        private DevExpress.ExpressApp.ReportsV2.ReportsModuleV2 reportsModuleV21;
        private DevExpress.ExpressApp.ReportsV2.Web.ReportsAspNetModuleV2 reportsAspNetModuleV21;
        private DevExpress.ExpressApp.StateMachine.StateMachineModule stateMachineModule;

        public ShellAspNetApplication()
        {
            InitializeComponent();
            stateMachineModule.StateMachineStorageType = typeof(StateMachine);
            //try
            //{
            //    AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            //    {
            //        var location = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath,
            //            args.Name.Split(',')[0] + ".dll");
            //        if (File.Exists(location))
            //        {
            //            var assembly = Assembly.LoadFrom(location);
            //            return assembly;
            //        }
            //        return null;
            //    };

            //    ReflectionHelper.GetImportedModules(out _loadedModuleTypes, out _loadedContextTypes);
            foreach (var loadedModuleType in Global.LoadedModuleTypes)
            {
                var loadedModule = (ModuleBase)Activator.CreateInstance(loadedModuleType);
                Modules.Insert(0, loadedModule);
            }
            //}
            //catch
            //{
            //    Trace.WriteLine(string.Format("Unable to load modules."));
            //}

        }

        //private IEnumerable<Type> _loadedModuleTypes;
        //private IEnumerable<Type> _loadedContextTypes;

        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        {
            foreach (var dbContext in Global.LoadedContextTypes)
            {
                args.ObjectSpaceProviders.Add(
                    args.Connection != null ?
                        new EFObjectSpaceProvider(dbContext, TypesInfo, null, (DbConnection)args.Connection) :
                        new EFObjectSpaceProvider(dbContext, TypesInfo, null, args.ConnectionString));
            }
        }

        private void ShellAspNetApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e)
        {
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
            if (System.Diagnostics.Debugger.IsAttached)
            {
                e.Updater.Update();
                e.Handled = true;
            }
            else
            {
                string message = "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
                    "This error occurred  because the automatic database update was disabled when the application was started without debugging.\r\n" +
                    "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " +
                    "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
                    "or manually create a database using the 'DBUpdater' tool.\r\n" +
                    "Anyway, refer to the following help topics for more detailed information:\r\n" +
                    "'Update Application and Database Versions' at http://help.devexpress.com/#Xaf/CustomDocument2795\r\n" +
                    "'Database Security References' at http://help.devexpress.com/#Xaf/CustomDocument3237\r\n" +
                    "If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/";

                if (e.CompatibilityError != null && e.CompatibilityError.Exception != null)
                {
                    message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
                }
                throw new InvalidOperationException(message);
            }
#endif
        }
        private void InitializeComponent()
        {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
            this.module3 = new Tralus.Shell.Module.ShellModule();
            this.module4 = new Tralus.Shell.Module.Web.ShellAspNetModule();
            this.securityModule = new DevExpress.ExpressApp.Security.SecurityModule();
            this.stateMachineModule = new DevExpress.ExpressApp.StateMachine.StateMachineModule();
            this.validationModule1 = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.conditionalAppearanceModule1 = new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule();
            this.reportsModuleV21 = new DevExpress.ExpressApp.ReportsV2.ReportsModuleV2();
            this.reportsAspNetModuleV21 = new DevExpress.ExpressApp.ReportsV2.Web.ReportsAspNetModuleV2();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // stateMachineModule
            // 
            this.stateMachineModule.StateMachineStorageType = typeof(DevExpress.ExpressApp.StateMachine.Xpo.XpoStateMachine);
            // 
            // validationModule1
            // 
            this.validationModule1.AllowValidationDetailsAccess = true;
            this.validationModule1.IgnoreWarningAndInformationRules = false;
            // 
            // reportsModuleV21
            // 
            this.reportsModuleV21.EnableInplaceReports = true;
            this.reportsModuleV21.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.EF.ReportDataV2);
            this.reportsModuleV21.ReportStoreMode = DevExpress.ExpressApp.ReportsV2.ReportStoreModes.XML;
            this.reportsModuleV21.ShowAdditionalNavigation = true;
            // 
            // reportsAspNetModuleV21
            // 
            //this.reportsAspNetModuleV21.ClientLibrariesLocation = DevExpress.ExpressApp.ReportsV2.Web.ClientLibrariesLocations.Embedded;
            this.reportsAspNetModuleV21.DesignAndPreviewDisplayMode = DevExpress.ExpressApp.ReportsV2.Web.DesignAndPreviewDisplayModes.DetailView;
            // 
            // ShellAspNetApplication
            // 
            this.ApplicationName = "Tralus.Shell";
            this.CollectionsEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.reportsModuleV21);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.Modules.Add(this.securityModule);
            this.Modules.Add(this.validationModule1);
            this.Modules.Add(this.conditionalAppearanceModule1);
            this.Modules.Add(this.stateMachineModule);
            this.Modules.Add(this.reportsAspNetModuleV21);
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.ShellAspNetApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}
