using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace LunchRoletteApi.Migrations
{
    [Migration(1)]
    public class Migration1 : Migration
    {
        public override void Up()
        {
            Create.Table("TestTable")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()
            .WithColumn("Name").AsString(255).NotNullable().WithDefaultValue("Anonymous");

        }

        public override void Down()
        {

            Delete.Table("TestTable");
        }
    }

}
