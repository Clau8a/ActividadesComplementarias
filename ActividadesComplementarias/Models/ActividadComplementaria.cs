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
    
    public partial class ActividadComplementaria
    {
        public ActividadComplementaria()
        {
            this.ActividadCursada = new HashSet<ActividadCursada>();
        }
    
        public int idActividadComplementaria { get; set; }
        public string nombreActComplementaria { get; set; }
        public string descripcion { get; set; }
        public int carrera { get; set; }
        public string modalidad { get; set; }
        public int noCreditos { get; set; }
    
        public virtual Carrera Carrera1 { get; set; }
        public virtual ICollection<ActividadCursada> ActividadCursada { get; set; }
    }
}
