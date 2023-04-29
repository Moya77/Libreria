using ClientesWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientesWebService.Controllers
{
    public class CustomerListController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public CustomerListController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration.GetValue<string>("APIUrl");
        }

        // GET: ListCustomerController
        public async Task<ActionResult> CustomerList()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}CustomersService/");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<List<Customer>>(responseBody);

            return View(customers);
        }

        // GET: ListCustomerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ListCustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ListCustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ListCustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ListCustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ListCustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ListCustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
