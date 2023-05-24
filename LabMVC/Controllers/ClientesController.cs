using LabMVC.Cripto;
using LabWebForms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabMVC.Controllers
{
    public class ClientesController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Estado = new SelectList(Estado.Todos(), "Id", "Nome");
            ViewBag.Cidades = new SelectList(Cidade.Todos(new Estado() { Id = 1 }), "Id", "Nome");
            return View();
        }


        public ActionResult HtmlPuro()
        {
            ViewBag.Estado = Estado.Todos();
            ViewBag.Cidades = Cidade.Todos(new Estado() { Id = 1 });
            return View(new Cliente());
        }
        public ActionResult ObterCidades(int estadoId)
        {
            Estado estado = new Estado { Id = estadoId };
            List<Cidade> cidades = Cidade.Todos(estado);
            return Json(cidades, JsonRequestBehavior.AllowGet);
        }


        
        [HttpPost]
        public ActionResult Salvar(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                // Criptografar a senha antes de salvar
                string senhaCriptografada = Encript.Encrypt(cliente.Senha, "chave_de_criptografia");

                // Atribuir a senha criptografada ao objeto cliente
                cliente.Senha = senhaCriptografada;
                cliente.Salvar(); // Chamar o método Salvar() para salvar o cliente no banco de dados
                ViewBag.MensagemSucesso = "Cadastro realizado com sucesso!";

                return RedirectToAction("Listar", "Home");
                // Redirecionar para a página de índice após o salvamento
            }

            // Se houver erros de validação, retorne a view com os erros
            ViewBag.Estado = Estado.Todos();
            ViewBag.Cidades = new SelectList(Cidade.Todos(new Estado() { Id = 1 }), "Id", "Nome");
            return View("Index", cliente);
        }

        public ActionResult Editar(int id, int estadoId)
        {
            // Lógica para obter os dados do cliente com o ID fornecido
            Cliente cliente = Cliente.BuscarPorId(id);

            // Carregar as cidades correspondentes ao estado selecionado
            Estado estado = new Estado { Id = estadoId };
            List<Cidade> cidades = Cidade.Todos(estado);
            ViewBag.Cidades = new SelectList(cidades, "Id", "Nome");

            // Renderizar a view de edição com os dados do cliente
            return View(cliente);
        }
                
        [HttpPost]
        public ActionResult Atualizar(Cliente cliente, int estadoId)
        {
            if (ModelState.IsValid)
            {
                // Chamar o método de atualização na instância do cliente
                cliente.Atualizar(cliente.Id);

                // Redirecionar para a página desejada após a atualização
                return RedirectToAction("Listar", "Home");
            }

            // Carregar as cidades correspondentes ao estado selecionado
            Estado estado = new Estado { Id = estadoId };
            List<Cidade> cidades = Cidade.Todos(estado);
            ViewBag.Cidades = new SelectList(cidades, "Id", "Nome");

            // Retorna a view com o objeto cliente para exibir os erros de validação
            return View(cliente);
        }




    }
}