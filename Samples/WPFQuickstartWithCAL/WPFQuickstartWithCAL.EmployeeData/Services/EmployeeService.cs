using System;
using System.Collections.Generic;
using WPFQuickstartWithCAL.EmployeeData.BusinessEntities;

namespace WPFQuickstartWithCAL.EmployeeData.Services
{
    public class EmployeeService
    {
        private List<Employee> _employees = new List<Employee>();

        public EmployeeService()
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            _employees.Add(new Employee() { Id = 1, Name = "Abbas Syed", Position = "President", Age = 34, EmployedOn = DateTime.Now.AddYears(-13) });
            _employees.Add(new Employee() { Id = 2, Name = "Nadar Issa", Position = "CEO", Age = 56, EmployedOn = DateTime.Now.AddYears(-1) });
            _employees.Add(new Employee() { Id = 3, Name = "Manar Karim", Position = "Sales Manager", Age = 24, EmployedOn = DateTime.Now.AddYears(-2) });
            _employees.Add(new Employee() { Id = 4, Name = "Mohamed Shammi", Position = "HR Manager", Age = 67, EmployedOn = DateTime.Now.AddYears(-4) });
            _employees.Add(new Employee() { Id = 5, Name = "Glasson Stuart", Position = "CTO", Age = 32, EmployedOn = DateTime.Now.AddYears(-6) });
            _employees.Add(new Employee() { Id = 6, Name = "Breyer Markus", Position = "Development Manager", Age = 21, EmployedOn = DateTime.Now.AddYears(-3) });
        }

        public Employee GetEmployee(int id)
        {
            return _employees.Find(delegate(Employee match)
            {
                if (match.Id == id)
                    return true;
                return false;
            });
        }
    }
}
