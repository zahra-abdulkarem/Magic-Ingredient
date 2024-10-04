using DataAccese.Models;
using Microsoft.Data.SqlClient;


namespace DataAccese
{
    public class UserData
    {

        public static List<UserDTO> GetAllUsers()
        {
            var List = new List<UserDTO>();


            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Users", conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {

                                List.Add(new UserDTO(
                                    reader.GetInt32(reader.GetOrdinal("UserID")),
                                    reader.GetString(reader.GetOrdinal("UserName")).Trim(),
                                    reader.GetString(reader.GetOrdinal("Email")).Trim(),
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
        
        public static int AddNewUser(UserDTO NewUser)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("AddNewUser", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserName", NewUser.UserName);
                cmd.Parameters.AddWithValue("@Email", NewUser.Email);
                cmd.Parameters.AddWithValue("@ImagePath", NewUser.ImagePath);
                var outputIdParam = new SqlParameter("@NewUserID", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                cmd.Parameters.Add(outputIdParam);
                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outputIdParam.Value;

            }
        }

        public static bool UpdateUser(UserDTO UpdatedUser)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("UpdateUser", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserName", UpdatedUser.UserName);
                cmd.Parameters.AddWithValue("@Email", UpdatedUser.Email);
                cmd.Parameters.AddWithValue("@ImagePath", UpdatedUser.ImagePath);

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }

            return false;
        }

        public static bool DeleteUser(int ID)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("DeleteUser", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", ID);
                conn.Open();
                int rowsAffected = (int)cmd.ExecuteScalar();
                return (rowsAffected == 1);
            }

            return false;
        }


        public static UserDTO GetUserByID(int ID)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE UserID = @UserID", conn))
            {

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UserID", ID);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserDTO(
                            reader.GetInt32(reader.GetOrdinal("UserID")),
                            reader.GetString(reader.GetOrdinal("UserName")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("ImagePath"))
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


        public static UserDTO GetUserByUserNameAndEmail(string UserName, string Email)
        {
            using (SqlConnection conn = new SqlConnection(Settings.ConnactionString))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE UserName = @UserName AND Email = @Email", conn))
            {

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Email", Email);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserDTO(
                            reader.GetInt32(reader.GetOrdinal("UserID")),
                            reader.GetString(reader.GetOrdinal("UserName")),
                            reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("ImagePath"))
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
    }
}
