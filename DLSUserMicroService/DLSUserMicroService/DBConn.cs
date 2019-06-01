using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserMicroServiceASP.Models;

namespace UserMicroServiceASP
{
    public class DBConn
    {
        // Azure
        string connection = "Server=userinfopostgresdb.postgres.database.azure.com;Database=users;Port=5432;User Id=dlsgroup2019@userinfopostgresdb;Password=Recommender2019;Ssl Mode=Require;";

        // Localhost
        //string connection = "Host=localhost;Username=postgres;Password=9p8zhrtwk;Database=users";

        public User CreateUser(User user)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connection))
                {
                    conn.Open();

                    // Insert some data
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO users (username, password, email, is_admin) VALUES (@u, @p, @e, @i) RETURNING user_id;";
                        cmd.Parameters.AddWithValue("u", user.Username);
                        cmd.Parameters.AddWithValue("p", user.Password);
                        cmd.Parameters.AddWithValue("e", user.Email);
                        cmd.Parameters.AddWithValue("i", user.IsAdmin);
                        var objid = cmd.ExecuteScalar();
                        var id = Convert.ToInt32(objid);
                        user.UserId = id;
                    }
                    conn.Close();
                }
                return user;
            }
            catch
            {
                return null;
            }
        }

        public int DeleteUser(int userId)
        {
            int affectedrows = -1;
            try
            {
                using (var conn = new NpgsqlConnection(connection))
                {
                    conn.Open();

                    // Insert some data
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "DELETE FROM users WHERE users.user_id = @u";
                        cmd.Parameters.AddWithValue("u", userId);
                        affectedrows = cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                return affectedrows;
            }
            catch
            {
                return -1;
            }
        }

        public User GetUser(int userId)
        {
            var u = new User();
            using (var conn = new NpgsqlConnection(connection))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM users WHERE user_id = " + userId, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        u.UserId = reader.GetInt32(0);
                        u.Username = reader.GetString(1);
                        u.Password = reader.GetString(2);
                        u.Email = reader.GetString(3);
                        u.IsAdmin = reader.GetBoolean(4);
                    }


                conn.Close();
            }
            return u;
        }

        public User Login(string username, string password)
        {
            var u = new User();
            using (var conn = new NpgsqlConnection(connection))
            {
                conn.Open();
                var cmdline = "SELECT * FROM users WHERE username = '" + username + "' AND password = '" + password + "'";
                using (var cmd = new NpgsqlCommand(cmdline, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        u.UserId = reader.GetInt32(0);
                        u.Username = reader.GetString(1);
                        u.Password = reader.GetString(2);
                        u.Email = reader.GetString(3);
                        u.IsAdmin = reader.GetBoolean(4);
                    }


                conn.Close();
            }
            return u;
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            using (var conn = new NpgsqlConnection(connection))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM users", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        var u = new User();
                        u.UserId = reader.GetInt32(0);
                        u.Username = reader.GetString(1);
                        u.Password = reader.GetString(2);
                        u.Email = reader.GetString(3);
                        u.IsAdmin = reader.GetBoolean(4);
                        users.Add(u);
                    }


                conn.Close();
            }
            return users;
        }

        public int UpdateUser(int userId, User user)
        {
            int affectedrows = -1;
            try
            {
                using (var conn = new NpgsqlConnection(connection))
                {
                    conn.Open();

                    // Insert some data
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE users SET username = @u, password = @p, email = @e, is_admin = @i WHERE user_id = " + userId;
                        cmd.Parameters.AddWithValue("u", user.Username);
                        cmd.Parameters.AddWithValue("p", user.Password);
                        cmd.Parameters.AddWithValue("e", user.Email);
                        cmd.Parameters.AddWithValue("i", user.IsAdmin);
                        affectedrows = cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                return affectedrows;
            }
            catch
            {
                return -1;
            }
        }
    }
}