using JWT_Authentication_Authorization.DataContext;
using JWT_Authentication_Authorization.Interface;
using JWT_Authentication_Authorization.Model;

namespace JWT_Authentication_Authorization.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly JwtContext _jwtContext;

        public EmployeeService(JwtContext jwtContext)
        {
            _jwtContext = jwtContext;
        }

        public Employee AddEmployee(Employee employee)
        {
           var emp=_jwtContext.Employees.Add(employee);
           _jwtContext.SaveChanges();
            return emp.Entity;
        }

        public List<Employee> GetEmployeeDetails()
        {
            var emp = _jwtContext.Employees.ToList();
            return emp;
        }
    }
}
