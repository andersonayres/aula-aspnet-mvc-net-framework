using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace LabWebForms.Models
{
    public class Cidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Estado Estado { get; set; }

        private static string ConnectionString = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

        public static List<Cidade> Todos(Estado estado)
        {
            if (estado != null)
            {
                return estado.Cidades();
            }

            return new List<Cidade>();
        }
    }

}
