namespace CustomFunctionAnalysis_003.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CodeAnalysis.MethodCalls")]
    public partial class MethodCall
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MethodCall()
        {
            MethodCallArguments = new HashSet<MethodCallArgument>();
        }

        public int ID { get; set; }

        public int CustomFunctionAttributeID { get; set; }

        public int MethodID { get; set; }

        public int CodeStart { get; set; }

        public int CodeEnd { get; set; }

        public virtual CustomFunctionAttribute CustomFunctionAttribute { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MethodCallArgument> MethodCallArguments { get; set; }

        public virtual Method Method { get; set; }
    }
}
