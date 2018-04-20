using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPA003.Models
{
    public partial class Database
    {
        [Key]
        public string DB { get; set; }
        public Nullable<int> FunctionCount { get; set; }
    }
}