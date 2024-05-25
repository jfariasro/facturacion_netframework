using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class Venta
    {
        public int idventa { get; set; }
        public string cliente { get; set; }

        Conexion conexion = new Conexion();

        public int Insertar()
        {
            try
            {
                using (var comando = new SqlCommand("InsertarVenta", conexion.Conectar()))
                {
                    comando.Parameters.AddWithValue("@cliente", cliente);
                    comando.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.ExecuteNonQuery();

                    int id = Convert.ToInt32(comando.Parameters["@id"].Value);
                    return id;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public string Modificar()
        {
            try
            {
                using (var comando = new SqlCommand("ModificarVenta", conexion.Conectar()))
                {
                    comando.Parameters.AddWithValue("@idventa", idventa);
                    comando.Parameters.AddWithValue("@cliente", cliente);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.ExecuteNonQuery();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Venta Consultar()
        {
            try
            {
                using (var comando = new SqlCommand("ConsultarVenta", conexion.Conectar()))
                {
                    comando.Parameters.AddWithValue("@idventa", idventa);
                    comando.CommandType = CommandType.StoredProcedure;
                    var reader = comando.ExecuteReader();

                    Venta venta = new Venta();
                    while (reader.Read())
                    {
                        venta.idventa = reader.GetInt32(0);
                        venta.cliente = reader.GetString(2);
                    }

                    return venta;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public SqlDataReader ConsultarTotal()
        {
            try
            {
                using (var comando = new SqlCommand("consultar_venta_total", conexion.Conectar()))
                {
                    comando.Parameters.AddWithValue("@idventa", idventa);
                    comando.CommandType = CommandType.StoredProcedure;
                    var reader = comando.ExecuteReader();

                    return reader;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Eliminar()
        {
            try
            {
                using (var comando = new SqlCommand("EliminarVenta", conexion.Conectar()))
                {
                    comando.Parameters.AddWithValue("@idventa", idventa);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.ExecuteNonQuery();

                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
