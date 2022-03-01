using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLibrary.Interfaces
{
    public interface IDataAccess
    {
        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString);
        public void SaveData<T>(string sqlStatement, T parameters, string connectionString);

    }
}
