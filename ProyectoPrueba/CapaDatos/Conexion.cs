using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    class Conexion
    {
        private SqlConnection conexion = new SqlConnection("Data Source=DESKTOP-R5G525J;Initial Catalog=prueba;Integrated Security=True;");

        public SqlConnection Conectar()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                    conexion.Open();
                return conexion;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
