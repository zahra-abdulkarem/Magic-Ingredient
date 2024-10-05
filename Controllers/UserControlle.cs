using DataAccese.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserControlle : ControllerBase
    {

        [HttpGet("AllUsers", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDTO>> GetAllUsers()
        {
            List<UserDTO> Users = DataAccese.UserData.GetAllUsers();
            if (Users.Count == 0)
            {
                return NotFound("No Users Found!");
            }
            return Ok(Users);
        }


        [HttpGet("{id}", Name = "FindUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO> GetUserById(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Accepted id {id}");
            }

            UserDTO UDTO = DataAccese.UserData.GetUserByID(id);
            if (UDTO == null)
            {
                return NotFound($"User with id {id} not found");
            }
            return Ok(UDTO);
        }


        [HttpGet("UserName={UserName};Email={Email}", Name = "FindUserName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO> GetUser(string UserName, string Email)
        {
            if (UserName == ""|| Email == "")
            {
                return BadRequest($"Not Accepted UserName={UserName};Email={Email}");
            }

            UserDTO UDTO = DataAccese.UserData.GetUserByUserNameAndEmail(UserName , Email);
            if (UDTO == null)
            {
                return NotFound($"User with UserName={UserName};Email={Email} not found");
            }
            return Ok(UDTO);
        }


        [HttpPost(Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<UserDTO> AddNewUser(UserDTO NewUser)
        {
            if (NewUser == null || string.IsNullOrEmpty(NewUser.UserName) || string.IsNullOrEmpty(NewUser.Email) || string.IsNullOrEmpty(NewUser.ImagePath))
            {
                return BadRequest("Invalid User data.");
            }

            NewUser.UserID = DataAccese.UserData.AddNewUser(NewUser);
            
            return CreatedAtRoute("FindUser", new { id = NewUser.UserID }, NewUser);
        }


        [HttpDelete("DeleteUserID={id}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteUserByID(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Accepted ID : {id}.");
            }

            if (DataAccese.UserData.DeleteUser(id))
                return Ok($"User With ID : {id} Has Been Deleted.");
            else
                return NotFound($"User with id = {id} not found");
        }


        [HttpPut("{id}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> UpdateUser(int id, UserDTO updatedUser)
        {
            if (updatedUser == null || string.IsNullOrEmpty(updatedUser.UserName) || string.IsNullOrEmpty(updatedUser.Email) || string.IsNullOrEmpty(updatedUser.ImagePath))
            {
                return BadRequest("Invalid string.IsNullOrEmpty( data.");
            }

            UserDTO UDTO = DataAccese.UserData.GetUserByID(id);
            if (UDTO == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            UDTO.UserName = updatedUser.UserName;
            UDTO.Email = updatedUser.Email;
            UDTO.ImagePath = updatedUser.ImagePath;
            
            if (DataAccese.UserData.UpdateUser(UDTO))
                return Ok(UDTO);
            else
                return StatusCode(500, new { message = "Error Updating User" });
        }


        //------------------------------------------------------------------------------------------------------------------
        [HttpPost("UploadUserImage")]
        public async Task<IActionResult> UploadUserImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No File Upload.");

            var uploadDirectory = @"C:\Users\Windows 10\Desktop\Magic-Ingredient-Project\Server\Images\UserImages";
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
            var uploadDirectory = @"C:\Users\Windows 10\Desktop\Magic-Ingredient-Project\Server\Images\UserImages";
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

