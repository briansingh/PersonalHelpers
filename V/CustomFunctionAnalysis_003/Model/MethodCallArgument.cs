namespace CustomFunctionAnalysis_003.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CodeAnalysis.MethodCallArguments")]
    public partial class MethodCallArgument
    {
        public int ID { get; set; }

        [Required]
        [StringLength(512)]
        public string ArgumentValue { get; set; }

        public int ArgumentIndex { get; set; }

        public int? MethodCall_ID { get; set; }

        public virtual MethodCall MethodCall { get; set; }
    }
}
