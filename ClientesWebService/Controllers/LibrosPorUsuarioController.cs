using Books.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Books.Controllers
{
    public class LibrosPorUsuarioController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public LibrosPorUsuarioController(HttpClient httpClient, IConfiguration configuration)
        {

            _httpClient = httpClient;
            _apiUrl = configuration.GetValue<string>("APIUrl");
        }
        // GET: LibrosPorUsuario
        public async Task<ActionResult> librosUsuarios()
        {

            var response = await _httpClient.GetAsync($"{_apiUrl}BookService/GetListBooksUsers");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var listaLibros = JsonConvert.DeserializeObject<Dictionary<string, ListaLibrosUsuarios>>(responseBody);


            return View(listaLibros);
        }

        // GET: LibrosPorUsuario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LibrosPorUsuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibrosPorUsuario/Create
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

        // GET: LibrosPorUsuario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LibrosPorUsuario/Edit/5
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

        // GET: LibrosPorUsuario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LibrosPorUsuario/Delete/5
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
