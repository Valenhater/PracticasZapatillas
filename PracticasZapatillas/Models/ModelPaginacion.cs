using System.ComponentModel.DataAnnotations.Schema;

namespace PracticasZapatillas.Models
{
    public class ModelPaginacion
    {
        public int  NumeroRegistrosImagenes { get; set; }

        public Zapatilla Zapatilla { get; set; }

        public Imagen Imagen { get; set; }
    }
}
