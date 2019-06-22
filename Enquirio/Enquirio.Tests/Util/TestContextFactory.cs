using System;
using System.Collections.Generic;
using System.Data.Common;
using Enquirio.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

// Class for creating an in memory Sqlite test database and creating
// contexts to access it
namespace Enquirio.Tests.Util {
    class TestContextFactory : IDisposable {

        private DbConnection _connection;
        private readonly List<string> _sampleData;
        private readonly string _url = "DataSource =:memory:";
        
        // Factory can optionally be passed a list of sql statements
        // to create sample data
        public TestContextFactory(List<string> sampleData = null) {
            _sampleData = sampleData;
        }
        
        public DbContextEnq GetContext() {
            // The first time GetContext is called a new connection will be created
            if (_connection == null) {
                _connection = new SqliteConnection(_url);
                _connection.Open();

                using (var context = new DbContextEnq(GetOptions())) {
                    context.Database.EnsureCreated();

                    // Add sample data if present
                    if (_sampleData != null) {
                        foreach (var command in _sampleData) {
                            context.Database.ExecuteSqlCommand(command);
                        }
                    }
                }
            }

            return new DbContextEnq(GetOptions());
        }

        public void Dispose() {
            if (_connection != null) {
                _connection.Dispose();
                _connection = null;
            }
        }

        private DbContextOptions<DbContextEnq> GetOptions() {
            return new DbContextOptionsBuilder<DbContextEnq>()
                .UseSqlite(_connection).Options;
        }

    }
}
