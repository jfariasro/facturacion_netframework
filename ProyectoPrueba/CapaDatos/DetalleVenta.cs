using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class DetalleVenta
    {
        public int iddetalle { get; set; }
        public int idventa { get; set; }
        public string producto { get; set; }
        public int cantidad { get; set; }
        public float precio { get; set; }
        public float iva {  get; set; } = 0;
        public float precio_iva { get; set; }
        public float total { get; set; }
        public float total_iva { get; set; }

        Conexion conexion = new Conexion();

        public string Insertar()
        {
            try
            {
                using (var comando = new SqlCommand("insertar_detalle", conexion.Conectar()))
                {
                    comando.Parameters.AddWithValue("@idventa", idventa);
                    comando.Parameters.AddWithValue("@producto", producto);
                    comando.Parameters.AddWithValue("@cantidad", cantidad);
                    comando.Parameters.AddWithValue("@precio", precio);
                    comando.Parameters.AddWithValue("@iva", iva);
                    comando.Parameters.AddWithValue("@precio_iva", precio_iva);
                    comando.Parameters.AddWithValue("@total", total);
                    comando.Parameters.AddWithValue("@total_iva", total_iva);
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

        public string Modificar()
        {
            try
            {
                using (var comando = new SqlCommand("modificar_detalle", conexion.Conectar()))
                {
                    comando.Parameters.AddWithValue("@iddetalleventa", iddetalle);
                    comando.Parameters.AddWithValue("@idventa", idventa);
                    comando.Parameters.AddWithValue("@producto", producto);
                    comando.Parameters.AddWithValue("@cantidad", cantidad);
                    comando.Parameters.AddWithValue("@precio", precio);
                    comando.Parameters.AddWithValue("@iva", iva);
                    comando.Parameters.AddWithValue("@precio_iva", precio_iva);
                    comando.Parameters.AddWithValue("@total", total);
                    comando.Parameters.AddWithValue("@total_iva", total_iva);
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

        public string Eliminar()
        {
            try
            {
                using (var comando = new SqlCommand("elimnar_detalle", conexion.Conectar()))
                {
                    comando.Parameters.AddWithValue("@iddetalleventa", iddetalle);
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

        public DataSet Consultar()
        {
            try
            {
                DataSet datos = new DataSet();
                using (var comando = new SqlCommand("consultar_detalle_venta", conexion.Conectar()))
                {
                    comando.Parameters.AddWithValue("@idventa", idventa);
                    comando.CommandType = CommandType.StoredProcedure;

                    using (var adaptador = new SqlDataAdapter())
                    {
                        adaptador.SelectCommand = comando;
                        adaptador.Fill(datos);
                    }
                }

                return datos;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
