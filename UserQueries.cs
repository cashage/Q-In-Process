using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;


namespace QueueInProcess
{
    public class UserQueries
    {

        public int Id { get; set; }

        public string Query { get; set; }

        public int MappedId { get; set; }

        public int IsAnswer { get; set; }
        public int IsDeleted { get; set; }


        public List<UserQueries> GetAllQueries(int pagenumber)
        {
            List<UserQueries> UQs = new List<UserQueries>();

            string CS = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("usp_GetQueries", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@pagenumber", System.Data.SqlDbType.Int).Value = pagenumber;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserQueries us = new UserQueries();
                    try
                    {
                        us.Id = (int)reader["Id"];
                        us.Query = reader["Query"] != DBNull.Value ? reader["Query"].ToString() : String.Empty;
                        us.MappedId = (int)reader["MappedID"];
                        us.IsAnswer = (int)reader["IsAnswer"];
                        if (us != null)
                        {
                            UQs.Add(us);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }

            return UQs;
        }


        public void PostQuery(UserQueries query)
        {

            string CS = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("usp_PostQuery", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Query", System.Data.SqlDbType.VarChar).Value = query.Query;
                    cmd.Parameters.Add("@MappedID", System.Data.SqlDbType.Int).Value = query.MappedId;
                    cmd.Parameters.Add("@IsAnswer", System.Data.SqlDbType.Int).Value = query.IsAnswer;
                    cmd.Parameters.Add("@IsDeleted", System.Data.SqlDbType.Int).Value = query.IsDeleted;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}