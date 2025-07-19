using ContactAgendaApi.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace ContactAgendaApi.Repositories;

// Sample Dapper repository for demonstration
public class DapperContactRepository
{
    // Connection string for SQLite database
    private readonly string _connectionString;

    // Constructor receives configuration to get the connection string
    public DapperContactRepository(IConfiguration configuration)
    {
        // Use the connection string from appsettings.json or fallback to local file
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=contacts.db";
    }

    // Example: Get all contacts using Dapper (raw SQL)
    public IEnumerable<Contact> GetAllContacts()
    {
        // Open a connection to the SQLite database
        using var connection = new SqliteConnection(_connectionString);
        // Execute a SQL query and map results to Contact objects
        return connection.Query<Contact>("SELECT Id, Name, Email, Phone FROM Contacts").ToList();
    }
}
