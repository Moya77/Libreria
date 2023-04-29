using Books.Models;
using ClientesWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientesWebService.Controllers
{
    public class IngresoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public IngresoController(HttpClient httpClient, IConfiguration configuration)
        {

            _httpClient = httpClient;
            _apiUrl = configuration.GetValue<string>("APIUrl");
        }
        public ActionResult Ingreso()
        {
            return View(new IncomingBook());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> IncommingBook(IncomingBook incombook)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_apiUrl}BookService/IncommingBook/", incombook);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var incomingResponse = JsonConvert.DeserializeObject<IncomingBook>(responseBody);

                return View("Ingreso", incomingResponse);
            }
            catch
            {
                return View();
            }
        }

        // GET: IngresoController/Edit/5
        public async Task<ActionResult> GetBook(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}BookService/GetBookById/{id}");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var book = JsonConvert.DeserializeObject<Book>(responseBody);

                return Ok(book);
            }
            catch
            {
                return View();
            }
        }



        // GET: IngresoController/Delete/5
        public async Task<ActionResult> GetCustomer(int id)
        {

            var response = await _httpClient.GetAsync($"{_apiUrl}CustomersService/{id}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(responseBody);
            return Ok(customer);
        }


    }
}
