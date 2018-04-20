namespace CustomFunctionAnalysis_003.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CustomFunctionsTargetModel : DbContext
    {
        public CustomFunctionsTargetModel()
            : base("name=CustomFunctionsTargetModel")
        {
        }

        public virtual DbSet<CustomFunctionAttribute> CustomFunctionAttributes { get; set; }
        public virtual DbSet<CustomFunctionDetail> CustomFunctionDetails { get; set; }
        public virtual DbSet<MethodCallArgument> MethodCallArguments { get; set; }
        public virtual DbSet<MethodCall> MethodCalls { get; set; }
        public virtual DbSet<Method> Methods { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomFunctionAttribute>()
                .Property(e => e.DB)
                .IsUnicode(false);

            modelBuilder.Entity<CustomFunctionDetail>()
                .Property(e => e.DB)
                .IsUnicode(false);

            modelBuilder.Entity<CustomFunctionDetail>()
                .Property(e => e.SourceCode)
                .IsUnicode(false);

            modelBuilder.Entity<CustomFunctionDetail>()
                .Property(e => e.Lang)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CustomFunctionDetail>()
                .Property(e => e.OID)
                .IsUnicode(false);

            modelBuilder.Entity<MethodCall>()
                .HasMany(e => e.MethodCallArguments)
                .WithOptional(e => e.MethodCall)
                .HasForeignKey(e => e.MethodCall_ID);
        }
    }
}
