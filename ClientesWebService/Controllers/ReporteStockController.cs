using System.IO;
using System.Reflection;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    public class ReportStockController : Controller
    {
        public ActionResult ReporteStock(string reportName, string format = "PDF")
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Books.Resources.{reportName}.rdl";

            foreach (var resource in assembly.GetManifestResourceNames())
            {
                Console.WriteLine(resource);
            }


            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                return NotFound("Informe no encontrado");
            }

          
            var tempFilePath = Path.GetTempFileName();
            using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }

       
            var localReport = new LocalReport(tempFilePath);



            var result = localReport.Execute(RenderType.Pdf);
            var base64string = Convert.ToBase64String(result.MainStream.ToArray());
            ViewBag.ReportBase64 = base64string;

            return View();
        }
    }
}
