using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
        /// <summary>
        /// Singleton factory to provide a single shared instance of MySqlConnection.
        /// Ensures that the connection is only created once and reused throughout the application.
        /// </summary>
        public class DbConnectionFactory
        {
            private static MySqlConnection _connection;

            private static readonly object _lock = new();

            private static readonly string _connectionString = "Server=localhost;Database=your_db;User=root;Password=your_pw;";

            /// <summary>
            /// Returns a single shared MySqlConnection instance.
            /// Creates a new one if it doesn't already exist.
            /// </summary>
            public static MySqlConnection GetConnection()
            {
                // Check if connection is not yet created
                if (_connection == null)
                {
                    lock (_lock)
                    {
                        if (_connection == null)
                        {
                            _connection = new MySqlConnection(_connectionString);
                        }
                    }
                }

                return _connection;
            }
        }
    }
