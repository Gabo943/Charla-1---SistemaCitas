using System;

namespace SistemaTurnos
{
    public class Ciudadano
    {
        public string Cedula { get; set; }
        public string NombreCompleto { get; set; }
        public string Tramite { get; set; }
        public decimal Costo { get; set; }
        public DateTime HoraLlegada { get; set; }
    }
}
