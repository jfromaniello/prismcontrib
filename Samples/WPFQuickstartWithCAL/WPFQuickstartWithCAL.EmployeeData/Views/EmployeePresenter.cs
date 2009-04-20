using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Wpf.Events;
using WPFQuickstartWithCAL.Infrastructure.Events;
using WPFQuickstartWithCAL.EmployeeData.Services;
using WPFQuickstartWithCAL.EmployeeData.BusinessEntities;

namespace WPFQuickstartWithCAL.EmployeeData.Views
{
    public class EmployeePresenter
    {
        private EmployeeService _employeeService;

        public EmployeePresenter(IEmployeeView view, EmployeeService employeeService, IEventAggregator eventAggregator)
        {
            this.View = view;
            this._employeeService = employeeService;

            eventAggregator.GetEvent<ShowEmployeeEvent>().Subscribe(SetSelectedEmployee, ThreadOption.UIThread, true);
        }

        public void SetSelectedEmployee(int index)
        {
            Employee employee = _employeeService.GetEmployee(index);
            View.Model = employee;
        }

        public IEmployeeView View { get; set; }
    }
}
