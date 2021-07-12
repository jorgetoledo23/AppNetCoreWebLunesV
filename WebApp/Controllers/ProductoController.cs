using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductoController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductoController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }



        public IActionResult HomeProductos(int Page)
        {

            PaginacionProductoViewModel paginacionPVM = new PaginacionProductoViewModel();

            if (Page == 0)
            {
                paginacionPVM.Pagina = 1;
            }
            else {
                paginacionPVM.Pagina = Page;
            }

            int ProductosPorPagina = 8;
            int CantidadProductos = _context.tblProductos.ToList().Count();
            //int TotalPaginas = 0; // 80/8 = 10    81 / 8 = 10

            if (CantidadProductos % ProductosPorPagina == 0){
                paginacionPVM.CantidadPagina = CantidadProductos / ProductosPorPagina;
            }else {
                paginacionPVM.CantidadPagina = CantidadProductos / ProductosPorPagina + 1;
            }


            //var Productos = _context.tblProductos.Skip(0, 12, 24, 36)
            paginacionPVM.ListaProductos = _context.tblProductos
                .Include(p => p.Categoria)
                .Skip((paginacionPVM.Pagina - 1) * ProductosPorPagina) //Pagina 4
                .Take(ProductosPorPagina)
                .ToList();

            //Lista Producto
            return View(paginacionPVM);
        }


        public IActionResult AddProducto() {
            ViewData["CategoriaId"] = new SelectList(_context.tblCategorias.ToList(), "CategoriaId", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProducto(Producto P)
        {
            if (ModelState.IsValid)
            {
                if (P.ImagenFile == null)
                {
                    P.Imagen = "no-disponible.png";
                }
                else
                {
                    //Guardar la Imagen en el Servidor
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(P.ImagenFile.FileName); //no-disponible
                    string extencion = Path.GetExtension(P.ImagenFile.FileName); //.png
                    P.Imagen = fileName + DateTime.Now.ToString("ddMMyyyyHHmmss") + extencion;
                    //no-disponible05072021193430.png
                    //no-disponible05072021193431.png
                    string rutaArchivo = Path.Combine(wwwRootPath + "/images/" + P.Imagen);
                    using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await P.ImagenFile.CopyToAsync(fileStream);
                    }
                }

                _context.Add(P);
                _context.SaveChanges();
                //return RedirectToAction("HomeCategorias");
                return RedirectToAction(nameof(HomeProductos));
            }
            else
            {
                return View(P);
            }
        }

    }
}
