using ClientesWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientesWebService.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public CustomerController(HttpClient httpClient, IConfiguration configuration)
        {

            _httpClient = httpClient;
            _apiUrl = configuration.GetValue<string>("APIUrl");
        }

        // GET: ClienteController
        public ActionResult Customer()
        {

            return View(new Customer());
        }


        // POST: ClienteController/Create

        [HttpPost]
        public async Task<IActionResult> saveCustomer(Customer customer)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_apiUrl}CustomersService/", customer);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var custom = JsonConvert.DeserializeObject<Customer>(responseBody);

            return View("Customer", custom);
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





    }
}
