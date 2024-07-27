using IdentityServerApi.Host.Configurations;
using IdentityServerApi.Host.Data;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Repositories.Interfaces;
using Npgsql;

namespace IdentityServerApi.Host.Repositories
{
    public class UserBffAccountRepository(IConfiguration config, ApplicationDbContext context) : IUserBffAccountRepository
    {
        public async Task<GeneralResponse<List<SearchAdminUserResponse>>> AdminGetUsersByNameAsync(string userName)
        {
            var sqlQuery = @"
                SELECT u.""UserName"", r.""Name"" AS ""RoleName""
                FROM public.""AspNetUsers"" u
                JOIN public.""AspNetUserRoles"" ur ON u.""Id"" = ur.""UserId""
                JOIN public.""AspNetRoles"" r ON ur.""RoleId"" = r.""Id""
                WHERE LOWER(u.""UserName"") LIKE @userName
                LIMIT 5";

            List<SearchAdminUserResponse> searchedUsers = new List<SearchAdminUserResponse>();

            using (NpgsqlConnection connection = new NpgsqlConnection(config["ConnectionString"]))
            {
                NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@userName", "%" + userName.ToLower() + "%");

                await connection.OpenAsync();

                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        searchedUsers.Add(new SearchAdminUserResponse
                        {
                            UserName = reader.GetString(0),
                            RoleName = reader.GetString(1)
                        });
                    }
                }
            }

            if (!searchedUsers.Any())
            {
                return new GeneralResponse<List<SearchAdminUserResponse>>(false, $"Not found any users by {userName}", null!);
            }

            return new GeneralResponse<List<SearchAdminUserResponse>>(true, "Successfully", searchedUsers);
        }

        public async Task<GeneralResponse<List<SearchUserResponse>>> UserGetUsersByNameAsync(string userName)
        {
            var sqlQuery = @"SELECT u.""UserName"", u.""Id"" 
                    FROM public.""AspNetUsers"" u
                    WHERE LOWER(u.""UserName"") LIKE @userName
                    LIMIT 5";
            List<SearchUserResponse> searchedUsers = new List<SearchUserResponse>();

            using (NpgsqlConnection connection = new NpgsqlConnection(config["ConnectionString"]))
            {
                NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@userName", "%" + userName.ToLower() + "%");

                await connection.OpenAsync();

                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        searchedUsers.Add(new SearchUserResponse
                        {
                            UserName = reader.GetString(0),
                            Id = reader.GetString(1)
                        });
                    }
                }
            }

            if (!searchedUsers.Any())
            {
                return new GeneralResponse<List<SearchUserResponse>>(false, $"Not found any users by {userName}", null!);
            }

            return new GeneralResponse<List<SearchUserResponse>>(true, "Successfully", searchedUsers);
        }
    }
}
