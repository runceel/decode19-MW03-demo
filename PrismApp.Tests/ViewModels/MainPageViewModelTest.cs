using Moq;
using Prism.Navigation;
using PrismApp.Entities;
using PrismApp.UseCases;
using PrismApp.ViewModels;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PrismApp.Tests.ViewModels
{
    public class MainPageViewModelTest
    {
        [Fact]
        public void EnableCase()
        {
            var empMock = new Mock<IEmployeeUseCase>();
            empMock.SetupSet(x => x.TargetId = 10).Verifiable();
            empMock.Setup(x => x.LookupAsync()).Returns(Task.CompletedTask).Verifiable();
            empMock.Setup(x => x.Employee).Returns(new Employee(10, "Test")).Verifiable();

            var naviMock = new Mock<INavigationService>();
            naviMock.Setup(x => x.NavigateAsync("NextPage", It.Is<NavigationParameters>(arg => arg.GetValue<int>("id") == 10)))
                .ReturnsAsync(new NavigationResult())
                .Verifiable();
            var vm = new MainPageViewModel(naviMock.Object, empMock.Object);
            Assert.False(vm.LookupCommand.CanExecute());
            vm.EmployeeId = "10";
            Assert.True(vm.LookupCommand.CanExecute());
            vm.LookupCommand.Execute();
            Assert.True(vm.NavigateToNextCommand.CanExecute());
            vm.NavigateToNextCommand.Execute();

            empMock.Verify();
            naviMock.Verify();
        }

        [Fact]
        public void DisableCase()
        {
            var empMock = new Mock<IEmployeeUseCase>();
            empMock.SetupSet(x => x.TargetId = 10).Verifiable();
            empMock.Setup(x => x.LookupAsync()).Returns(Task.CompletedTask).Verifiable();
            empMock.Setup(x => x.Employee).Returns<Employee>(null).Verifiable();

            var naviMock = new Mock<INavigationService>();
            var vm = new MainPageViewModel(naviMock.Object, empMock.Object);
            Assert.False(vm.LookupCommand.CanExecute());
            vm.EmployeeId = "10";
            Assert.True(vm.LookupCommand.CanExecute());
            vm.LookupCommand.Execute();
            Assert.False(vm.NavigateToNextCommand.CanExecute());

            empMock.Verify();
            naviMock.Verify();
        }
    }
}
