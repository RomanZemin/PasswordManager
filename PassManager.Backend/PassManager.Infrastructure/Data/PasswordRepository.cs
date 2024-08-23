using PassManager.Domain.Entities;
using PassManager.Application.Interfaces;
using Npgsql;
using Microsoft.Extensions.Configuration;


namespace PassManager.Infrastructure.Data
{
    public class PasswordRepository : IPasswordRepository
    {
        private readonly string _connectionString;

        public PasswordRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(_connectionString), "Connection string 'DefaultConnection' not found.");
        }

        public async Task<IEnumerable<PasswordRecord>> GetPasswordsAsync()
        {
            var passwords = new List<PasswordRecord>();

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new NpgsqlCommand("SELECT * FROM PasswordRecords ORDER BY CreatedAt DESC", connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                passwords.Add(new PasswordRecord
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Password = reader.GetString(2),
                    Type = reader.GetString(3),
                    CreatedAt = reader.GetDateTime(4)
                });
            }

            return passwords;
        }

        public async Task<PasswordRecord?> GetPasswordByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand("SELECT * FROM PasswordRecords WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new PasswordRecord
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Password = reader.GetString(2),
                    Type = reader.GetString(3),
                    CreatedAt = reader.GetDateTime(4)
                };
            }

            return null;
        }

        public async Task AddPasswordAsync(PasswordRecord passwordRecord)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand("INSERT INTO PasswordRecords (Name, Password, Type, CreatedAt) VALUES (@Name, @Password, @Type, @CreatedAt)", connection);
            command.Parameters.AddWithValue("@Name", passwordRecord.Name);
            command.Parameters.AddWithValue("@Password", passwordRecord.Password);
            command.Parameters.AddWithValue("@Type", passwordRecord.Type);
            command.Parameters.AddWithValue("@CreatedAt", passwordRecord.CreatedAt);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> PasswordExistsAsync(string name)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand("SELECT COUNT(1) FROM PasswordRecords WHERE Name = @Name", connection);
            command.Parameters.AddWithValue("@Name", name);

            var result = await command.ExecuteScalarAsync();
            return result != null && (long)result > 0;
        }

        public async Task DeletePasswordAsync(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new NpgsqlCommand("DELETE FROM PasswordRecords WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();

        }
    }
}
