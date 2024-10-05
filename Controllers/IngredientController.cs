using Microsoft.AspNetCore.Mvc;
using DataAccese.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        [HttpGet("Ingredients", Name = "GetAllIngredients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<IngredientDTO>> GetAllIngredients()
        {
            List<IngredientDTO> Ingredients = DataAccese.IngredientData.GetAllIngredients();
            if (Ingredients.Count == 0)
            {
                return NotFound("No Ingredient Found!");
            }
            return Ok(Ingredients);
        }
        

        [HttpGet("{id}", Name = "FindIngredient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IngredientDTO> GetIngredientById(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Accepted id {id}");
            }

            IngredientDTO IDTO = DataAccese.IngredientData.FindIngredient(id);
            if (IDTO == null)
            {
                return NotFound($"Ingredient with id {id} not found");
            }
            return Ok(IDTO);
        }

        [HttpPost(Name = "AddNewIngredient")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IngredientDTO> AddNewIngredient(IngredientDTO NewIngredient)
        {
            if (NewIngredient == null || string.IsNullOrEmpty(NewIngredient.IngredientName) || NewIngredient.CategoryID < 0)
            {
                return BadRequest("Invalid Ingredient data.");
            }

            IngredientDTO Ingredient = new IngredientDTO(NewIngredient.IngredientId ,NewIngredient.IngredientName , NewIngredient.CategoryID , NewIngredient.ImagePath);
            DataAccese.IngredientData.AddNewIngredient(Ingredient);
            Ingredient.IngredientId = Ingredient.IngredientId;
            return CreatedAtRoute("FindIngredient", new { id = NewIngredient.IngredientId }, NewIngredient);
        }

        [HttpDelete("{id}", Name = "DeleteIngredientById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteIngredientById(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Accepted ID : {id}.");
            }

            if (DataAccese.IngredientData.DeleteIngredient(id))
                return Ok($"Ingredient With ID : {id} Has Been Deleted.");
            else
                return NotFound($"Ingredient with id = {id} not found");
        }

        [HttpPut("{id}", Name = "UpdateIngredient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IngredientDTO> UpdateIngredient(int id, IngredientDTO updatedIngredient)
        {
            if (updatedIngredient == null || string.IsNullOrEmpty(updatedIngredient.IngredientName) || updatedIngredient.CategoryID < 0 || string.IsNullOrEmpty(updatedIngredient.ImagePath))
            {
                return BadRequest("Invalid Ingredient data.");
            }

            IngredientDTO IDTO = DataAccese.IngredientData.FindIngredient(id);
            if (IDTO == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }


            IDTO.IngredientName = updatedIngredient.IngredientName;
            IDTO.CategoryID = updatedIngredient.CategoryID;
            IDTO.ImagePath = updatedIngredient.ImagePath;
            if (DataAccese.IngredientData.UpdateIngredient(IDTO))
                return Ok(IDTO);
            else
                return StatusCode(500, new { message = "Error Updating Ingredient" });
        }

        [HttpPost("UploadIngredientImage")]
        public async Task<IActionResult> UploadIngredientImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No File Upload.");

            var uploadDirectory = @"C:\Users\Windows 10\Desktop\Magic-Ingredient-Project\Server\Images\IngredientsImages";
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
            var uploadDirectory = @"C:\Users\Windows 10\Desktop\Magic-Ingredient-Project\Server\Images\IngredientsImages";
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
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }
    }

}
