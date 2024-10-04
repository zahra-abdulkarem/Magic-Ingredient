using DataAccese.Models;
using Microsoft.Data.SqlClient;

namespace DataAccese
{
    public class CountryData
    {
        
        public static List<CountryDTO> GetAllCountries()
        {
            var List = new List<CountryDTO>();


            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Countries", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                List.Add(new CountryDTO(
                                    reader.GetInt32(reader.GetOrdinal("CountryID")),
                                    reader.GetString(reader.GetOrdinal("CountryName")).Trim()
                                ));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }

            return List;
        }

        public static CountryDTO FindCountry(int ID)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("GetCountry", conn))
            {

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CountryID", ID);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CountryDTO(
                            reader.GetInt32(reader.GetOrdinal("CountryID")),
                            reader.GetString(reader.GetOrdinal("CountryName")).Trim()
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }


        public static int AddNewCountry(CountryDTO CDTO)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("AddNewCountry", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CountryName", CDTO.CountryName);
                
                var outputIdParam = new SqlParameter("@CountryID", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                cmd.Parameters.Add(outputIdParam);
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputIdParam.Value;

            }
        }

        public static bool UpdateCountry(CountryDTO CDTO)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("UpdateCountry", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CountryID", CDTO.CountryID);
                cmd.Parameters.AddWithValue("@CountryName", CDTO.CountryName);
                

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }

            return false;
        }

        public static bool DeleteCountry(int Id)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("DeleteCountry", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CountryID", Id);
                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return (rowsAffected == 1);
            }

            return false;
        }


    }
}

