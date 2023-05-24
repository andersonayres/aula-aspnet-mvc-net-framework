using LabMVC.Filtros;
using LabWebForms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabMVC.Controllers
{
    public class HomeController : LogadoController
    {
        [AutenticadoFilter]
        public ActionResult Index()
        {
            // if (!Logado()) return null;

            // return new HttpUnauthorizedResult();

            ViewBag.clientes = Cliente.Todos();
            //ViewData["clientes ssds"] = Cliente.Todos();
            return View(new
            {
                Clientes = Cliente.Todos(),
                Mensagem = "oi"
            });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Listar()
        {
            List<Cliente> clientes = Cliente.Todos(); // Obtém os clientes do banco de dados usando o método Todos() da classe Cliente

            return View(clientes);
        }
        public ActionResult Editar(int id)
        {
            // Lógica para obter os dados do cliente com o ID fornecido
            Cliente cliente = Cliente.BuscarPorId(id);

            // Renderizar a view de edição com os dados do cliente
            return View(cliente);
        }
        
        
        public ActionResult BemVindo()
        {
            return View("BemVindo");
        }
        

    }
}