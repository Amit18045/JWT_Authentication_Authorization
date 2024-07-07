using JWT_Authentication_Authorization.Model;

namespace JWT_Authentication_Authorization.Interface
{
    public interface IEmployeeService
    {
        public List<Employee> GetEmployeeDetails();
        public Employee AddEmployee(Employee employee);
    }
}
