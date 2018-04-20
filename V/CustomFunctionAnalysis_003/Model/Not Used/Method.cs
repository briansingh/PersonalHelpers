using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomFunctionAnalysis_003.Model
{
    public enum MethodType
    {
        System,
        Metadata,
        Unknown
    }

    [Table("Methods", Schema = "CodeAnalysis")]
    public class Method
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(512)]
        public string Namespace { get; set; }

        [Required]
        [StringLength(256)]
        public string ClassName { get; set; }

        [Required]
        [StringLength(256)]
        public string MethodName { get; set; }

        [Required]
        public MethodType MethodType { get; set; }

        public virtual List<MethodCall> MethodCalls { get; set; }

        public Method()
        {
            MethodType = MethodType.Unknown;
        }
    }

}
