namespace CustomFunctionAnalysis_003.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CustomFunctionsTargetModel : DbContext
    {
        public CustomFunctionsTargetModel()
            : base("name=CustomFunctionsTarget")
        {
        }

        public virtual DbSet<CustomFunctionDetail> CustomFunctions { get; set; }
        public virtual DbSet<CustomFunctionAttribute> CustomFunctionAttributes { get; set; }
        public virtual DbSet<Method> Methods { get; set; }
        public virtual DbSet<MethodCall> MethodCalls { get; set; }
        public virtual DbSet<MethodCallArgument> MethodCallArguments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CustomFunction>()
            //    .Property(e => e.DB)
            //    .IsUnicode(false);

            modelBuilder.Entity<CustomFunction>()
                .Property(e => e.SourceCode)
                .IsUnicode(false);

            modelBuilder.Entity<CustomFunction>()
                .Property(e => e.Lang)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CustomFunction>()
                .Property(e => e.OID)
                .IsUnicode(false);
        }
    }
}
