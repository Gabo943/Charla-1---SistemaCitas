using System.Windows.Forms;

namespace SistemaTurnos
{
    // Clase estática: no necesita instanciarse y centraliza la seguridad
    public static class UtilidadesValidacion
    {
        public static bool EsCampoValido(string texto)
        {
            return !string.IsNullOrWhiteSpace(texto);
        }

        public static void PermitirSoloFormatoCedula(KeyPressEventArgs e)
        {
            // Solo permite números, guiones y la tecla de borrar (BackSpace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true; // Bloquea cualquier otra tecla
            }
        }
    }
}
