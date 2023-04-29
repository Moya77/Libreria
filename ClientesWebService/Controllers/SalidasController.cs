using ClientesWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Books.Controllers
{
    public class SalidasController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public SalidasController(HttpClient httpClient, IConfiguration configuration)
        {

            _httpClient = httpClient;
            _apiUrl = configuration.GetValue<string>("APIUrl");
        }
        // GET: SalidasController
        public ActionResult Salidas()
        {
            return View(new OutBook());
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomer(string id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}CustomersService/{id}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(responseBody);
            return Ok(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RetiroDeLibros(OutBook outbook)
        {
            try
            {
                HttpResponseMessage response;
                if (outbook.UserBooks.Count > 0)
                {
                    response = await _httpClient.PostAsJsonAsync($"{_apiUrl}BookService/RetirarLibros/", outbook);
                    outbook.Procesed = true;
                }
                else
                {
                    outbook.FechaSalida = "";
                    response = await _httpClient.PostAsJsonAsync($"{_apiUrl}BookService/GetUserBooks/", outbook);

                }
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var Outbook = JsonConvert.DeserializeObject<OutBook>(responseBody);
                if (Outbook != null)
                {
                    Outbook.Procesed = outbook.Procesed;
                }

                return View("Salidas", Outbook);
            }
            catch
            {
                return View();
            }
        }

        // GET: SalidasController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

    }
}
