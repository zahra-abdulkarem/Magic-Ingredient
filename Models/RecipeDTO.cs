namespace DataAccese.Models
{
    public class RecipeDTO
    {
        public int RecipeID { get; set; }
        public string? RecipeName { get; set; }
        public string? RecipeIngredientsIDs { get; set; }
        public int RecipeCountryID { get; set; }
        public string? ImagePath { get; set; }

        public List<IngredientDTO> RecipeIngredients { get; set; }
        public CountryDTO Country { get; set; }

        public RecipeDTO(int recipeID , string? recipeName , string? recipeIngredientsIDs, int recipeCountryID, string? imagePath)
        {
            RecipeID = recipeID;
            RecipeName = recipeName;
            RecipeIngredientsIDs = recipeIngredientsIDs;
            RecipeCountryID = recipeCountryID;
            ImagePath = imagePath;

            Country = CountryData.FindCountry(RecipeCountryID);
            RecipeIngredients = IngredientData.FillIngredients(RecipeIngredientsIDs);
        }
    }
}
