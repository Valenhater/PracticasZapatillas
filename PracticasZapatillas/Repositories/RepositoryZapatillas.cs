using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PracticasZapatillas.Data;
using PracticasZapatillas.Models;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PracticasZapatillas.Repositories
{
    #region PROCEDURES

    //    create procedure SP_GRUPO_IMAGENES_ZAPATILLA_OUT
    //(@posicion int, @idproducto int
    //, @registros int out)
    //as
    //select @registros = count(IDIMAGEN) from imageneszapaspractica
    //where IDPRODUCTO = @idproducto
    //select IDIMAGEN, IDPRODUCTO, IMAGEN from
    //    (select cast(
    //    ROW_NUMBER() OVER (ORDER BY IDIMAGEN) as int) AS POSICION
    //    , IDIMAGEN, IDPRODUCTO, IMAGEN
    //    from imageneszapaspractica
    //    where IDPRODUCTO = @idproducto) as QUERY
    //    where QUERY.POSICION >= @posicion and QUERY.POSICION<(@posicion)
    //go

    #endregion
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;
        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }
        public async Task<List<Zapatilla>> GetAllZapatillasAsync()
        {
            return await this.context.Zapatillas.ToListAsync();
        }
        public async Task<Zapatilla> FindZapatillasAsync(int id)
        {
            return await this.context.Zapatillas
                .FirstOrDefaultAsync(x => x.IdProducto == id);
        }

        public async Task<ModelPaginacion> GetImagenesZapatillasAsync (int posicion, int idzapatilla)
        {
            string sql = "SP_GRUPO_IMAGENES_ZAPATILLA_OUT @posicion, @idproducto, @registros out";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamIdzapatilla =
                new SqlParameter("@idproducto", idzapatilla);
            SqlParameter pamRegistros = new SqlParameter("@registros", -1);
            pamRegistros.Direction = ParameterDirection.Output;
            var consulta =
                this.context.Imagenes.FromSqlRaw
                (sql, pamPosicion, pamIdzapatilla, pamRegistros);
            //PRIMERO DEBEMOS EJECUTAR LA CONSULTA PARA PODER RECUPERAR 
            //LOS PARAMETROS DE SALIDA
            var datos = await consulta.ToListAsync();
            Imagen imagen = datos.FirstOrDefault();
            int registros = (int)pamRegistros.Value;
            return new ModelPaginacion
            {
                NumeroRegistrosImagenes = registros,
                Imagen = imagen
            };
        }
    }
}
