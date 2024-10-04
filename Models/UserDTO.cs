using Microsoft.Extensions.Primitives;

namespace DataAccese.Models
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }

        public UserDTO(int userID , string userName, string email, string imagePath)
        {
            UserID = userID;
            UserName = userName;
            Email = email;
            ImagePath = imagePath;
        }
    }
}
