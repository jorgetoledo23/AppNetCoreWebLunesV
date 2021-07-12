using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string Imagen { get; set; } // no-disponible.png

        [NotMapped]
        [Display(Name = "Imagen")]
        public IFormFile ImagenFile { get; set; }



    }
}
