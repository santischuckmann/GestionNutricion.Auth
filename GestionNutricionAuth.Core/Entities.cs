using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionNutricionAuth.Core.Entities
{
    public class User : EntidadBase
    {
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }

    }
}
