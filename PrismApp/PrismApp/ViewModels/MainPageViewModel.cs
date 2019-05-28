using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using PrismApp.Entities;
using PrismApp.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrismApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public IEmployeeUseCase EmployeeUseCase { get; }

        // for designer
        public MainPageViewModel() : base(null)
        {
        }

        public MainPageViewModel(INavigationService navigationService, IEmployeeUseCase employeeUseCase)
            : base(navigationService)
        {
            Title = "Main Page";
            EmployeeUseCase = employeeUseCase ?? throw new ArgumentNullException(nameof(employeeUseCase));
        }

        private DelegateCommand _lookupCommand;
        public DelegateCommand LookupCommand =>
            _lookupCommand ?? (_lookupCommand = new DelegateCommand(ExecuteLookupCommand, CanExecuteLookupCommand)
                .ObservesProperty(() => EmployeeId));

        private DelegateCommand _navigateToNextPageCommand;
        public DelegateCommand NavigateToNextCommand =>
            _navigateToNextPageCommand ?? (_navigateToNextPageCommand = new DelegateCommand(ExecuteNavigateToNextCommand, CanExecuteNavigateToNextCommand)
                .ObservesProperty(() => EmployeeUseCase.Employee));

        private async void ExecuteNavigateToNextCommand()
        {
            await NavigationService.NavigateAsync("NextPage", new NavigationParameters
            {
                { "id", EmployeeUseCase.Employee.Id },
            });
        }

        private bool CanExecuteNavigateToNextCommand() => EmployeeUseCase.Employee != null;

        private async void ExecuteLookupCommand()
        {
            EmployeeUseCase.TargetId = int.Parse(EmployeeId);
            await EmployeeUseCase.LookupAsync();
        }

        private bool CanExecuteLookupCommand() => ValidateInputs();

        private string _employeeId;
        public string EmployeeId
        {
            get => _employeeId;
            set => _ = SetProperty(ref _employeeId, value);
        }

        private bool ValidateInputs() => int.TryParse(EmployeeId, out var _);
    }
}
