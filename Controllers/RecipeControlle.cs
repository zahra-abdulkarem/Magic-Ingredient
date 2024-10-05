using DataAccese.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeControlle : ControllerBase
    {

        [HttpGet("Recipes", Name = "GetAllRecipes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<RecipeDTO>> GetAllRecipes()
        {
            List<RecipeDTO> Recipes = DataAccese.RecipeData.GetAllRecipes();
            if (Recipes.Count == 0)
            {
                return NotFound("No Recipe Found!");
            }
            return Ok(Recipes);
        }


        [HttpGet("{id}", Name = "FindRecipe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<RecipeDTO> GetRecipeById(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Accepted id {id}");
            }

            RecipeDTO UDTO = DataAccese.RecipeData.FindRecipe(id);
            if (UDTO == null)
            {
                return NotFound($"Recipe with id {id} not found");
            }
            return Ok(UDTO);
        }


        [HttpPost(Name = "AddNewRecipe")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<RecipeDTO> AddNewRecipe(RecipeDTO NewRecipe)
        {
            if (NewRecipe == null || string.IsNullOrEmpty(NewRecipe.RecipeName) || string.IsNullOrEmpty(NewRecipe.RecipeIngredientsIDs) || string.IsNullOrEmpty(NewRecipe.ImagePath) || NewRecipe.RecipeCountryID > 0)
            {
                return BadRequest("Invalid Recipe data.");
            }

            NewRecipe.RecipeID = DataAccese.RecipeData.AddNewRecipe(NewRecipe);

            return CreatedAtRoute("FindRecipe", new { id = NewRecipe.RecipeID }, NewRecipe);
        }


        [HttpDelete("{id}", Name = "DeleteRecipe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteRecipe(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Accepted ID : {id}.");
            }

            if (DataAccese.UserData.DeleteUser(id))
                return Ok($"Recipe With ID : {id} Has Been Deleted.");
            else
                return NotFound($"Recipe with id = {id} not found");
        }


        [HttpPut("{id}", Name = "UpdateRecipe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RecipeDTO> UpdateRecipe(int id, RecipeDTO updatedRecipe)
        {
            if (updatedRecipe == null || string.IsNullOrEmpty(updatedRecipe.RecipeName) || string.IsNullOrEmpty(updatedRecipe.ImagePath) || string.IsNullOrEmpty(updatedRecipe.RecipeIngredientsIDs) || updatedRecipe.RecipeCountryID > 0)
            {
                return BadRequest("Invalid Recipe");
            }

            RecipeDTO RDTO = DataAccese.RecipeData.FindRecipe(id);
            if (RDTO == null)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }

            RDTO.RecipeName = updatedRecipe.RecipeName;
            RDTO.RecipeIngredients = updatedRecipe.RecipeIngredients;
            RDTO.RecipeCountryID = updatedRecipe.RecipeCountryID;
            RDTO.ImagePath = updatedRecipe.ImagePath;

            if (DataAccese.RecipeData.UpdateRecipe(RDTO))
                return Ok(RDTO);
            else
                return StatusCode(500, new { message = "Error Updating Recipe" });
        }


        //------------------------------------------------------------------------------------------------------------------
        [HttpPost("UploadRecipeImage")]
        public async Task<IActionResult> UploadRecipeImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No File Upload.");

            var uploadDirectory = @"C:\Users\Windows 10\Desktop\Magic-Ingredient-Project\Server\Images\RecipeImages";
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(uploadDirectory);


            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Ok(new { filePath });
        }


        [HttpGet("GetImage/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            var uploadDirectory = @"C:\Users\Windows 10\Desktop\Magic-Ingredient-Project\Server\Images\RecipeImages";
            var filePath = Path.Combine(uploadDirectory, fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Image not found.");


            var Image = System.IO.File.OpenRead(filePath);
            var mimeType = GetMimeType(filePath);
            return File(Image, mimeType);
        }

        private string GetMimeType(string filePath)
        {
            var extention = Path.GetExtension(filePath).ToLowerInvariant();
            return extention switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image.png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }
    }
}
