//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CustomFunctionWebApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MethodCall
    {
        public MethodCall()
        {
            this.MethodCallArguments = new HashSet<MethodCallArgument>();
        }
    
        public int ID { get; set; }
        public int CustomFunctionAttributeID { get; set; }
        public int MethodID { get; set; }
        public int CodeStart { get; set; }
        public int CodeEnd { get; set; }
    
        public virtual CustomFunctionAttribute CustomFunctionAttribute { get; set; }
        public virtual ICollection<MethodCallArgument> MethodCallArguments { get; set; }
        public virtual Method Method { get; set; }
    }
}
