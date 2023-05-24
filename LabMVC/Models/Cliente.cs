using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Npgsql;

namespace LabWebForms.Models
{
    public class Cliente
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public Cidade Cidade { get; set; }

        public Estado Estado { get; set; }

        public void Atualizar(int id)
        {
            this.Id = id;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string query = "UPDATE Clientes SET @Login = Login, Senha = @Senha, Nome = @Nome, Email = @Email, id_cidade = @id_cidade WHERE id == @Id";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", Login);
                    command.Parameters.AddWithValue("@Senha", Senha);
                    command.Parameters.AddWithValue("@Nome", Nome);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@idCidade", Cidade.Id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void Salvar()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string query = "INSERT INTO Clientes (Login, Senha, Nome, Email, id_cidade) VALUES (@Login, @Senha, @Nome, @Email, @idCidade)";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", Login);
                    command.Parameters.AddWithValue("@Senha", Senha);
                    command.Parameters.AddWithValue("@Nome", Nome); 
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@idCidade", Cidade.Id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Cliente> Todos()
        {
            List<Cliente> clientes = new List<Cliente>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT Id, Login, Senha, Nome, id_cidade, Email FROM Clientes";
                NpgsqlCommand command = new NpgsqlCommand(query, connection);

                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Cliente cliente = new Cliente
                    {
                        Id = (int)reader["Id"],
                        Login = reader["Login"].ToString(),
                        Senha = reader["Senha"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Email = reader["Email"].ToString()
                    };

                    if (reader["id_cidade"] != DBNull.Value)
                        cliente.Cidade = new Cidade() { Id = Convert.ToInt32(reader["id_cidade"]) };

                    clientes.Add(cliente);
                }
            }
            return clientes;
        }
        public static Cliente BuscarPorId(int _id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT * FROM Clientes WHERE Id = @Id";
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", _id);
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    cliente.Login = reader.GetString(reader.GetOrdinal("Login"));
                    cliente.Senha = reader.GetString(reader.GetOrdinal("Senha"));
                    cliente.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                    cliente.Email = reader.GetString(reader.GetOrdinal("Email"));
                    // Preencha os outros campos conforme necessário
                    return cliente;
                }
                return null;
            }
        }
        public static Cliente BuscarPorUsername(string username)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT * FROM Clientes WHERE Login = @Username";
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    cliente.Login = reader.GetString(reader.GetOrdinal("Login"));
                    cliente.Senha = reader.GetString(reader.GetOrdinal("Senha"));
                    cliente.Nome = reader.GetString(reader.GetOrdinal("Nome"));
                    cliente.Email = reader.GetString(reader.GetOrdinal("Email"));
                    
                    return cliente;
                }
                return null;
            }
        }



    }
}
