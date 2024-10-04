using Microsoft.Data.SqlClient;
using DataAccese.Models;
using System.Data;

namespace DataAccese
{

    public class IngredientData
    {
        
        public static List<IngredientDTO> GetAllIngredients()
        {
            var List = new List<IngredientDTO>();
            

            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select * from Ingredients", conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                List.Add(new IngredientDTO(
                                    reader.GetInt32(reader.GetOrdinal("IngredientID")),
                                    reader.GetString(reader.GetOrdinal("IngredientName")).Trim(),
                                    reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                    reader.GetString(reader.GetOrdinal("ImagePath")).Trim()
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

        public static IngredientDTO FindIngredient(int ID)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("GetIngredientById", conn))
            {

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IngredientID", ID);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new IngredientDTO(
                            reader.GetInt32(reader.GetOrdinal("IngredientID")),
                            reader.GetString(reader.GetOrdinal("IngredientName")).Trim(),
                            reader.GetInt32(reader.GetOrdinal("CategoryID")),
                            reader.GetString(reader.GetOrdinal("ImagePath")).Trim()
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


        public static int AddNewIngredient(IngredientDTO IDTO)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("AddNewIngredient", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IngredientName", IDTO.IngredientName);
                cmd.Parameters.AddWithValue("@CategoryID", IDTO.CategoryID);
                cmd.Parameters.AddWithValue("@ImagePath", IDTO.ImagePath);
                var outputIdParam = new SqlParameter("@NewIngredientID", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                cmd.Parameters.Add(outputIdParam);
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputIdParam.Value;

            }
        }

        public static bool UpdateIngredient(IngredientDTO IDTO)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("UpdateIngredient", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IngredientID", IDTO.IngredientId);
                cmd.Parameters.AddWithValue("@IngredientName", IDTO.IngredientName);
                cmd.Parameters.AddWithValue("@CategoryID", IDTO.CategoryID);
                cmd.Parameters.AddWithValue("@ImagePath", IDTO.ImagePath);

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }

            return false;
        }

        public static bool DeleteIngredient(int Id)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("DeleteIngredient", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IngredientID", Id);
                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return (rowsAffected == 1);
            }

            return false;
        }


        //should be in the buiccness layer
        public static List<IngredientDTO> FillIngredients(string RecipeIngredientsIDs)
        {
            List<IngredientDTO> RecipeIngredients = new List<IngredientDTO>();

            string[] IngredientRecords = RecipeIngredientsIDs.Split(',');
            for (int i = 0; i < IngredientRecords.Length; i++)
            {
                IngredientDTO Ingredient = IngredientData.FindIngredient(Convert.ToInt32(IngredientRecords[i].Trim()));
                if (Ingredient != null)
                {
                    RecipeIngredients.Add(Ingredient);
                }
            }

            return RecipeIngredients;
        }

        public static List<string> GetAllImages()
        {
            var List = new List<string>();


            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ImagePath FROM Ingredients", conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                List.Add(reader.GetString(reader.GetOrdinal("ImagePath")).Trim());
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



    }

    

    /*public class Ingredient
    {

    } */
}
