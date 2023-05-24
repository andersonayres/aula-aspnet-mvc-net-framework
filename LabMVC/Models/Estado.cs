using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace LabWebForms.Models
{
    public class Estado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string UF { get; set; }

        private static string ConnectionString = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

        public override string ToString()
        {
            return this.Nome;
        }

        public static List<Estado> Todos()
        {
            List<Estado> estados = new List<Estado>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = "SELECT id, nome, uf FROM Estados";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Estado estado = new Estado();
                            estado.Id = reader.GetInt32(0);
                            estado.Nome = reader.GetString(1);
                            estado.UF = reader.GetString(2);
                            estados.Add(estado);
                        }
                    }
                }
            }

            return estados;
        }
        public List<Cidade> Cidades()
        {
            List<Cidade> cidades = new List<Cidade>();

            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = "SELECT id, nome, id_estado FROM Cidades where id_estado = @estadoId";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@estadoId", Id);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cidade cidade = new Cidade();
                            cidade.Id = reader.GetInt32(0);
                            cidade.Nome = reader.GetString(1);
                            cidade.Estado = this; // Define o estado atual como o estado da cidade
                            cidades.Add(cidade);
                        }
                    }
                }
            }

            return cidades;
        }
    }
}


