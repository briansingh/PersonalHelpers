//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CustomFunctionsWebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MethodCallSummary
    {
        public int ID { get; set; }
        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public Nullable<int> MethodCallCount { get; set; }
    }
}
