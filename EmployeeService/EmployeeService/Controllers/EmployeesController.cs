using EmployeeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EmployeeService.Controllers
{
    
    public class EmployeesController : ApiController
    {
       
        public HttpResponseMessage GetEmployees()
        {
            using (EmployeeDBContext employeedbContext = new EmployeeDBContext())
            {
                var employees=employeedbContext.Employees.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, employees);
            }
        }

        //public IEnumerable<string> Get()
        //{
        //    IList<string> formatters = new List<string>();
        //    formatters.Add(GlobalConfiguration.Configuration.Formatters.JsonFormatter.GetType().FullName);
        //    formatters.Add(GlobalConfiguration.Configuration.Formatters.XmlFormatter.GetType().FullName);
        //    formatters.Add(GlobalConfiguration.Configuration.Formatters.FormUrlEncodedFormatter.GetType().FullName);
        //    return formatters.AsEnumerable<string>();
        //}
        public HttpResponseMessage Get(int id)
        {
            using (EmployeeDBContext dbContext = new EmployeeDBContext())
            {
                var entity = dbContext.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Employee with Id" + id.ToString() + "Not found");
                }
                
            }
        }

        //Custom Method
        //below this method return only the male employee

        [HttpGet]
        public HttpResponseMessage EmployeeList(string gender="All")
        {
            using (EmployeeDBContext dbContext = new EmployeeDBContext())
            {
               
                switch (gender.ToLower())
                {
                    case "all": return Request.CreateResponse(HttpStatusCode.OK,dbContext.Employees.ToList());
                    case "male": return Request.CreateResponse(HttpStatusCode.OK,dbContext.Employees.Where(e=>e.Gender.ToLower()=="male").ToList());
                    case "female": return Request.CreateResponse(HttpStatusCode.OK,dbContext.Employees.Where(e=>e.Gender.ToLower()=="female").ToList());
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            "Value for gender must be Male, Female or All. " + gender + " is invalid.");
                }
               

            }
        }

        //Post Method

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
           try
           {
              using (EmployeeDBContext dbContext = new EmployeeDBContext())
              {
                        dbContext.Employees.Add(employee);
                        dbContext.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                        message.Headers.Location = new Uri(Request.RequestUri +
                            employee.ID.ToString());

                        return message;
              }
           }
           catch (Exception ex)
           {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
           }
        }
        //Put Method
        public HttpResponseMessage Put([FromBody]int id, [FromUri] Employee employee)
        {
            try
            {
                using (EmployeeDBContext dbContext = new EmployeeDBContext())
                {
                    var entity = dbContext.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;

                        dbContext.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //Delete Method
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeDBContext dbContext = new EmployeeDBContext())
                {
                    var entity = dbContext.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity != null)
                    {
                        dbContext.Employees.Remove(dbContext.Employees.FirstOrDefault(e => e.ID == id));
                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                           "Employee with Id " + id.ToString() + " not found to Delete");
                    }
                 
                }
            }catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
           
        }

    }
}