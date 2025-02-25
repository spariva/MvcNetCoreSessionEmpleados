using Microsoft.AspNetCore.Mvc;
using MvcNetCoreSessionEmpleados.Extensions;
using MvcNetCoreSessionEmpleados.Models;
using MvcNetCoreSessionEmpleados.Repositories;

namespace MvcNetCoreSessionEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> SessionSalario(int? salario)
        {
            if (salario != null)
            {
                int sumaSalarial = 0;
                if(HttpContext.Session.GetString("SumaSalarial") != null)
                {
                    sumaSalarial = HttpContext.Session.getObject<int>("SumaSalarial");
                }
                sumaSalarial += salario.Value;
                HttpContext.Session.setObject("SumaSalarial", sumaSalarial);
                ViewBag.Mensaje = "Salario añadido" + sumaSalarial;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult SumaSalarial()
        {
            return View();
        }
    }
}
