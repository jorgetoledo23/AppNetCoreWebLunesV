using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CategoriaController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult HomeCategorias()
        {
            //List<Categoria> listaCategorias = _context.tblCategorias.ToList();
            return View(_context.tblCategorias.ToList());
        }

        public IActionResult addCategorias() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addCategorias(Categoria C) {
            //Guardar la Categoria
            if (ModelState.IsValid)
            {
                if (C.ImagenFile == null)
                {
                    C.Imagen = "no-disponible.png";
                }
                else
                {
                    //Guardar la Imagen en el Servidor
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(C.ImagenFile.FileName); //no-disponible
                    string extencion = Path.GetExtension(C.ImagenFile.FileName); //.png
                    C.Imagen = fileName + DateTime.Now.ToString("ddMMyyyyHHmmss") + extencion;
                    //no-disponible05072021193430.png
                    //no-disponible05072021193431.png
                    string rutaArchivo = Path.Combine(wwwRootPath + "/images/" + C.Imagen);
                    using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await C.ImagenFile.CopyToAsync(fileStream);
                    }
                }

                _context.Add(C);
                _context.SaveChanges();
                //return RedirectToAction("HomeCategorias");
                return RedirectToAction(nameof(HomeCategorias));
            }
            else
            {
                return View(C);
            }
        }

        [HttpGet]
        public IActionResult editCategoria(int CategoriaId) {
            var C = _context.tblCategorias.Where(c => c.CategoriaId.Equals(CategoriaId)).FirstOrDefault();
            if (C != null)
            {
                return View(C);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult editCategoria(Categoria C)
        {
            if (ModelState.IsValid)
            {
                var CategoriaEditada = _context.tblCategorias.Where(c => c.CategoriaId.Equals(C.CategoriaId)).FirstOrDefault();
                CategoriaEditada.Nombre = C.Nombre;
                CategoriaEditada.Descripcion = C.Descripcion;
                _context.SaveChanges();
                return RedirectToAction(nameof(HomeCategorias));
            }
            else
            {
                return View(C);
            } 
        }

        //TODO: Generar Delete (1 Punto para la 3ra Evaluacion!)

        [HttpGet]
        public IActionResult deleteCategoria(int CategoriaId)
        {
            var C = _context.tblCategorias.Where(c => c.CategoriaId.Equals(CategoriaId)).FirstOrDefault();
            if (C != null)
            {
                return View(C);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult deleteCategoria(Categoria C)
        {
            if (ModelState.IsValid)
            {
                var CategoriaEliminada = _context.tblCategorias.Where(c => c.CategoriaId.Equals(C.CategoriaId)).FirstOrDefault();
                _context.Entry(CategoriaEliminada).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                _context.SaveChanges();
                return RedirectToAction(nameof(HomeCategorias));
            }
            else
            {
                return View(C);
            }
        }



    }
}
