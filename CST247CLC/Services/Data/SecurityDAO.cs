using CST247CLC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CST247CLC.Services.Data
{
    public class SecurityDAO
    {
        public bool LoginValidationByUserPass(User user)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string query = $"SELECT rtrim(USERNAME) FROM dbo.Users WHERE USERNAME = @Username AND PASSWORD = @Password";
            bool results = false;       //default assumption of result

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
            return results;
        }

        public bool Register(User user)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Minesweeper;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string query = $"INSERT INTO dbo.Users rtrim(USERNAME, PASSWORD, FIRSTNAME, LASTNAME, SEX, AGE, STATE, EMAIL) VALUES (@Username, @Password, @FirstName, @Lastname, @Sex, @Age, @State, @Email)";
            bool results = false;       //default assumption of result
            //Check to see if the user data made it to the server in a valid configuration, meaning that all of the fields have something in them, and the email field contains an @ sign, could be upgraded later.
            if (validateUser(user))
            {

                //results = true;//Confirmed this works up to this point...  Starting here it doesn't
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
                    comm.Parameters.AddWithValue("@Email",  user.Email);
                    try
                    {
                        comm.Connection.Open();
                        
                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.HasRows) { results = true; }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }

            }
            return results;
        }

        //Runs the server-side validation of the user-input data
        //Currently, the validation is only checking for non-empty data fields
        public bool validateUser(User user)
        {
            bool validUser = false;
            if (user.FirstName.Length != 0)
            {
                validUser = true;
            }
            else if (user.LastName.Length != 0)
            {
                validUser = true;
            }
            else if (user.Sex.Length != 0)
            {
                validUser = true;
            }
            //Don't care what the number is, just want to make sure there is something there.
            else if (user.Age.ToString().Length != 0)
            {
                validUser = true;
            }
            else if (user.State.ToString().Length != 0)
            {
                validUser = true;
            }
            //If it's empty or doesn't have an @, fail the validation check
            else if (user.Email.ToString().Length != 0 || !user.Email.Contains("@"))
            {
                validUser = true;
            }
            else if (user.Username.Length != 0)
            {
                validUser = true;
            }
            else if (user.Password.Length != 0)
            {
                validUser = true;
            }
            return validUser;
        }

    }
}