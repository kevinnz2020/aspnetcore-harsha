using Microsoft.EntityFrameworkCore.Migrations;
using System.Globalization;

#nullable disable

namespace Entities.Migrations
{
    public partial class GetPersons_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_getAllPersons = @"
                CREATE PROCEDURE [dbo].[GetAllpersons]
                AS BEGIN
                    SELECT PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters From [dbo].[Persons]
                END

            ";
            migrationBuilder.Sql(sp_getAllPersons); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Undo 
            string sp_GetAllPersons = @"DROP PROCEDURE [dbo].[GetAllPersons]";
            migrationBuilder.Sql(sp_GetAllPersons);

        }
    }
}
