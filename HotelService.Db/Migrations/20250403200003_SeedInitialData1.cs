using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelService.Db.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DO $$
            BEGIN
                IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Users') THEN
                    INSERT INTO ""Users"" (""UserId"", ""Username"", ""Email"", ""PasswordHash"", ""Role"", ""RegistrationDate"")
                    VALUES 
                    (1, 'admin', 'admin@hotelservice.com', '$2a$11$K7Zg2J3sJ8Q1VZ7wYbLW9eWQN9d7tY1JZ8XvY6WXvY1JZ8XvY6WXvY', 'Admin', NOW()),
                    (2, 'host1', 'host1@hotelservice.com', '$2a$11$K7Zg2J3sJ8Q1VZ7wYbLW9eWQN9d7tY1JZ8XvY6WXvY1JZ8XvY6WXvY', 'Host', NOW()),
                    (3, 'host2', 'host2@hotelservice.com', '$2a$11$K7Zg2J3sJ8Q1VZ7wYbLW9eWQN9d7tY1JZ8XvY6WXvY1JZ8XvY6WXvY', 'Host', NOW()),
                    (4, 'customer1', 'customer1@example.com', '$2a$11$K7Zg2J3sJ8Q1VZ7wYbLW9eWQN9d7tY1JZ8XvY6WXvY1JZ8XvY6WXvY', 'Customer', NOW()),
                    (5, 'customer2', 'customer2@example.com', '$2a$11$K7Zg2J3sJ8Q1VZ7wYbLW9eWQN9d7tY1JZ8XvY6WXvY1JZ8XvY6WXvY', 'Customer', NOW());
                END IF;
            END $$;
        ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
