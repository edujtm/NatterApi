namespace Natter.Migrations.Migrations;
using FluentMigrator;

[Migration(20230824225820)]
public class AddUserTable : Migration
{
    public override void Up() => Create.Table("natter_users")
            .WithColumn("id").AsString(450).NotNullable().PrimaryKey()
            .WithColumn("username").AsString(256).Nullable()
            .WithColumn("normalized_username").AsString(256).Nullable()
            .WithColumn("password_hash").AsString(int.MaxValue).Nullable();

    public override void Down() => Delete.Table("natter_users");
}
