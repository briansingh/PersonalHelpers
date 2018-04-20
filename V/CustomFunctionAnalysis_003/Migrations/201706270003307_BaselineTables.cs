namespace CustomFunctionAnalysis_003.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaselineTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "CodeAnalysis.CustomFunctionAttributes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DB = c.String(maxLength: 50, unicode: false),
                        FunctionID = c.Int(nullable: false),
                        SHA256 = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "CodeAnalysis.MethodCalls",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomFunctionAttributeID = c.Int(nullable: false),
                        MethodID = c.Int(nullable: false),
                        CodeStart = c.Int(nullable: false),
                        CodeEnd = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("CodeAnalysis.CustomFunctionAttributes", t => t.CustomFunctionAttributeID, cascadeDelete: true)
                .ForeignKey("CodeAnalysis.Methods", t => t.MethodID, cascadeDelete: true)
                .Index(t => t.CustomFunctionAttributeID)
                .Index(t => t.MethodID);
            
            CreateTable(
                "CodeAnalysis.Methods",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Namespace = c.String(nullable: false, maxLength: 512),
                        ClassName = c.String(nullable: false, maxLength: 256),
                        MethodType = c.Int(nullable: false),
                        MethodName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "CodeAnalysis.MethodCallArguments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArgumentValue = c.String(nullable: false, maxLength: 512),
                        ArgumentIndex = c.Int(nullable: false),
                        MethodCall_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("CodeAnalysis.MethodCalls", t => t.MethodCall_ID)
                .Index(t => t.MethodCall_ID);
            
            CreateTable(
                "CodeAnalysis.CustomFunctionDetails",
                c => new
                    {
                        DB = c.String(nullable: false, maxLength: 50, unicode: false),
                        FunctionID = c.Int(nullable: false),
                        FunctionName = c.String(nullable: false, maxLength: 50),
                        SourceCode = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CRFVersionID = c.Int(nullable: false),
                        Lang = c.String(nullable: false, maxLength: 2, fixedLength: true, unicode: false),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        ServerSyncDate = c.DateTime(),
                        SourceObjectID = c.Int(),
                        SourceCopyTime = c.DateTime(),
                        OID = c.String(maxLength: 50, unicode: false),
                        UpdatedAfterSourceCopy = c.DateTime(),
                        IsDraft = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.DB, t.FunctionID });
            
        }
        
        public override void Down()
        {
            DropForeignKey("CodeAnalysis.MethodCallArguments", "MethodCall_ID", "CodeAnalysis.MethodCalls");
            DropForeignKey("CodeAnalysis.MethodCalls", "MethodID", "CodeAnalysis.Methods");
            DropForeignKey("CodeAnalysis.MethodCalls", "CustomFunctionAttributeID", "CodeAnalysis.CustomFunctionAttributes");
            DropIndex("CodeAnalysis.MethodCallArguments", new[] { "MethodCall_ID" });
            DropIndex("CodeAnalysis.MethodCalls", new[] { "MethodID" });
            DropIndex("CodeAnalysis.MethodCalls", new[] { "CustomFunctionAttributeID" });
            DropTable("CodeAnalysis.CustomFunctionDetails");
            DropTable("CodeAnalysis.MethodCallArguments");
            DropTable("CodeAnalysis.Methods");
            DropTable("CodeAnalysis.MethodCalls");
            DropTable("CodeAnalysis.CustomFunctionAttributes");
        }
    }
}
