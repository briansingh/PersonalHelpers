using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomFunctionAnalysis_003.Model
{
    [Table("CustomFunctionAttributes", Schema="CodeAnalysis")]
    public class CustomFunctionAttribute
    {
        [Key]
        public int ID { get; set; }

        [StringLength(64)]
        public string SHA256 { get; set; }

        [ForeignKey("CustomFunction"), Column(Order=0)]
        [StringLength(50)]
        public string DB { get; set; }

        [ForeignKey("CustomFunction"), Column(Order = 1)]
        public int FunctionID { get; set; }

        public virtual CustomFunctionDetail CustomFunction { get; set; }

        public virtual List<MethodCall> MethodCalls { get; set; }
    }
}
