using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AcercaTest.Models
{
    public class VehicleModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NumeroPedido { get; set; }

        [Required]
        public string Bastidor { get; set; }

        [Required]
        public string Modelo { get; set; }

        [Required]
        public string Matricula { get; set; }

        [Required]
        public DateTime FechaEntrega { get; set; }

        public Boolean Validate()
        {
            if (NumeroPedido <= 0) return false;
            if (Bastidor == "" || Bastidor == null) return false;
            if (Modelo == "" || Modelo == null) return false;
            if (Matricula == "" || Matricula == null) return false;
            if (FechaEntrega == null || FechaEntrega < new DateTime(2001, 1, 1)) return false;

            return true;
        }
    }

}