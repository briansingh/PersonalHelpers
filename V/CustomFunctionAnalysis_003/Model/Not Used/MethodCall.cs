using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomFunctionAnalysis_003.Model
{
    [Table("MethodCalls", Schema = "CodeAnalysis")]
    public class MethodCall
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int CustomFunctionAttributeID { get; set; }

        [Required]
        public int MethodID { get; set; }

        [Required]
        public int CodeStart { get; set; }

        [Required]
        public int CodeEnd { get; set; }

        public virtual CustomFunctionAttribute CustomFunctionAttribute { get; set; }
        public virtual Method Method { get; set; }
        public virtual List<MethodCallArgument> Arguments { get; set; }
    }
}
