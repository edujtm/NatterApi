namespace Natter.Infrastructure.Migrations;
using FluentMigrator;

[Migration(20230519000818)]
public class AddSpacesAndMessages : Migration
{
    public override void Up()
    {
        Create.Table("spaces")
            .WithColumn("space_id")
                .AsInt32().PrimaryKey()
            .WithColumn("name").AsString(255).NotNullable()
            .WithColumn("owner").AsString(30).NotNullable();

        Create.Sequence("space_id_seq");


        Create.Table("messages")
            .WithColumn("space_id")
                .AsInt32().NotNullable().ForeignKey("spaces", "space_id")
            .WithColumn("msg_id")
                .AsInt32().PrimaryKey()
            .WithColumn("author")
                .AsString(30).NotNullable()
            .WithColumn("msg_time")
                .AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("msg_text")
                .AsString(1024).NotNullable();

        Create.Sequence("msq_id_seq");

        Create.Index("msg_timestamp_idx")
            .OnTable("messages").OnColumn("msg_time");

        Create.Index("space_name_idx")
            .OnTable("spaces").OnColumn("name").Unique();
    }

    public override void Down()
    {
        Delete.Index("space_name_idx");
        Delete.Index("msg_timestamp_idx");

        Delete.Table("spaces");
        Delete.Sequence("space_id_seq");
        Delete.Table("messages");
        Delete.Sequence("msg_id_seq");
    }
}
