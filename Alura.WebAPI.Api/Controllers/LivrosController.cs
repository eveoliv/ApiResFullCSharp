using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;

namespace Alura.ListaLeitura.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly IRepository<Livro> repo;

        public LivrosController(IRepository<Livro> repo)
        {
            this.repo = repo;
        }

        [HttpGet("{id}")]
        public IActionResult Recuperar(int id)
        {
            var model = repo.Find(id);

            if (model == null)
                return NotFound();

            return Ok(model.ToApi());
        }

        [HttpGet]
        public IActionResult ListaDeLivros()
        {
            var lista = repo.All                
                .Select(l => l.ToApi())
                .ToList();

            return Ok(lista);
        }

        [HttpGet("{id}/capa")]
        public IActionResult ImagemCapa(int id)
        {
            byte[] img = repo.All
               .Where(l => l.Id == id)
               .Select(l => l.ImagemCapa)
               .FirstOrDefault();
            if (img != null)
            {
                return File(img, "image/png");
            }
            return File("~/images/capas/capa-vazia.png", "image/png");
        }

        [HttpPost]
        public IActionResult Incluir([FromForm]LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                repo.Incluir(livro);
                var url = Url.Action("Recuperar", new { id = livro.Id });
                return Created(url, livro);
            }
            return BadRequest();
        }

        [HttpPut]       
        public IActionResult Alterar([FromForm]LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                if (model.Capa == null)
                {
                    livro.ImagemCapa = repo.All
                        .Where(l => l.Id == livro.Id)
                        .Select(l => l.ImagemCapa)
                        .FirstOrDefault();
                }
                repo.Alterar(livro);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Remover(int id)
        {
            var model = repo.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            repo.Excluir(model);
            return NoContent();
        }
    }
}
