using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;
using System.Security.Cryptography;


namespace Notepad.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string newRandomPassword = GenerateRandomPassword(16);

            migrationBuilder.InsertData(
               table: "Users",
               columns: new[] { "Username", "Password" },
               values: new object[] { 
                   "admin", 
                   BCrypt.Net.BCrypt.HashPassword(newRandomPassword)
                });
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin"
            );
        }

       
        string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=<>?";

            using (var rng = RandomNumberGenerator.Create())
            {
                var result = new StringBuilder(length);
                var buffer = new byte[4];
                var remainingLength = length;

                while (remainingLength > 0)
                {
                    rng.GetBytes(buffer);
                    for (var i = 0; i < buffer.Length && remainingLength > 0; i++)
                    {
                        var randomChar = validChars[buffer[i] % validChars.Length];
                        result.Append(randomChar);
                        remainingLength--;
                    }
                }

                return result.ToString();
            }
        }
    }
}
