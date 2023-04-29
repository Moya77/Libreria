using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    public class ConsultaSalidasController : Controller
    {
        // GET: ConsultaSalidasController
        public ActionResult ConsultaSalidas()
        {
            return View();
        }

        // GET: ConsultaSalidasController/Details
        public ActionResult ReporteSalida(string fechaInicio, string fechaFinal)
        {
                string nombreReporte = "ReportLibro";
                return RedirectToAction("ReporteSalidas", "ReporteSalidas", new { reportName = nombreReporte,FechaInicio=fechaInicio,FechaFinal=fechaFinal });
        }


    }
}
