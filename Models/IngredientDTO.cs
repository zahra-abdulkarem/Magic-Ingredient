namespace DataAccese.Models
{
    public class IngredientDTO
    {
        public int IngredientId { get; set; }
        public string? IngredientName { get; set; }
        public int CategoryID { get; set; }
        public string? ImagePath { get; set; }

        public CategoryDTO Category { get; set; }

        public IngredientDTO(int ingredientId, string? ingredientName, int categoryID, string? imagePath)
        {
            IngredientId = ingredientId;
            IngredientName = ingredientName;
            CategoryID = categoryID;
            ImagePath = imagePath;
            Category = CategoryData.Find(CategoryID);
        }
    }
}
