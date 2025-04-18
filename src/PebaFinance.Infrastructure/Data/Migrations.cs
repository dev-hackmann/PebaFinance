using FluentMigrator;

namespace PebaFinance.Migrations;

[Migration(2025041801)]
public class CreateInitialSchema : Migration
{
    public override void Up()
    {
        // Tabela 'user'
        Create.Table("user")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("email").AsString(255).NotNullable()
            .WithColumn("password_hash").AsString(255).NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);

        Create.UniqueConstraint("UQ_user_email").OnTable("user").Column("email");

        // Tabela 'expense'
        Create.Table("expense")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("description").AsString(200).NotNullable()
            .WithColumn("value").AsDecimal(18, 2).NotNullable()
            .WithColumn("date").AsDate().NotNullable()
            .WithColumn("category").AsByte().NotNullable()
            .WithColumn("user_id").AsInt32().NotNullable();

        Create.Index("IX_Expense_User").OnTable("expense").OnColumn("user_id");

        Create.ForeignKey("FK_Expense_User")
            .FromTable("expense").ForeignColumn("user_id")
            .ToTable("user").PrimaryColumn("id");

        // Tabela 'income'
        Create.Table("income")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("description").AsString(200).NotNullable()
            .WithColumn("value").AsDecimal(18, 2).NotNullable()
            .WithColumn("date").AsDate().NotNullable()
            .WithColumn("user_id").AsInt32().NotNullable();

        Create.Index("IX_Income_User").OnTable("income").OnColumn("user_id");

        Create.ForeignKey("FK_Income_User")
            .FromTable("income").ForeignColumn("user_id")
            .ToTable("user").PrimaryColumn("id");

        // View 'financial_summary'
        Execute.Sql(@"
            DROP VIEW IF EXISTS financial_summary;
            CREATE VIEW financial_summary AS
            SELECT 'expense' AS type, value AS Value, date AS Date FROM expense
            UNION ALL
            SELECT 'income' AS type, value AS Value, date AS Date FROM income;
        ");
    }

    public override void Down()
    {
        Execute.Sql("DROP VIEW IF EXISTS financial_summary;");
        Delete.ForeignKey("FK_Income_User").OnTable("income");
        Delete.ForeignKey("FK_Expense_User").OnTable("expense");
        Delete.Index("IX_Income_User").OnTable("income");
        Delete.Index("IX_Expense_User").OnTable("expense");
        Delete.Table("income");
        Delete.Table("expense");
        Delete.UniqueConstraint("UQ_user_email").FromTable("user");
        Delete.Table("user");
    }
}
