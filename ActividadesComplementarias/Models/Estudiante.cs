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
    
    public partial class Estudiante
    {
        public Estudiante()
        {
            this.ActividadCursada = new HashSet<ActividadCursada>();
        }
    
        public int idEstudiante { get; set; }
        public string nombreEstudiante { get; set; }
        public int carrera { get; set; }
        public Nullable<double> creditosComplementarios { get; set; }
        public string contraseñaEstudiante { get; set; }
        public string saltContraseña { get; set; }
    
        public virtual ICollection<ActividadCursada> ActividadCursada { get; set; }
        public virtual Carrera Carrera1 { get; set; }
    }
}
