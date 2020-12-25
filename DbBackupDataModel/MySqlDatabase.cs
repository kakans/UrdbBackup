using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupDataModel
{
    public class MySqlDatabase
    {
        private MySqlConnection _connection = null;
        private MySqlCommand _command = null;

        public MySqlDatabase()
        {
            if (_connection == null)
            {
                _connection = new MySqlConnection(DbConfig.GetConnectionString());
            }
        }
        public MySqlDatabase(string dataSource,string username,string password)
        {
            _connection = new MySqlConnection (string.Format("Data Source={0};User Id={1};Password={2}", dataSource, username, password));
        }
        public MySqlDatabase(string dataSource, string username, string password,string database)
        {
            _connection = new MySqlConnection(string.Format("Data Source={0};User Id={1};Password={2};database={3}", dataSource, username, password, database));
        }
        public DataSet GetDataSet(string commandText, CommandType commandType)
        {
            DataSet ds = new DataSet();
            _connection.Open();
            try
            {
                _command = new MySqlCommand();
                _command.CommandText = commandText; // query
                _command.CommandType = commandType; // command type 
                _command.Connection = _connection;

                MySqlDataAdapter da = new MySqlDataAdapter(_command);
                da.Fill(ds);
                _connection.Close();
                _command.Dispose();
            }
            catch (System.Exception)
            {
                throw;
            }
            return ds;
        }
        public DataSet GetDataSet(string commandText, CommandType commandType, MySqlParameter[] param)
        {
            DataSet ds = new DataSet();
            _connection.Open();
            try
            {
                _command = new MySqlCommand();
                _command.CommandText = commandText;
                _command.CommandType = commandType;
                _command.Connection = _connection;

                foreach (var p in param)
                {
                    _command.Parameters.Add(p);
                }

                MySqlDataAdapter da = new MySqlDataAdapter(_command);

                da.Fill(ds);
                _connection.Close();
                _command.Dispose();
            }
            catch (System.Exception)
            {
                throw;
            }
            return ds;
        }
        public void ExecuteNonQuery(string commandText, CommandType commandType)
        {
            _connection.Open();
            try
            {
                _command = new MySqlCommand();
                _command.CommandText = commandText;
                _command.CommandType = commandType;
                _command.Connection = _connection;
                _command.ExecuteNonQuery();
                _connection.Close();
                _command.Dispose();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public void ExecuteNonQuery(string commandText, CommandType commandType, MySqlParameter[] param)
        {
            _connection.Open();
            try
            {
                _command = new MySqlCommand();
                _command.CommandText = commandText;
                _command.CommandType = commandType;
                _command.Connection = _connection;
                foreach (var p in param)
                {
                    _command.Parameters.Add(p);
                }
                _command.ExecuteNonQuery();
                _connection.Close();
                _command.Dispose();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public void ExecuteNonQuery(string commandText, CommandType commandType, MySqlParameter[] param, out long LastInsertedId)
        {
            _connection.Open();
            try
            {
                _command = new MySqlCommand();
                _command.CommandText = commandText;
                _command.CommandType = commandType;
                _command.Connection = _connection;
                foreach (var p in param)
                {
                    _command.Parameters.Add(p);
                }
                _command.ExecuteNonQuery();
                LastInsertedId = _command.LastInsertedId;
                _connection.Close();
                _command.Dispose();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

    }
}
