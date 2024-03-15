using Microsoft.AspNetCore.Mvc;
using PracticasZapatillas.Models;
using PracticasZapatillas.Repositories;

namespace PracticasZapatillas.Controllers
{
    public class ZapatillasController : Controller
    {
        private RepositoryZapatillas repo;

        public ZapatillasController(RepositoryZapatillas repo)
        {
            this.repo = repo;
        }

        
        public async Task<IActionResult> Inicio()
        {
            List<Zapatilla> zapatillas = await this.repo.GetAllZapatillasAsync();
            return View(zapatillas);
        }

        public async Task<IActionResult> ImagenesZapatillasOut
           (int? posicion, int idzapatilla)
        {
            if (posicion == null)
            {
                //POSICION PARA EL EMPLEADO
                posicion = 1;
            }
            ModelPaginacion model = await
                this.repo.GetImagenesZapatillasAsync
                (posicion.Value, idzapatilla);
            Zapatilla zapatilla =
                await this.repo.FindZapatillasAsync(idzapatilla);
            ViewData["ZAPATILLASELECCIONADA"] = zapatilla;
            ViewData["REGISTROS"] = model.NumeroRegistrosImagenes;
            ViewData["ZAPATILLA"] = idzapatilla;
            int siguiente = posicion.Value + 1;
            //DEBEMOS COMPROBAR QUE NO PASAMOS DEL NUMERO DE REGISTROS
            if (siguiente > model.NumeroRegistrosImagenes)
            {
                //EFECTO OPTICO
                siguiente = model.NumeroRegistrosImagenes;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["ULTIMO"] = model.NumeroRegistrosImagenes;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            return PartialView("_ImagenesZapatillasPartial", model.Imagen);
        }
    }
}
