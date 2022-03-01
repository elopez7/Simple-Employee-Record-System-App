using System.Data;
using System.Data.SQLite;
using Dapper;
using RecordLibrary.Interfaces;

namespace RecordLibrary.SQLiteHelpers
{
    public class SQLiteDataAccess : IDataAccess
    {
        public List<T> LoadData<T, U>(string sqlStament, U parameters, string connectionString)
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStament, parameters).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement, T parameters, string connectionString)
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters);
            }
        }
    }
}
