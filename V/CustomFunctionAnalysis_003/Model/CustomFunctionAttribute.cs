namespace CustomFunctionAnalysis_003.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CodeAnalysis.CustomFunctionAttributes")]
    public partial class CustomFunctionAttribute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomFunctionAttribute()
        {
            MethodCalls = new HashSet<MethodCall>();
        }

        [StringLength(50)]
        public string DB { get; set; }

        public int FunctionID { get; set; }

        [Key]
        public int ID { get; set; }

        [StringLength(64)]
        public string SHA256 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MethodCall> MethodCalls { get; set; }
    }
}
