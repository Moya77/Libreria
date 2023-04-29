using ClientesWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientesWebService.Controllers
{
    public class BookController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public BookController(HttpClient httpClient, IConfiguration configuration)
        {

            _httpClient = httpClient;
            _apiUrl = configuration.GetValue<string>("APIUrl");
        }
        // GET: ProductController
        public ActionResult Book()
        {
            return View(new Book());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveBook(Book book)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{_apiUrl}BookService/", book);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var lib = JsonConvert.DeserializeObject<Book>(responseBody);

                return View("Book", lib);
            }
            catch
            {
                return View(new Book());
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult GetBook(string name)
        {
            return View();
        }






    }
}
