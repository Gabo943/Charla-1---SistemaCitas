using SistemaTurnos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Charla_1___SistemaCitas
{
    public partial class Form1 : Form
    {
        //ESCENARIO#1: Uso de Colecciones Genéricas
        // 1. List
        private List<Ciudadano> listaHistorial = new List<Ciudadano>();

        // 2. Queue (Cola)
        private Queue<Ciudadano> filaTurnos = new Queue<Ciudadano>();

        // 3. Dictionary
        private Dictionary<string, decimal> catalogoTramites = new Dictionary<string, decimal>();

        public Form1()
        {
            InitializeComponent();
            ConfigurarSistema();
        }

        private void ConfigurarSistema()
        {
            // Llenamos el diccionario con los trámites y sus costos
            catalogoTramites.Add("Renovación de Cédula", 15.00m);
            catalogoTramites.Add("Inscripción de Nacimiento", 1.00m);
            catalogoTramites.Add("Certificado de Matrimonio", 5.50m);

            // Pasamos las llaves del diccionario al ComboBox
            cmbTramite.DataSource = catalogoTramites.Keys.ToList();

            // Conectamos el evento KeyPress de la cédula a nuestra clase de utilidades
            txtCedula.KeyPress += new KeyPressEventHandler(txtCedula_KeyPress);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // ESCENARIO#2: Evento KeyPress
        private void txtCedula_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Llamamos a la clase estática
            UtilidadesValidacion.PermitirSoloFormatoCedula(e);
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Limpiamos errores previos
            errorProvider1.Clear();

            // ESCENARIO#2: Try-Catch y ErrorProvider
            try
            {
                // 1. Validaciones
                if (!UtilidadesValidacion.EsCampoValido(txtCedula.Text))
                {
                    errorProvider1.SetError(txtCedula, "La cédula es obligatoria");
                    return;
                }
                if (!UtilidadesValidacion.EsCampoValido(txtNombre.Text))
                {
                    errorProvider1.SetError(txtNombre, "El nombre es obligatorio");
                    return;
                }

                // 2. Extracción de datos del diccionario
                string tramiteSeleccionado = cmbTramite.SelectedItem.ToString();
                decimal costoAplicado = catalogoTramites[tramiteSeleccionado];

                // 3. Creación del Objeto
                Ciudadano nuevoCiudadano = new Ciudadano
                {
                    Cedula = txtCedula.Text,
                    NombreCompleto = txtNombre.Text,
                    Tramite = tramiteSeleccionado,
                    Costo = costoAplicado,
                    HoraLlegada = DateTime.Now
                };

                // 4. Agregamos a las Colecciones Genéricas
                filaTurnos.Enqueue(nuevoCiudadano); // Entra a la fila de espera
                listaHistorial.Add(nuevoCiudadano); // Se guarda en el registro total

                ActualizarTabla();
                LimpiarCampos();
                MessageBox.Show("Turno registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Si algo falla, el programa no se cierra
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAtender_Click(object sender, EventArgs e)
        {
            if (filaTurnos.Count > 0)
            {
                //Dequeue saca a la persona de la fila o cola
                Ciudadano turnoActual = filaTurnos.Dequeue();

                lblTurnoActual.Text = $"Atendiendo a: {turnoActual.NombreCompleto} - {turnoActual.Tramite}";

                //Buscamos a esa misma persona en la lista
                //Buscamos coincidencia exacta por cédula y hora
                var ciudadanoAEliminar = listaHistorial.FirstOrDefault(c => c.Cedula == turnoActual.Cedula && c.HoraLlegada == turnoActual.HoraLlegada);

                //Si lo encontramos, lo borramos de la LISTA y refrescamos el Grid
                if (ciudadanoAEliminar != null)
                {
                    listaHistorial.Remove(ciudadanoAEliminar);
                    ActualizarTabla();
                }
            }
            else
            {
                MessageBox.Show("No hay personas en la fila de espera.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblTurnoActual.Text = "Esperando...";
            }
        }
        private void ActualizarTabla()
        {
            //Desconectar y volver a conectar para refrescar el DataGridView
            dgvHistorial.DataSource = null;
            dgvHistorial.DataSource = listaHistorial;
        }

        private void LimpiarCampos()
        {
            txtCedula.Clear();
            txtNombre.Clear();
            txtCedula.Focus();
        }
    }
}
