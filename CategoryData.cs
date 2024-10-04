using DataAccese.Models;
using Microsoft.Data.SqlClient;

namespace DataAccese
{
    public class CategoryData
    {
        public static CategoryDTO Find(int ID)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("GetCategory", conn))
            {

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryId", ID);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CategoryDTO(
                            reader.GetInt32(reader.GetOrdinal("CategoryID")),
                            reader.GetString(reader.GetOrdinal("Name")).Trim()
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
