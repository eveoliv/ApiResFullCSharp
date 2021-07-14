using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.HttpClients;
using Microsoft.AspNetCore.Authorization;

namespace Alura.ListaLeitura.WebApp.Controllers
{
    [Authorize]
    public class LivroController : Controller
    {       
        private readonly LivroApiClient api;

        public LivroController(LivroApiClient api)
        {           
            this.api = api;
        }

        [HttpGet]
        public IActionResult Novo()
        {
            return View(new LivroUpload());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Novo(LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                await api.PostLivroAsync(model);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ImagemCapa(int id)
        {           
            byte[] img = await api.GetCapaLivroAsync(id);
            
            if (img != null)
            {
                return File(img, "image/png");
            }
            return File("~/images/capas/capa-vazia.png", "image/png");
        }

        [HttpGet]
        public async Task<IActionResult> Detalhes(int id)
        {
            var model = await api.GetLivroAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model.ToUpload());
        }      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detalhes(LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                await api.PutLivroAsync(model);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remover(int id)
        {
            var model = await api.GetLivroAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            await api.DeleteLivroAsync(id);

            return RedirectToAction("Index", "Home");
        }
    }
}