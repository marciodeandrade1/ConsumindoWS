using ConsumindoWS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ConsumindoWS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public IActionResult Index(Models.Cep cep)
        {
            if (!ModelState.IsValid)
            { 
            return View(cep);
            }
            using (Correios.AtendeClienteClient correios = new Correios.AtendeClienteClient())
            {
                var consulta = correios.consultaCEPAsync(cep.Codigo.Replace("-", ""));

                if (consulta != null)
                {
                    ViewBag.Endereco = new Models.Endereco()
                    {
                        Descricao = consulta.end,
                        Complemento = consulta.complemento,
                        Bairro = consulta.bairro,
                        Cidade = consulta.cidade,
                        UF = consulta.uf
                    };
                }
            }
            return View(cep);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
