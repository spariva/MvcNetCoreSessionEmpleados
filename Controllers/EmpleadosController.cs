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

        public async Task<IActionResult> CestaBorrar(int id)
        {
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("Ids");
            idsEmpleados.Remove(id);
            HttpContext.Session.SetObject("Ids", idsEmpleados);
            return RedirectToAction("CestaOk");
        }

        public async Task<IActionResult> CestaOK()
        {
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("Ids");
            if (idsEmpleados == null)
            {
                ViewBag.Mensaje = "No hay empleados en la cesta";
                return View();
            }
            //recupero los empleados con los ids almacenados en la sesión, si no devuelve nada, nos trae null
            // y en la vista con un if no pintamos nada.
            List<Empleado> empleados = await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
            return View(empleados);
        }

        ////V6
        //public async Task<IActionResult> SessionV6(int? idEmpleado)
        //{
        //    if (idEmpleado != null)
        //    {
        //        List<int> idsEmpleados;
        //        if (HttpContext.Session.GetObject<List<int>>("Ids") == null)
        //        {
        //            idsEmpleados = new List<int>();
        //        }
        //        else
        //        {
        //            idsEmpleados = HttpContext.Session.GetObject<List<int>>("Ids");
        //        }
        //        idsEmpleados.Add(idEmpleado.Value);
        //        HttpContext.Session.SetObject("Ids", idsEmpleados);
        //    }

        //    List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
        //    return View(empleados);
        //}

        public async Task<IActionResult> SessionV6(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                //ALMACENAREMOS LO MINIMO QUE PODAMOS (int) 
                List<int> idsEmpleados;
                if (HttpContext.Session.GetObject<List<int>>("Ids") == null)
                {
                    //NO EXISTE Y CREAMOS LA COLECCION 
                    idsEmpleados = new List<int>();
                }
                else
                {
                    //EXISTE Y RECUPERAMOS LA COLECCION 
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("Ids");
                }
                idsEmpleados.Add(idEmpleado.Value);
                //REFRESCAMOS LOS DATOS DE SESSION 
                HttpContext.Session.SetObject("Ids", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: "
                + idsEmpleados.Count;
            }
            //COMPROBAMOS SI TENEMOS IDS EN SESSION 
            List<int> ids = HttpContext.Session.GetObject<List<int>>("Ids");
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        //V5: Muestra check o no según esté en session
        public async Task<IActionResult> SessionV5(int? idEmpleado)
        {
            List<int> ids;
            if (HttpContext.Session.GetObject<List<int>>("Ids") == null)
            {
                ids = new List<int>();
            }
            else
            {
                ids = HttpContext.Session.GetObject<List<int>>("Ids");
            }

            if (idEmpleado != null)
            {
                ids.Add(idEmpleado.Value);
                HttpContext.Session.SetObject("Ids", ids);
            }

            List<Empleado> empleadosCesta = await this.repo.GetEmpleadosSessionAsync(ids);
            List<Empleado> empleados = await this.repo.GetEmpleadosNotInIdsAsync(ids);

            ViewBag.EmpleadosCesta = empleadosCesta;
            return View(empleados);
        }

        //V4: solo muestra los empleados que no has seleccionado, por lo que no puedes seleccionar dos veces al mismo (en teoría)
        public async Task<IActionResult> SessionV4(int? idEmpleado)
        {
            List<int> ids;
            if (HttpContext.Session.GetObject<List<int>>("Ids") == null)
            {
                ids = new List<int>();
            }
            else
            {
                ids = HttpContext.Session.GetObject<List<int>>("Ids");
            }

            if (idEmpleado != null)
            {
                ids.Add(idEmpleado.Value);
                HttpContext.Session.SetObject("Ids", ids);
            }

            List<Empleado> empleados = await this.repo.GetEmpleadosNotInIdsAsync(ids);
            return View(empleados);
        }

        public async Task<IActionResult> SessionEmpleadosOK(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                List<int> idsEmpleados;
                //almaceno lo mínimo int
                // Si no está en session lo instancio vacío, sino lo recupero y almaceno en la lista
                if (HttpContext.Session.GetObject<List<int>>("IdsEmpleados") == null)
                {
                    idsEmpleados = new List<int>();
                }
                else
                {
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IdsEmpleados");
                }
                //añado a la lista el id del empleado (sería el id del producto) y lo seteas para que se guarde en la sesión
                idsEmpleados.Add(idEmpleado.Value);
                HttpContext.Session.SetObject("IdsEmpleados", idsEmpleados);
            }
            //esto es para mostrar los empleados en la vista. Sería mostrar los productos?
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);

        }



        public async Task<IActionResult> SessionEmpleados(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                Empleado empleado = await this.repo.FindEmpleadoAsync(idEmpleado.Value);
                List<Empleado> empleadosList;
                if (HttpContext.Session.GetObject<List<Empleado>>("Empleados") != null)
                {
                    empleadosList = HttpContext.Session.GetObject<List<Empleado>>("Empleados");
                }
                else
                {
                    empleadosList = new List<Empleado>();
                }

                empleadosList.Add(empleado);
                HttpContext.Session.SetObject("Empleados", empleadosList);
                ViewBag.Mensaje = "Empleado añadido: " + empleado.Apellido;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult CestaEmpleados()
        {
            return View();
        }

        public async Task<IActionResult> SessionSalario(int? salario)
        {
            if (salario != null)
            {
                int sumaSalarial = 0;
                if(HttpContext.Session.GetString("SumaSalarial") != null)
                {
                    sumaSalarial = HttpContext.Session.GetObject<int>("SumaSalarial");
                }
                sumaSalarial += salario.Value;
                HttpContext.Session.SetObject("SumaSalarial", sumaSalarial);
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
