//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ActividadesComplementarias.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Carrera
    {
        public Carrera()
        {
            this.Estudiante = new HashSet<Estudiante>();
        }
    
        public int idCarrera { get; set; }
        public string nombreCarrera { get; set; }
        public int departamento { get; set; }
    
        public virtual Departamento Departamento1 { get; set; }
        public virtual ICollection<Estudiante> Estudiante { get; set; }
    }
}
