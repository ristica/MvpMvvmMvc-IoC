﻿using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Demo.Dependencies.Contracts;
using Demo.Wpf.ViewModels.Base;
using Demo.Wpf.ViewModels.Commands;
using Demo.Wpf.ViewModels.Contracts;
using Demo.Wpf.Views.Contracts;
using Demo.Wpf.Views.Contracts.Base;

namespace Demo.Wpf.ViewModels
{
    public class HelpViewModel : BaseViewModel, IHelpViewModel
    {
        private readonly IHelpWindow _window;
        private string _windowTitle;
        private string _product;
        private string _version;
        private string _copyright;
        private string _description;

        #region PROPERTIES

        public string WindowTitle
        {
            get => this._windowTitle;
            set
            {
                this._windowTitle = value;
                OnPropertyChanged();
            }
        }

        public string Product
        {
            get => this._product;
            set
            {
                this._product = value;
                OnPropertyChanged();
            }
        }

        public string Version
        {
            get => this._version;
            set
            {
                this._version = value;
                OnPropertyChanged();
            }
        }

        public string Copyright
        {
            get => this._copyright;
            set
            {
                this._copyright = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => this._description;
            set
            {
                this._description = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public ICommand CloseCommand { get; set; }

        public HelpViewModel(IDependencyContainer dependencyContainer) : base(dependencyContainer)
        {
            this._window = dependencyContainer.Resolve<IHelpWindow>();

            this.RegisterCommands();
            this.SetDataContext();
        }

        public IBaseWindow GetWindow() => this._window;

        protected sealed override void SetDataContext()
        {
            this.WindowTitle = GetAssemblyTitle();
            this.Description = GetAssemblyDescription();
            this.Product = GetAssemblyProduct();
            this.Version = GetAssemblyVersion();
            this.Copyright = GetAssemblyCopyright();

            this._window.SetDataContext(this);
        }

        private void RegisterCommands()
        {
            this.CloseCommand = new DelegateCommand(ExecuteCloseCommand, CanExecuteCloseCommand);
        }

        private void ExecuteCloseCommand(object obj)
        {
            this._window.CloseMe();
        }

        private bool CanExecuteCloseCommand(object arg) => true;

        private string GetAssemblyCopyright()
        {
            dynamic attribute = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)
                .First();

            return attribute.Copyright;
        }

        private string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private string GetAssemblyProduct()
        {
            dynamic attribute = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyProductAttribute), false)
                .First();

            return attribute.Product;
        }

        private string GetAssemblyDescription()
        {
            dynamic attribute = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                .First();

            return attribute.Description;
        }

        private string GetAssemblyTitle()
        {
            dynamic attribute = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyTitleAttribute), false)
                .First();

            return attribute.Title;
        }
    }
}
