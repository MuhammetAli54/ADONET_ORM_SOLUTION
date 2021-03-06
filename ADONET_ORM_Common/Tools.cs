using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADONET_ORM_Common
{
    public static class Tools
    {
        public static string SQLConnectionStringSentence { get; set; } = "Server=DESKTOP-TUMHS1A;Database=OKULKITAPLIGI; Trusted_Connection=True;";

        private static SqlConnection _mysqlConnection;
        public static SqlConnection MySqlDBConnection
        {
            get
            {
                if (_mysqlConnection == null)
                {
                    _mysqlConnection = new SqlConnection(SQLConnectionStringSentence);
                }
                return _mysqlConnection;
            }

            set
            {
                _mysqlConnection = value;
            }

        }

        public static void OpenTheConnection()
        {
            try
            {
                if (MySqlDBConnection.State!=ConnectionState.Open)
                {
                    MySqlDBConnection.ConnectionString = SQLConnectionStringSentence;
                    MySqlDBConnection.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ET> ToList<ET>(this DataTable dt) where ET : class, new()
        {
            Type type = typeof(ET);
            List<ET> list = new List<ET>();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (DataRow rowitem in dt.Rows)
            {
                ET myET = new ET();

                foreach (PropertyInfo propertyitem in propertyInfos)
                {
                    object theobject = rowitem[propertyitem.Name];
                    if (theobject != null && theobject.ToString().Length > 0)
                    {
                        propertyitem.SetValue(myET, theobject);
                    }
                }
                list.Add(myET);
            }

            return list;
        }

        public static ET ToET<ET>(this DataTable dt) where ET : class, new()
        {
            Type theType = typeof(ET);
            ET entity = new ET();
            PropertyInfo[] propertyInfos = theType.GetProperties();
            foreach (DataRow rowitem in dt.Rows)
            {
                foreach (var propertyitem in propertyInfos)
                {
                    object theobject = rowitem[propertyitem.Name];
                    if (theobject != null && theobject.ToString().Length > 0)
                    {
                        propertyitem.SetValue(entity, theobject);
                    }
                }
            }
            return entity;
        }

    }
}
