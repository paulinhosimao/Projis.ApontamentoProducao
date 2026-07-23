using Microsoft.AspNetCore.Mvc;
using Projis.ApontamentoProducao.Models;
using Projis.ApontamentoProducao.Services;

namespace Projis.ApontamentoProducao.Controllers
{
    public class ApontamentoProducaoController : Controller
    {
        private readonly ApontamentoService _service;

        public ApontamentoProducaoController(ApontamentoService service)
        {
            _service = service;
        }

        public IActionResult Index(int filial = 17, string codOs = "000010")
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario")))
            {
                return RedirectToAction("Index", "Login");
            }

            

            var model = _service.BuscarOp(filial, codOs);

            if (model == null)
            {
                model = new ApontamentoProducaoViewModel
                {
                    Filial = filial,
                    CodigoOs = codOs,
                    DescricaoProduto = "- OP não encontrada",
                    StatusOp = "NF"
                };
            }

            return View(model);
        }
    }
}