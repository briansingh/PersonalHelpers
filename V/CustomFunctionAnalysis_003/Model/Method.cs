namespace CustomFunctionAnalysis_003.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CodeAnalysis.Methods")]
    public partial class Method
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Method()
        {
            MethodCalls = new HashSet<MethodCall>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(512)]
        public string Namespace { get; set; }

        [Required]
        [StringLength(256)]
        public string ClassName { get; set; }

        public int MethodType { get; set; }

        [Required]
        [StringLength(256)]
        public string MethodName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MethodCall> MethodCalls { get; set; }
    }
}
