using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotTowel2.Models
{
    public class Database
    {
        public string Name { get; set; }
        public int FunctionCount { get; set; }

        public Database()
        {
            Name = "Undefined";
            FunctionCount = -1;
        }
    }
}