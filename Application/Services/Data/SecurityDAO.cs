using CST247CLC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CST247CLC.Services.Data
{
    public class SecurityDAO
    {
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Minesweeper;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public bool LoginValidationByUserPass(User user)
        {
            
            string query = $"SELECT rtrim(USERNAME) FROM dbo.Users WHERE USERNAME = @Username AND PASSWORD = @Password";
            bool results = false;       //default assumption of result
            if (user != null)
            {
                using (SqlConnection con = new SqlConnection(connectionString)) //'using' ensures connections are closed after use.
                {
                    SqlCommand comm = new SqlCommand(query, con);
                    try
                    {
                        comm.Connection.Open();
                        comm.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 50).Value = user.Username;
                        comm.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 50).Value = user.Password;
                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.HasRows) { results = true; }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
            }
            return results;
        }

        public User LoadUser(User user)
        {
            string query = $"SELECT * FROM dbo.Users WHERE USERNAME = @Username AND PASSWORD = @Password";
            User save = new User();
            using (SqlConnection con = new SqlConnection(connectionString)) //'using' ensures connections are closed after use.
            {
                SqlCommand comm = new SqlCommand(query, con);
                try
                {
                    comm.Connection.Open();
                    comm.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 50).Value = user.Username;
                    comm.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 50).Value = user.Password;

                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        save.UserID = reader.GetGuid(reader.GetOrdinal("UserID"));
                        save.Username = reader["Username"].ToString();
                        save.Password = reader["Password"].ToString();
                        save.FirstName = reader["FirstName"].ToString();
                        save.LastName = reader["LastName"].ToString();
                        save.Sex = reader["Sex"].ToString();
                        save.Age = (int)reader["Age"];
                        save.State = reader["State"].ToString();
                        save.Email = reader["Email"].ToString();
                        save.savedBoard = JsonConvert.DeserializeObject<MinesweeperModels.Board>(reader["GameString"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return save;
            }
        }

        public bool Register(User user)
        {
            string query = $"INSERT INTO Users (USERNAME, PASSWORD, FIRSTNAME, LASTNAME, SEX, AGE, STATE, EMAIL) VALUES (@Username, @Password, @FirstName, @Lastname, @Sex, @Age, @State, @Email)";
            bool results = false;       //default assumption of result
            
            if (ValidateUser(user))
            {
                using (SqlConnection con = new SqlConnection(connectionString)) //'using' ensures connections are closed after use.
                {
                    SqlCommand comm = new SqlCommand(query, con);
                    comm.Parameters.AddWithValue("@Username", user.Username);
                    comm.Parameters.AddWithValue("@Password", user.Password);
                    comm.Parameters.AddWithValue("@FirstName", user.FirstName);
                    comm.Parameters.AddWithValue("@LastName", user.LastName);
                    comm.Parameters.AddWithValue("@Sex", user.Sex);
                    comm.Parameters.AddWithValue("@Age", user.Age);
                    comm.Parameters.AddWithValue("@State", user.State);
                    comm.Parameters.AddWithValue("@Email", user.Email);
                    try
                    {
                        comm.Connection.Open();
                        comm.ExecuteNonQuery();
                        results = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            return results;
        }

        //Runs the server-side validation of the user-input data
        //Currently, the validation is only checking for non-empty data fields
        public bool ValidateUser(User user)
        {
            bool validUser = false;
            if (user.FirstName.Length != 0 && 
                user.LastName.Length != 0 &&
                user.Sex.Length != 0 &&
                user.Age.ToString().Length != 0 &&
                user.State.ToString().Length != 0 &&
                (user.Email.ToString().Length != 0 || !user.Email.Contains("@")) &&
                user.Username.Length != 0 &&
                user.Password.Length != 0)
            {
                validUser = true;
            }
            Console.Out.WriteLine("validateUser: ", validUser);
            return validUser;
        }

    }
}