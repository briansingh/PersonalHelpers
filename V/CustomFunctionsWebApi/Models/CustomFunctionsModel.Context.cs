﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Registry1_100Entities : DbContext
    {
        public Registry1_100Entities()
            : base("name=Registry1_100Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CustomFunctionAttribute> CustomFunctionAttributes { get; set; }
        public virtual DbSet<MethodCallArgument> MethodCallArguments { get; set; }
        public virtual DbSet<MethodCall> MethodCalls { get; set; }
        public virtual DbSet<Method> Methods { get; set; }
        public virtual DbSet<CustomFunction> CustomFunctions { get; set; }
        public virtual DbSet<DatabaseName> DatabaseNames { get; set; }
        public virtual DbSet<CustomFunctionSummary> CustomFunctionSummaries { get; set; }
        public virtual DbSet<MethodCallSummary> MethodCallSummaries { get; set; }
        public virtual DbSet<Namespace> Namespaces { get; set; }
    }
}
