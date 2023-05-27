namespace Natter.Infrastructure.Migrations;
using FluentMigrator;

[Migration(20230519000818)]
public class AddSpacesAndMessages : Migration
{
    public override void Up()
    {
        this.Create.Table("spaces")
            .WithColumn("space_id")
                .AsInt32().PrimaryKey()
            .WithColumn("name").AsString(255).NotNullable()
            .WithColumn("owner").AsString(30).NotNullable();

        this.Create.Sequence("space_id_seq");


        this.Create.Table("messages")
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

        this.Create.Sequence("msq_id_seq");

        this.Create.Index("msg_timestamp_idx")
            .OnTable("messages").OnColumn("msg_time");

        this.Create.Index("space_name_idx")
            .OnTable("spaces").OnColumn("name").Unique();
    }

    public override void Down()
    {
        this.Delete.Index("space_name_idx");
        this.Delete.Index("msg_timestamp_idx");

        this.Delete.Table("spaces");
        this.Delete.Sequence("space_id_seq");
        this.Delete.Table("messages");
        this.Delete.Sequence("msg_id_seq");
    }
}
