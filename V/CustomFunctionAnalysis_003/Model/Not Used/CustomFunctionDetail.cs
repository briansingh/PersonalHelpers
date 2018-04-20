using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomFunctionAnalysis_003.Model
{
    public class CustomFunctionDetail
    {
        [Column(Order = 1), Key]
        public int FunctionID { get; set; }

        [Column(Order = 0), Key]
        [StringLength(50)]
        public string DB { get; set; }

        [Required]
        [StringLength(50)]
        public string FunctionName { get; set; }

        [Required]
        [StringLength(8000)]
        public string SourceCode { get; set; }

        public int CRFVersionID { get; set; }

        [Required]
        [StringLength(2)]
        public string Lang { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime? ServerSyncDate { get; set; }

        public int? SourceObjectID { get; set; }

        public DateTime? SourceCopyTime { get; set; }

        [StringLength(50)]
        public string OID { get; set; }

        public DateTime? UpdatedAfterSourceCopy { get; set; }

        public bool IsDraft { get; set; }

        [StringLength(50)]
        public string SourceUrlId { get; set; }
    }
}
