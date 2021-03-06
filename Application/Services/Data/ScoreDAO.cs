﻿using CST247CLC.Models;
using MinesweeperModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CST247CLC.Services.Data
{
    public class ScoreDAO
    {
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Minesweeper;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public bool SaveScore(User user, PlayerStat score)
        {
            string query = $"INSERT INTO dbo.Scores (userID, score, difficulty, clicks) VALUES (@UserID, @Score, @Difficulty, @Clicks)";
            bool results = false;       //default assumption of result
            if (score.Score > 0)
            {
                using (SqlConnection con = new SqlConnection(connectionString)) //'using' ensures connections are closed after use.
                {
                    SqlCommand comm = new SqlCommand(query, con);
                    comm.Parameters.AddWithValue("@UserID", user.UserID);
                    comm.Parameters.AddWithValue("@Score", score.Score);
                    comm.Parameters.AddWithValue("@Difficulty", score.Difficulty);
                    comm.Parameters.AddWithValue("@Clicks", score.Clicks);
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


        public List<PlayerStat> GetAllScores()
        {
            string query = $"SELECT [U].[FirstName], [U].[LastName], [S].[difficulty], [S].[score], [S].[clicks] FROM [dbo].[Users] AS U JOIN [dbo].[Scores] AS S ON [U].[UserID] = [S].[userID]";
            List<PlayerStat> scores = new List<PlayerStat>();
            using (SqlConnection con = new SqlConnection(connectionString)) //'using' ensures connections are closed after use.
            {
                SqlCommand comm = new SqlCommand(query, con);
                try
                {
                    comm.Connection.Open();

                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        PlayerStat score = new PlayerStat
                        {
                            PlayerName = reader["FirstName"].ToString() + " " + reader["LastName"].ToString(),
                            Score = (int)reader["score"],
                            Clicks = (int)reader["clicks"],
                            Difficulty = reader["difficulty"].ToString()
                        };
                        scores.Add(score);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                scores.Sort();
                return scores.Where(m => m.Score > 0).ToList();
            }
        }

        public List<PlayerStat> GetUserScores(User user)
        {
            string query = $"select * from [dbo].[Scores] where [userID] = @UserID";
            List<PlayerStat> scores = new List<PlayerStat>();
            using (SqlConnection con = new SqlConnection(connectionString)) //'using' ensures connections are closed after use.
            {
                SqlCommand comm = new SqlCommand(query, con);
                try
                {
                    comm.Connection.Open();
                    comm.Parameters.Add("@UserID", System.Data.SqlDbType.UniqueIdentifier);
                    comm.Parameters["@UserID"].Value = user.UserID;
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        PlayerStat score = new PlayerStat
                        {
                            PlayerName = user.FirstName + " " + user.LastName,
                            Score = (int)reader["score"],
                            Clicks = (int)reader["clicks"],
                            Difficulty = reader["difficulty"].ToString()
                        };
                        scores.Add(score);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                scores.Sort();
                return scores.Where(m => m.Score > 0).ToList();
            }
        }

        public List<PlayerStat> GetUserScoresByName(string name)
        {
            string query = $"SELECT [U].[FirstName], [U].[LastName], [S].[difficulty], [S].[score], [S].[clicks] FROM [dbo].[Users] AS U JOIN [dbo].[Scores] AS S ON [U].[UserID] = [S].[userID] WHERE [FirstName] = @Name OR [LastName] = @Name";
            List<PlayerStat> scores = new List<PlayerStat>();
            using (SqlConnection con = new SqlConnection(connectionString)) //'using' ensures connections are closed after use.
            {
                SqlCommand comm = new SqlCommand(query, con);
                try
                {
                    comm.Connection.Open();
                    comm.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 50).Value = name;
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        PlayerStat score = new PlayerStat
                        {
                            PlayerName = reader["FirstName"].ToString() + " " + reader["LastName"].ToString(),
                            Score = (int)reader["score"],
                            Clicks = (int)reader["clicks"],
                            Difficulty = reader["difficulty"].ToString()
                        };
                        scores.Add(score);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                scores.Sort();
                return scores.Where(m => m.Score > 0).ToList();
            }
        }

    }
}