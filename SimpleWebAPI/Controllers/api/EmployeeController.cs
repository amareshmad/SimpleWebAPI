using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SimpleWebAPI.Models;


namespace SimpleWebAPI.Controllers.api
{
    public class EmployeeController : ApiController
    {
        
            public EmployeeController()
            {
            }

            //Get 
            public IHttpActionResult GetAllEmployees()
            {
                IList<EmployeeViewModel> employees = null;

                using (var ctx = new EmployeeEntities1())
                {
                    employees = ctx.Employees
                                .Select(s => new EmployeeViewModel()
                                {
                                    Id = s.Id,
                                    FirstName = s.Firstname,
                                    LastName = s.Lastname,
                                    Department = s.department
                                }).ToList<EmployeeViewModel>();
                }

                if (employees.Count == 0)
                {
                    return NotFound();
                }

                return Ok(employees);
            }


            //Post insert
            //Get action methods of the previous section
            public IHttpActionResult PostNewEmployee(EmployeeViewModel employee)
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid data.");

                using (var ctx = new EmployeeEntities1())
                {
                    ctx.Employees.Add(new Employee()
                    {
                        Id = (long)employee.Id,
                        Firstname = employee.FirstName,
                        Lastname = employee.LastName,
                        department = employee.Department
                    });

                    ctx.SaveChanges();
                }

                return Ok();
            }

            //put update
            public IHttpActionResult Put(EmployeeViewModel employee)
            {
                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");

                using (var ctx = new EmployeeEntities1())
                {
                    var existingEmployee = ctx.Employees.Where(s => s.Id == employee.Id)
                                                            .FirstOrDefault<Employee>();

                    if (existingEmployee != null)
                    {
                        existingEmployee.Firstname = employee.FirstName;
                        existingEmployee.Lastname = employee.LastName;
                        existingEmployee.department = employee.Department;

                        ctx.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }

                return Ok();
            }


            //Delete
            public IHttpActionResult Delete(int id)
            {
                if (id <= 0)
                    return BadRequest("Not a valid employee id");

                using (var ctx = new EmployeeEntities1())
                {
                    var employee = ctx.Employees
                        .Where(s => s.Id == id)
                        .FirstOrDefault();

                    ctx.Entry(employee).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }

                return Ok();
            }
        }
    
}
