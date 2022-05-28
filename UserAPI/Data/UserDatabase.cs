using Dapper;
using System.Data.SqlClient;

namespace UserAPI.Data
{
    public class UserDatabase
    {
        public async static Task<User> GetUser(string email, string password)
        {
            var user = new User();
            using (SqlConnection cnn = new SqlConnection(@"Data Source=DESKTOP-PI3FG32\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;TrustServerCertificate=False"))
            {
                cnn.Open();

                var dbPassword = await cnn.QuerySingleAsync<string>(@"SELECT Password FROM Users WHERE Email = @Email", new { Email = email });

                if (BCrypt.Net.BCrypt.EnhancedVerify(password, dbPassword))
                {
                    user = await cnn.QuerySingleAsync<User>(@"SELECT [Id]
      ,[Email]
      ,[FirstName]
      ,[SurName]
  FROM [dbo].[Users]
  WHERE Email = @Email
", new
                    {
                        Email = email,
                    });
                }
            }
            return user;
        }

        public async static Task RegisterUser(User user)
        {
            using (SqlConnection cnn = new SqlConnection(@"Data Source=DESKTOP-PI3FG32\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;TrustServerCertificate=False"))
            {
                cnn.Open();
                await cnn.ExecuteAsync(@"INSERT INTO [dbo].[Users]
           ([Email]
           ,[FirstName]
           ,[SurName]
           ,[Password])
     VALUES
           (@Email, @FirstName, @SurName, @Password)
", new
                {
                    Email = user.Email,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    SurName = user.Surname
                });
            }
        }
    }
}
