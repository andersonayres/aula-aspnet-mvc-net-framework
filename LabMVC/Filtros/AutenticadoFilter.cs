using LabMVC.DTO;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace LabMVC.Filtros
{
    public class AutenticadoFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        private const string name = "usuario_logado";

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies[name];
            if (cookie != null)
            {
                try
                {
                    string value = LabMVC.Cripto.Encript.Decrypt(cookie.Value, "12188282sjjabqghhnnwqwqw");
                    LoginDTO loginDTO = JsonConvert.DeserializeObject<LoginDTO>(value);

                    // Realize a validação do usuário com base nas informações do objeto LoginDTO
                    // Por exemplo, verifique se as credenciais do usuário correspondem a um registro válido no banco de dados
                    if (ValidarCredenciais(loginDTO.Login, loginDTO.Senha))
                    {
                        // O usuário está autenticado com sucesso, você pode armazenar informações adicionais na sessão, se necessário
                        // filterContext.HttpContext.Session["usuario_logado"] = loginDTO;
                        return;
                    }
                }
                catch
                {
                    // A descriptografia falhou, o cookie não é válido
                }
            }

            // O cookie de autenticação não está presente ou a autenticação falhou, redirecione para a página de login
            filterContext.Result = new HttpUnauthorizedResult();
        }

        private bool ValidarCredenciais(string login, string senha)
        {
            // Aqui você implementa a lógica de validação do usuário com base nas credenciais fornecidas
            // Por exemplo, verifique se as credenciais correspondem a um registro válido no banco de dados

            // Retorna true se as credenciais são válidas, caso contrário, retorna false
            if (login == "anderson" && senha == "123456")
            {
                return true;
            }

            return false;
        }


        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                // O usuário não está autenticado, redirecione para a página de login
                filterContext.Result = new RedirectResult("~/Login");
            }
        }
    }

}