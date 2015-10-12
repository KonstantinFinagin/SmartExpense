namespace SmartExpense.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Caliburn.Micro;
    using Microsoft.Practices.Unity;
    using SmartExpense.UI.Helpers;
    using SmartExpense.UI.Messages;
    using SmartExpense.UILogic.ViewModels;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : CaliburnApplication
    {
        private Assembly viewAssembly;
        private UnityContainer container;

        private IEventAggregator eventAggregator;

        public App()
        {
            InitializeComponent();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            var assemblies = base.SelectAssemblies().ToList();
            this.viewAssembly = assemblies.First();

            assemblies.Add(typeof (ShellViewModel).GetTypeInfo().Assembly);
            return assemblies;
        }
        
        protected override void Configure()
        {
            ConfigureTypeMappings();

            this.container = new UnityContainer();

            RegisterServices();
            RegisterViewModels();

            this.eventAggregator = this.container.Resolve<IEventAggregator>();
        }

        private void RegisterServices()
        {
            this.container.RegisterContainerControlled<IEventAggregator, EventAggregator>();
        }

        private void ConfigureTypeMappings()
        {
            ViewLocator.LocateForModelType = (viewModelType, visualParent, context) =>
            {
                string viewTypeName = viewModelType.FullName
                    .Replace("UILogic.ViewModels", "UI.Views")
                    .Replace("Model",string.Empty);

                var viewType = this.viewAssembly.GetViewType(viewTypeName);
                var view = ViewLocator.GetOrCreateViewType(viewType);

                return view;
            };

        }

        private void RegisterViewModels()
        {
            this.container.RegisterPerResolve<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service != null)
            {
                return this.container.Resolve(service);
            }

            if (!string.IsNullOrWhiteSpace(key))
            {
                return this.container.Resolve(Type.GetType(key));
            }

            return null;
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.container.ResolveAll(service);
        }

        protected override void BuildUp(object instance)
        {
            this.container.BuildUp(instance);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootViewFor<ShellViewModel>();

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                this.eventAggregator.PublishOnUIThread(new ResumeStateMessage());   
            }
        }

        protected override void OnSuspending(object sender, SuspendingEventArgs e)
        {
            this.eventAggregator.PublishOnUIThread(new SuspendStateMessage(e.SuspendingOperation));
        }
    }
}
