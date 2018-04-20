using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomFunctionAnalysis_003.Model
{
    [Table("MethodCallArguments", Schema = "CodeAnalysis")]
    public class MethodCallArgument
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(512)]
        public string ArgumentValue { get; set; }

        [Required]
        public int ArgumentIndex { get; set; }

        public virtual MethodCall MethodCall { get; set; }
    }
}
