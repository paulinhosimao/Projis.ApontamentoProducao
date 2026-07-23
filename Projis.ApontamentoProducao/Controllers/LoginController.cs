using Microsoft.AspNetCore.Mvc;
using Projis.ApontamentoProducao.Models;
using Projis.ApontamentoProducao.Services;

namespace Projis.ApontamentoProducao.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public LoginController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                return RedirectToAction("Index", "ApontamentoProducao");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool loginOk = _usuarioService.ValidarUsuario(
                model.Usuario,
                model.Senha);

            if (!loginOk)
            {
                ModelState.AddModelError("", "Usuário ou senha inválidos.");
                return View(model);
            }

            HttpContext.Session.SetString("Usuario", model.Usuario);

            return RedirectToAction("Index", "ApontamentoProducao");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["Mensagem"] = "Logout realizado com sucesso.";

            return RedirectToAction("Index");
        }
    }
}