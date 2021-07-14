using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Alura.ListaLeitura.Modelos;
using System.Collections.Generic;
using Alura.ListaLeitura.HttpClients;
using Alura.ListaLeitura.WebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace Alura.ListaLeitura.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly LivroApiClient api;        

        public HomeController(LivroApiClient api)
        {           
            this.api = api;
        }

        private async Task<IEnumerable<LivroApi>> ListaDoTipo(TipoListaLeitura tipo)
        {
            var lista = await api.GetListaDeLeituraAsync(tipo);
            return lista.Livros;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.User.Claims.First(c => c.Type == "Token").Value;            

            var model = new HomeViewModel
            {
                ParaLer = await ListaDoTipo(TipoListaLeitura.ParaLer),
                Lendo = await ListaDoTipo(TipoListaLeitura.Lendo),
                Lidos = await ListaDoTipo(TipoListaLeitura.Lidos)
            };
            return View(model);
        }
    }
}