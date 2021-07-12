using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class PaginacionProductoViewModel
    {
        public List<Producto> ListaProductos { get; set; }

        public int Pagina { get; set; }

        public int CantidadPagina { get; set; }
    }
}
