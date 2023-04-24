﻿using FullStack.API.Data;
using FullStack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly FullStackDBContext _fullStackDBContext;

        public EmployeesController(FullStackDBContext fullStackDBContext)
        {
          _fullStackDBContext = fullStackDBContext;
        }

        [HttpGet]
        public async Task <IActionResult> GetAllEmployees()
        {
            var employees = await _fullStackDBContext.Employees.ToListAsync();

            return Ok(employees);
        }

        [HttpPost]

        public async Task<IActionResult> AddEmployee([FromBody]Employee employeeRequest)
        {
            employeeRequest.Id = Guid.NewGuid();

            await _fullStackDBContext.Employees.AddAsync(employeeRequest);
            await _fullStackDBContext.SaveChangesAsync(); 
            
            return Ok(employeeRequest);
        }

        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetEmployee([FromRoute]Guid id)
        {
            var employee = await _fullStackDBContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if(employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, Employee updateEmployeeRequest)
        {
            var employee = await _fullStackDBContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            employee.Name = updateEmployeeRequest.Name;
            employee.Email = updateEmployeeRequest.Email;   
            employee.Salary = updateEmployeeRequest.Salary;
            employee.Phone = updateEmployeeRequest.Phone;
            employee.Department = updateEmployeeRequest.Department;

            await _fullStackDBContext.SaveChangesAsync();

            return Ok(employee);
        }
    }
}
