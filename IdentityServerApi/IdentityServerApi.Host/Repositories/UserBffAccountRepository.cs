using IdentityServerApi.Host.Configurations;
using IdentityServerApi.Host.Data;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityServerApi.Host.Repositories
{
    public class UserBffAccountRepository(UserManager<UserApp> userManager, IConfiguration config) : IUserBffAccountRepository
    {
        public async Task<GeneralResponse<List<UserResponse>>> GetUsersByNameAsync(string userName)
        {
            var sqlQuery = @"
                SELECT u.""UserName"", r.""Name"" AS ""RoleName""
                FROM public.""AspNetUsers"" u
                JOIN public.""AspNetUserRoles"" ur ON u.""Id"" = ur.""UserId""
                JOIN public.""AspNetRoles"" r ON ur.""RoleId"" = r.""Id""
                WHERE LOWER(u.""UserName"") LIKE @userName
                LIMIT 5";

            List<UserResponse> users = new List<UserResponse>();

            using (NpgsqlConnection connection = new NpgsqlConnection(config["ConnectionString"]))
            {
                NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@userName", "%" + userName.ToLower() + "%");

                connection.Open();

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new UserResponse
                        {
                            UserName = reader.GetString(0),
                            RoleName = reader.GetString(1)
                        });
                    }
                }
            }

            if (users == null)
            {
                return new GeneralResponse<List<UserResponse>>(false, $"Not found any users by {userName}", null!);
            }

            return new GeneralResponse<List<UserResponse>>(true, "Successfully", users);
        }
    }
}
