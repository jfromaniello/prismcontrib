using System;

namespace WPFQuickstartWithCAL.EmployeeData.BusinessEntities
{
    public class Employee
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Position { set; get; }
        public int Age { set; get; }
        public DateTime EmployedOn { set; get; }
    }
}
