namespace CustomFunctionAnalysis_003.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CustomFunctionsSourceModel : DbContext
    {
        public CustomFunctionsSourceModel()
            : base("name=CustomFunctionsSource")
        {
        }

        public CustomFunctionsSourceModel(string sourceConnectionString) 
            : base(sourceConnectionString)
        {

        }

        public virtual DbSet<CustomFunction> CustomFunctions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomFunction>()
                .Property(e => e.FunctionID);

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

            modelBuilder.Entity<CustomFunction>()
                .Property(e => e.SourceUrlId)
                .IsUnicode(false);
        }
    }
}
