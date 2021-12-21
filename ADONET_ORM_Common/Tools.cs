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
        private static SqlConnection _mysqlConnection;
        public static SqlConnection MySqlDBConnection 
        { 
            get
            {
                if (_mysqlConnection==null)
                {
                    _mysqlConnection = new SqlConnection("Server=DESKTOP-TUMHS1A;Database=NORTHWND; Trusted_Connection=True;");
                }
                return _mysqlConnection;
            }

            set
            {
                _mysqlConnection = value;
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
                    object o = rowitem[propertyitem.Name];
                    if (o!= null)
                    {
                        propertyitem.SetValue(myET, o);
                    }
                }
                list.Add(myET);
            }

            return list;
        }
    }
}
