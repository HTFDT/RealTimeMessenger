using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Triggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE FUNCTION incr() 
    RETURNS trigger 
    LANGUAGE plpgsql AS
$incr$
BEGIN
    UPDATE ""Groups"" SET ""LastMessageNum"" = ""LastMessageNum"" + 1 
        WHERE ""Id"" = NEW.""GroupId"";
    RETURN NULL;
END 
$incr$;");
            
            migrationBuilder.Sql(@"
CREATE FUNCTION set_message_num() 
	RETURNS trigger 
	LANGUAGE plpgsql AS
$set_message_num$
BEGIN
	NEW.""GroupNumber"" := (SELECT ""LastMessageNum"" FROM ""Groups"" WHERE ""Id"" = NEW.""GroupId"");
	RETURN NEW;
END 
$set_message_num$;");

            migrationBuilder.Sql(@"
CREATE TRIGGER set_new_message_num
BEFORE INSERT ON ""Messages""
FOR EACH ROW
EXECUTE FUNCTION set_message_num();");
            
            migrationBuilder.Sql(@"
CREATE TRIGGER incr_last_message_number
AFTER INSERT ON ""Messages""
FOR EACH ROW
EXECUTE FUNCTION incr();");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS set_new_message_num ON ""Messages"";");
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS incr_last_message_number ON ""Messages"";");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS incr();");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS set_message_num();");
        }
    }
}
