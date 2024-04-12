using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dat = Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace Data
{
    public class Connection
    {
        private Dat.Sql.SqlDatabase sqldatabase;

        public Connection()
        {
            conectarBaseDatos();
        }
        private void conectarBaseDatos()
        {
            string DB = ConfigurationManager.AppSettings["DB"].ToString();
            string Server = ConfigurationManager.AppSettings["Instance"].ToString();
            string User = ConfigurationManager.AppSettings["UserName"].ToString();
            string Password = ConfigurationManager.AppSettings["Password"].ToString();
            string App = ConfigurationManager.AppSettings["ApplicationName"].ToString();
            string Connec = string.Format("Database={0};Server={1};User ID={2};Password={3};Application Name={4};Connect Timeout=1500", DB, Server, User, Password, App);
            try
            {
                sqldatabase = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(Connec);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        public DataSet ejecDataSet(string sql)
        {
            return sqldatabase.ExecuteDataSet(CommandType.Text, sql);
        }

        public DataTable ejecReader(string sql)
        {
            DataTable dt = new DataTable();
            dt.Load(sqldatabase.ExecuteReader(CommandType.Text, sql));
            return dt;
        }


        public DataTable ejecutarDataSet(string proc, params object[] values)
        {
            DbCommand cmd = sqldatabase.GetStoredProcCommand(proc, values);
            cmd.CommandTimeout = 0;
            DataTable dt = new DataTable();
            dt.Load(sqldatabase.ExecuteReader(cmd));
            return dt;
        }

    
        public DataTable ejecDataTable(string proc, params object[] values)
        {
            DataTable dt = new DataTable();
            try
            {
                DbCommand cmd = sqldatabase.GetStoredProcCommand(proc, values);
                cmd.CommandTimeout = 0;
                DataSet ds = sqldatabase.ExecuteDataSet(cmd);
                if (ds.Tables.Count > 0) dt = ds.Tables[0].Copy();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ejecScalar(string proc, params object[] values)
        {
            DbCommand cmd = sqldatabase.GetStoredProcCommand(proc, values);
            cmd.CommandTimeout = 0;
            return Convert.ToString(sqldatabase.ExecuteScalar(cmd));
        }

    }
}
