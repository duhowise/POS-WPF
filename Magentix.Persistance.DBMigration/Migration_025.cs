using FluentMigrator;

namespace Magentix.Persistance.DBMigration
{
    [Migration(25)]
    public class Migration_025 : Migration
    {
        public override void Up()
        {
            base.Create.Column("SortAlphabetically").OnTable("ScreenMenuCategories").AsBoolean().WithDefaultValue(false);
        }

        public override void Down()
        {

        }
    }
}