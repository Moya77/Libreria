using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNetCore.Reporting;

public class ReporteSalidasController : Controller
{
    public ActionResult ReporteSalidas(string reportName, string FechaInicio, string FechaFinal, string format = "PDF")
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"Books.Resources.{reportName}.rdl";

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
