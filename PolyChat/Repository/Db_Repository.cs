using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace PolyChat.Repository
{
    public class Db_Repository
    {
        private string _connectionString;
        public Db_Repository()
        {

            var config = new ConfigurationBuilder().AddUserSecrets<Db_Repository>()
                        .Build();
            _connectionString = config.GetConnectionString("dbConn");
        }

        public bool AddUser(string firstname, string lastname)
        {
            //metod för att lägga till observatör
            string CheckIfLetters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvXxYyZzÅåÄäÖö@.";
            foreach (char c in firstname)
            {
                if (!CheckIfLetters.Contains(c))
                {
                    return false;
                }
            }

            foreach (char c in lastname)
            {
                if (c == ' ')
                {
                    return false;
                }
            }

            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                StringBuilder sql = new StringBuilder("insert into PolyUser");
                sql.AppendLine("(email ,PolyPassword) ");
                sql.AppendLine("values(@email , @PolyPassword) ");
                using var command = new NpgsqlCommand(sql.ToString(), conn);
                command.Parameters.AddWithValue("@email", firstname);
                command.Parameters.AddWithValue("@PolyPassword", lastname);
                var result = command.ExecuteScalar();
                return true;
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "23503")
                {
                    if (ex.ConstraintName == "person_sex_id_fkey")
                    {
                        throw new Exception("fel kön", ex);
                    }
                    throw new Exception("Du försöker ge en rank till en pirat som inte existerar", ex);
                }
                throw new Exception("Allvarligt fel, får inte kontakt med databasen", ex);

            }

            return false;

        }




        public List<PolyUser> GetPolyUsers()
        {

            try
            {
                var PolyUsers = new List<PolyUser>();
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand();
                cmd.CommandText = "Select email, polypassword FROM polyuser ORDER BY email";
                cmd.Connection = conn;
                PolyUser? polyuser = null;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        polyuser = new PolyUser();
                        {
                           // polyuser.id = reader.GetInt32(0);
                            polyuser.email = reader["email"] == DBNull.Value ? null : (string?)reader["email"];
                            polyuser.PolyPassword = reader["PolyPassword"] == DBNull.Value ? null : (string?)reader["PolyPassword"];
                        }
                        PolyUsers.Add(polyuser);
                    }
                }
                return PolyUsers;
            }
            catch (PostgresException ex)
            {
                throw new Exception("Någonting i databasen stämmer inte överens med koden i programmet. Specifikt i metoden GetObservers()", ex);
            }
        }


        public bool CheckIfEmailIsRegistered(List<PolyUser> list, string email)
        {

            try
            {
                List<PolyUser>? PolyUsers = new List<PolyUser>();
                PolyUsers = list;

                foreach (var item in PolyUsers)
                {
                    if (email == item.email)
                    {
                        return false;
                    }
                    if (email != item.email)
                    {
                        return true;
                    }

                }
                return true;
            }
            catch (PostgresException ex)
            {
                throw new Exception("Någonting i databasen stämmer inte överens med koden i programmet. ", ex);
                return false;
            }
        }


        



        public bool CheckIfUserExist(string email, string password)
        {

            try
            {
                var PolyUsers = new List<PolyUser>();
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                StringBuilder sql = new StringBuilder("Select * from polyuser ");
                sql.AppendLine("where email =@email");
                using var cmd = new NpgsqlCommand(sql.ToString(), conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@PolyPassword", password);
             

                PolyUser? polyuser = null;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        polyuser = new PolyUser();
                        {
                            polyuser.email = reader["email"] == DBNull.Value ? null : (string?)reader["email"];
                            polyuser.PolyPassword = reader["PolyPassword"] == DBNull.Value ? null : (string?)reader["PolyPassword"];
                        }
                        PolyUsers.Add(polyuser);
                    }
                }
                foreach (var item in PolyUsers)
                {
                    if (item == null)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (PostgresException ex)
            {
                throw new Exception("Någonting i databasen stämmer inte överens med koden i programmet. Specifikt i metoden GetObservers()", ex);
            }
        }





        public bool CheckIfPasswordMatchesUser(string email, string password)
        {


            try
            {
                string PassToLower = password.ToLower();
                var PolyUsers = new List<PolyUser>();
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                StringBuilder sql = new StringBuilder("Select * FROM polyuser ");
                sql.AppendLine("where email =@email ");
                using var cmd = new NpgsqlCommand(sql.ToString(), conn);
                cmd.Parameters.AddWithValue("@email", email);



                PolyUser? polyuser = null;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        polyuser = new PolyUser();
                        {
                            // polyuser.id = reader.GetInt32(0);
                            polyuser.email = reader["email"] == DBNull.Value ? null : (string?)reader["email"];
                            polyuser.PolyPassword = reader["PolyPassword"] == DBNull.Value ? null : (string?)reader["PolyPassword"];
                        }
                        PolyUsers.Add(polyuser);
                    }
                }
                foreach (var item in PolyUsers)
                {
                    if (item.PolyPassword == PassToLower)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (PostgresException ex)
            {
                throw new Exception("Någonting i databasen stämmer inte överens med koden i programmet. Specifikt i metoden GetObservers()", ex);
            }
        }

























    }
}
