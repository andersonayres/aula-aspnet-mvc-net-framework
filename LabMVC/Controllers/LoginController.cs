using LabMVC.Cripto;
using LabMVC.DTO;
using LabMVC.ModelViews;
using LabWebForms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabMVC.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logar(LoginDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Login) || string.IsNullOrEmpty(loginDTO.Senha))
                return View("index", new ErroModelView { Mensagem = "Login ou senha inválida" });

            // Buscar o cliente no banco de dados pelo nome de usuário
            Cliente cliente = Cliente.BuscarPorUsername(loginDTO.Login);

            if (cliente != null)
            {
                // Descriptografar a senha do cliente para comparação
                string senhaDescriptografada = Encript.Decrypt(cliente.Senha, "chave_de_criptografia");

                if (senhaDescriptografada == loginDTO.Senha)
                {
                    var cookie = new HttpCookie("usuario_logado");

                    
                    cookie.Value = cliente.Id.ToString();

                    
                    cookie.Expires = DateTime.Now.AddDays(1);

                    
                    cookie.HttpOnly = true;

                    
                    Response.Cookies.Add(cookie);

                    
                    return RedirectToAction("BemVindo", "Home");
                }
            }

            return View("index", new ErroModelView { Mensagem = "Login ou senha inválida" });
        }



        public ActionResult CadastrarCliente(ClienteDTO clienteDTO)
        {
            if (clienteDTO == null || string.IsNullOrEmpty(clienteDTO.Nome) || string.IsNullOrEmpty(clienteDTO.Email))
            {
                return View("index", new ErroModelView { Mensagem = "Nome ou email inválido" });
            }

            // Lógica para salvar o contato no banco de dados
            // Exemplo:
            Cliente contato = new Cliente
            {
                Nome = clienteDTO.Nome,
                Email = clienteDTO.Email,
                Login = clienteDTO.Login,
                Senha = clienteDTO.Senha,
            };

            // Chamada para salvar o contato no banco de dados usando o seu código de acesso ao banco de dados

            return RedirectToAction("Index", "Home");
        }
    }
}