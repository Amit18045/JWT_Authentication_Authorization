using JWT_Authentication_Authorization.Interface;
using JWT_Authentication_Authorization.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace JWT_Authentication_Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet]
        public List<Employee> GetEmployee()
        {
            var emp=_employeeService.GetEmployeeDetails();
            return emp;
        }
        [HttpPost]
        public Employee AddEmployee([FromBody] Employee employee)
        {
            var emp = _employeeService.AddEmployee(employee);
            return emp;
        }
    }
}
