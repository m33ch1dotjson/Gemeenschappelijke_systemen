using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Data
{
    public class PostRepository : IPostRepository 
    {
        private readonly MySqlConnection _connection;

        public PostRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<Post> GetByTitleAsync(string title, CancellationToken ct = default)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                await _connection.OpenAsync(ct);

            const string sql = "SELECT Content FROM Post WHERE Title = @Title";

            using var command = new MySqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@Title", title);

            using var reader = await command.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                var post = new Post();
                post.SetTitle(reader.GetString("Title"));
                post.SetContent(reader.GetString("Content"));

                return post;
            }

            return null;
        }

        public async Task AddAsync(Post post, CancellationToken ct)
        {
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync(ct);

            const string sql = """
            INSERT INTO Post (Title, Content)
            VALUES (@Title, @Content)
            """;

            using var cmd = new MySqlCommand(sql, _connection);
            cmd.Parameters.AddWithValue("@Title", post.GetTitle());
            cmd.Parameters.AddWithValue("@Username", post.GetContent());

            await cmd.ExecuteNonQueryAsync(ct);
        }

        public async Task UpdateContent(Post post, CancellationToken ct)
        {
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync(ct);

            const string sql = """
            UPDATE Post
            SET Content = @Content
            WHERE Title = @Title;
            """;

            using var cmd = new MySqlCommand(sql, _connection);
            cmd.Parameters.AddWithValue("@Title", post.GetTitle());
            cmd.Parameters.AddWithValue("@Content", post.GetContent());

            await cmd.ExecuteNonQueryAsync(ct);
        }

    }
}
