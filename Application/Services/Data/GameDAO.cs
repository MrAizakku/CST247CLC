using CST247CLC.Models;
using MinesweeperModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CST247CLC.Services.Data
{
    public class GameDAO
    {
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Minesweeper;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public bool SaveGame(User user, Board board)
        {
            string query = $"UPDATE dbo.Users SET GameString = @GameString  WHERE UserID = (@UserID)";
            bool results = false;       //default assumption of result
            using (SqlConnection con = new SqlConnection(connectionString)) //'using' ensures connections are closed after use.
            {
                SqlCommand comm = new SqlCommand(query, con);
                comm.Parameters.AddWithValue("@GameString", JsonConvert.SerializeObject(board));
                comm.Parameters.AddWithValue("@UserID", user.UserID);
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
            return results;
        }
        public bool ClearSave(User user)
        {
            string query = $"UPDATE dbo.Users SET GameString = @GameString  WHERE UserID = (@UserID)";
            bool results = false;       //default assumption of result
            using (SqlConnection con = new SqlConnection(connectionString)) //'using' ensures connections are closed after use.
            {
                SqlCommand comm = new SqlCommand(query, con);
                comm.Parameters.AddWithValue("@GameString", DBNull.Value);
                comm.Parameters.AddWithValue("@UserID", user.UserID);
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
            return results;
        }
    }
}