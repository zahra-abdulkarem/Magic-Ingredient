namespace DataAccese.Models
{
    public class CategoryDTO
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }

        public CategoryDTO(int categoryID, string name)
        {
            CategoryID = categoryID;
            Name = name;
        }
    }
}
