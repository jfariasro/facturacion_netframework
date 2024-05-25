using CapaDatos;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class frmNuevaVenta : Form
    {
        Venta venta = new Venta();
        DetalleVenta detalle = new DetalleVenta();
        public frmNuevaVenta()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int contador = dataGridView1.RowCount;
            if(contador == 1)
            {
                txtCliente.Clear();
                txtCliente.ReadOnly = false;
                txtTotal.Clear();
            }
            txtProducto.Clear();
            txtCantidad.Clear();
            txtPrecio.Clear();
            rdb1.Checked = false;
            rdb2.Checked = false;
            Program.detalle = 0;
        }

        private void frmNuevaVenta_Load(object sender, EventArgs e)
        {
            if(Program.idventa != 0 && dataGridView1.RowCount > 1)
            {
                MostrarVenta();

                MostrarDetalles();
            }
        }

        private void MostrarVenta()
        {
            venta.idventa = Program.idventa;
            venta = venta.Consultar();

            txtCliente.Text = venta.cliente;

            txtCliente.ReadOnly = true;
        }

        private void MostrarDetalles()
        {
            DataSet ds = new DataSet();
            detalle.idventa = Program.idventa;
            ds = detalle.Consultar();

            dataGridView1.Rows.Clear();

            float total = 0;

            foreach (DataRow fila in ds.Tables[0].Rows)
            {
                dataGridView1.Rows.Add(null, null, fila[0], fila[1], fila[2], fila[3], fila[4], fila[5], fila[6], fila[7]);
                total += float.Parse(fila[7].ToString());
            }

            txtTotal.Text = total.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string m = "";

            if(txtCliente.Text.Length == 0)
            {
                m += "Falta Ingresar Cliente\n";
            }
            if (txtProducto.Text.Length == 0)
            {
                m += "Falta Ingresar Producto\n";
            }
            if (txtCantidad.Text.Length == 0)
            {
                m += "Falta Ingresar Cantidad\n";
            }
            if (txtPrecio.Text.Length == 0)
            {
                m += "Falta Ingresar Precio\n";
            }
            if (rdb1.Checked == false && rdb2.Checked == false)
            {
                m += "Seleccione una opción del IVA\n";
            }

            if(m.Length > 0)
            {
                MessageBox.Show(m, "FALTA INGRESAR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if(dataGridView1.RowCount == 1 && Program.idventa == 0)
                {
                    venta.cliente = txtCliente.Text;

                    Program.idventa = venta.Insertar();
                }

                detalle.iva = 0;
                if (rdb1.Checked)
                {
                    detalle.iva = 0.12f;
                }

                detalle.idventa = Program.idventa;
                detalle.producto = txtProducto.Text;
                detalle.cantidad = int.Parse(txtCantidad.Text);
                detalle.precio = float.Parse(txtPrecio.Text);
                detalle.total = detalle.precio * detalle.cantidad;
                detalle.precio_iva = detalle.precio + (detalle.precio * detalle.iva);
                detalle.total_iva = detalle.precio_iva * detalle.cantidad;

                if(Program.detalle == 0)
                {
                    string mensaje = detalle.Insertar();
                }
                else
                {
                    detalle.iddetalle = Program.detalle;
                    string mensaje = detalle.Modificar();
                }

                MostrarVenta();
                MostrarDetalles();

                button2_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            venta.idventa = Program.idventa;
            var reader = venta.ConsultarTotal();

            detalle.idventa = detalle.idventa;
            DataSet datos = new DataSet();
            datos = detalle.Consultar();

            Document document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            string rutaArchivo = @"C:\Users\Ruben\OneDrive\Documentos\Factura\factura-" + Program.idventa + ".pdf";
            PdfWriter.GetInstance(document, new FileStream(rutaArchivo, FileMode.Create));
            document.Open();

            if (reader.Read())
            {
                // Crear encabezado de la factura con la información obtenida
                Paragraph encabezado = new Paragraph();
                encabezado.Alignment = Element.ALIGN_CENTER;
                encabezado.Add(new Chunk("Factura #" + venta.idventa.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1252, Font.Bold)));
                encabezado.Add(Chunk.NEWLINE);
                encabezado.Add(new Chunk("Fecha: " + reader.GetDateTime(1).ToShortDateString(), FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                encabezado.Add(Chunk.NEWLINE);
                encabezado.Add(new Chunk("Cliente: " + reader.GetString(2), FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                encabezado.Add(Chunk.NEWLINE);
                encabezado.Add(new Chunk("Total: " + reader.GetDecimal(3).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                encabezado.Add(Chunk.NEWLINE);
                encabezado.Add(new Chunk("Total a pagar: " + reader.GetDecimal(4).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                encabezado.Add(Chunk.NEWLINE);
                encabezado.Add(Chunk.NEWLINE);
                document.Add(encabezado);
            }

            // Agregar detalles de la venta
            PdfPTable tabla = new PdfPTable(7);
            tabla.WidthPercentage = 100;
            tabla.SetWidths(new float[] { 2f, 1f, 1f, 1f, 1f, 1f, 1f });
            tabla.AddCell("Producto");
            tabla.AddCell("Cantidad");
            tabla.AddCell("Precio");
            tabla.AddCell("Iva");
            tabla.AddCell("Precio Iva");
            tabla.AddCell("Total");
            tabla.AddCell("Total con IVA");

            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                tabla.AddCell(fila[1].ToString());
                tabla.AddCell(fila[2].ToString());
                tabla.AddCell(fila[3].ToString());
                tabla.AddCell(fila[4].ToString());
                tabla.AddCell(fila[5].ToString());
                tabla.AddCell(fila[6].ToString());
                tabla.AddCell(fila[7].ToString());
            }

            document.Add(tabla);

            // Cerrar el documento
            document.Close();

            LimpiarTodo(sender, e);

            // Abrir el archivo PDF
            System.Diagnostics.Process.Start(rutaArchivo);
        }

        private void LimpiarTodo(object sender, EventArgs e)
        {
            Program.idventa = 0;
            Program.detalle = 0;
            venta = new Venta();
            detalle = new DetalleVenta();
            dataGridView1.Rows.Clear();
            button2_Click(sender, e);

            MessageBox.Show("Venta Generada", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
