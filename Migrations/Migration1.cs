﻿using System;
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
            Create.Table("Location")
            .WithColumn("locationId").AsInt32().NotNullable().PrimaryKey()
            .WithColumn("displayName").AsString(255)

            .WithColumn("description").AsString(255);
        }

        public override void Down()
        {

            Delete.Table("Location");
        }
    }

    [Migration(2)]
    public class Migration2 : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Location").Row(new
            {
                locationId = 1,
                displayName = "JJ Gumbo's",
                description = "Mushy rice and left-over meat"
            });
            Insert.IntoTable("Location").Row(new
            {
                locationId = 2,
                displayName = "JJ Greek",
                description = "Stinky coke machine"
            });
        }

        public override void Down()
        {
            Delete.FromTable("Location").Row(new { locationId = 1 });
            Delete.FromTable("Location").Row(new { locationId = 2 });
        }
    }

    [Migration(5)]
    public class MigrationHappy : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Location").Row(new
            {
                locationId = 3,
                displayName = "JJ Pizza",
                description = "its'a meh"
            });
        }

        public override void Down()
        {
            Delete.FromTable("Location").Row(new { locationId = 3 });
        }

        [Migration(4)]
        public class MigrationNotHappy : Migration
        {
            public override void Up()
            {
                Insert.IntoTable("Location").Row(new
                {
                    locationId = 4,
                    displayName = "JJ BBQ",
                    description = "Great potatoe"
                });
            }

            public override void Down()
            {
                Delete.FromTable("Location").Row(new { locationId = 4 });
            }
        }
    }

    [Migration(6)]
    public class Migration6 : Migration
    {
        public override void Up()
        {
            Create.Table("TodaysLunch")
                .WithColumn("todaysLunchId").AsInt32().NotNullable().PrimaryKey()
                .WithColumn("lunchDate").AsDate().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("displayName").AsString(255)
                .WithColumn("upCount").AsInt32()
                .WithColumn("downCount").AsInt32();
        }
        public override void Down()
        {
            Delete.Table("TodaysLunch");
        }
    }
}
