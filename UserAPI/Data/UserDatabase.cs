using Dapper;
using Serilog;
using System.Data.SqlClient;

namespace UserAPI.Data
{
    public class UserDatabase
    {
        public async static Task<User> GetUser(string email, string password)
        {
            var user = new User();
            try
            {
                using SqlConnection cnn = new SqlConnection(@"Data Source=DESKTOP-PI3FG32\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;TrustServerCertificate=False");

                cnn.Open();

                var dbPassword = await cnn.QuerySingleAsync<string>(@"SELECT Password FROM Users WHERE Email = @Email", new { Email = email });

                if (BCrypt.Net.BCrypt.EnhancedVerify(password, dbPassword))
                {
                    user = await cnn.QuerySingleOrDefaultAsync<User>(@"
SELECT [Id]
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
                return user;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return null;
            }
        }

        public async static Task<string> RegisterUser(User user)
        {
            if (await CheckIfUserExists(user.Email))
            {
                return "Użytkownik o podanym mailu już istnieje";
            }

            try
            {
                using SqlConnection cnn = new SqlConnection(@"Data Source=DESKTOP-PI3FG32\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;TrustServerCertificate=False");
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
                return "Użytkownik został dodany do bazy";
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return "Wystąpił błąd podczas dodawania użytkownika";
            }

        }

        public async static Task<bool> CheckIfUserExists(string email)
        {
            try
            {
                using SqlConnection cnn = new SqlConnection(@"Data Source=DESKTOP-PI3FG32\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True;TrustServerCertificate=False");
                cnn.Open();

                var user = await cnn.QuerySingleOrDefaultAsync<User>(@"SELECT Email FROM Users WHERE Email = @Email", new { Email = email });

                return user != null;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                throw;
            }

        }
    }
}
