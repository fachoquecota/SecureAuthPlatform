using AuthAPI.Data.Modules.Auth.Interfaces;
using AuthAPI.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace AuthAPI.Data.Modules.Auth
{
    public class AuthSP : IAuth
    {
        private readonly Connection _connectionHelper;

        public AuthSP(Connection connectionHelper)
        {
            _connectionHelper = connectionHelper;
        }

        public async Task CreateUserAsync(UserModel user)
        {
            using (var connection = new SqlConnection(_connectionHelper.getCadenaSQL()))
            {
                await connection.OpenAsync();
                try
                {
                    using (var command = new SqlCommand("spCreateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agrega los parámetros necesarios para el procedimiento almacenado
                        command.Parameters.Add(new SqlParameter("@username", user.username));
                        command.Parameters.Add(new SqlParameter("@passwordHash", user.passwordHash));
                        command.Parameters.Add(new SqlParameter("@email", user.email));
                        command.Parameters.Add(new SqlParameter("@firstName", user.firstName));
                        command.Parameters.Add(new SqlParameter("@lastName", user.lastName));

                        // Ejecutar el procedimiento almacenado
                        await command.ExecuteNonQueryAsync();
                    }
                }
                finally
                {
                    // Asegúrate de cerrar la conexión después de ejecutar el comando
                    await connection.CloseAsync();
                }
            }
        }

        public async Task<UserModel> AuthenticateUserAsync(string username)
        {
            UserModel authenticatedUser = null;

            using (var connection = new SqlConnection(_connectionHelper.getCadenaSQL()))
            {
                await connection.OpenAsync();
                try
                {
                    using (var command = new SqlCommand("spAuthenticateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agrega los parámetros necesarios para el procedimiento almacenado
                        command.Parameters.Add(new SqlParameter("@username", username));

                        // Ejecutar el procedimiento almacenado y leer los resultados
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                authenticatedUser = new UserModel
                                {
                                    userId = reader.GetInt32(reader.GetOrdinal("userId")),
                                    username = reader.GetString(reader.GetOrdinal("username")),
                                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                                    passwordHash = reader.GetString(reader.GetOrdinal("passwordHash")),
                                    firstName = reader.IsDBNull(reader.GetOrdinal("firstName")) ? null : reader.GetString(reader.GetOrdinal("firstName")),
                                    lastName = reader.IsDBNull(reader.GetOrdinal("lastName")) ? null : reader.GetString(reader.GetOrdinal("lastName")),                                                                              
                                };
                            }
                        }
                    }
                }
                finally
                {
                    // Asegúrate de cerrar la conexión después de ejecutar el comando
                    await connection.CloseAsync();
                }
            }
            return authenticatedUser;
        }
    }
}

