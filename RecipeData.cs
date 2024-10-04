using Microsoft.Data.SqlClient;
using DataAccese.Models;

namespace DataAccese
{
    public class RecipeData
    {

        public static List<RecipeDTO> GetAllRecipes()
        {
            var List = new List<RecipeDTO>();


            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Recipes", conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                List.Add(new RecipeDTO(
                                    reader.GetInt32(reader.GetOrdinal("RecipeID")),
                                    reader.GetString(reader.GetOrdinal("RecipeName")).Trim(),
                                    reader.GetString(reader.GetOrdinal("RecipeIngredientsIDs")).Trim(),
                                    reader.GetInt32(reader.GetOrdinal("RecipeCountryID")),
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

        public static RecipeDTO FindRecipe(int ID)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("GetRecipeById", conn))
            {

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RecipeId", ID);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new RecipeDTO(
                            reader.GetInt32(reader.GetOrdinal("RecipeID")),
                            reader.GetString(reader.GetOrdinal("RecipeName")).Trim(),
                            reader.GetString(reader.GetOrdinal("RecipeIngredientsIDs")).Trim(),
                            reader.GetInt32(reader.GetOrdinal("RecipeCountryID")),
                            reader.GetString(reader.GetOrdinal("ImagePath")).Trim()
                        );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }


        public static int AddNewRecipe(RecipeDTO RDTO)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("AddNewRecipe", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RecipeName", RDTO.RecipeName);
                cmd.Parameters.AddWithValue("@RecipeIngredientsIDs", RDTO.RecipeIngredientsIDs);
                cmd.Parameters.AddWithValue("@RecipeCountryID", RDTO.RecipeCountryID);
                cmd.Parameters.AddWithValue("@ImagePath", RDTO.ImagePath);
                var outputIdParam = new SqlParameter("@RecipeID", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                cmd.Parameters.Add(outputIdParam);
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputIdParam.Value;

            }
        }

        public static bool UpdateRecipe(RecipeDTO RDTO)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("UpdateRecipe", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RecipeID", RDTO.RecipeID);
                cmd.Parameters.AddWithValue("@RecipeName", RDTO.RecipeName);
                cmd.Parameters.AddWithValue("@RecipeIngredientsIDs", RDTO.RecipeIngredientsIDs);
                cmd.Parameters.AddWithValue("@RecipeCountryID", RDTO.RecipeCountryID);
                cmd.Parameters.AddWithValue("@ImagePath", RDTO.ImagePath);

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }

            return false;
        }

        public static bool DeleteRecipe(int Id)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("DeleteRecipe", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RecipeID", Id);
                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return (rowsAffected == 1);
            }

            return false;
        }


    }
}
