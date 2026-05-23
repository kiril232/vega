using Microsoft.EntityFrameworkCore;

namespace Vega.UserService.Data;

public static class DbInit
{
    public static void Apply(UserDbContext db)
    {
        db.Database.ExecuteSqlRaw("""
            CREATE SCHEMA IF NOT EXISTS users;

            CREATE TABLE IF NOT EXISTS users.users (
                "Id"           uuid                        NOT NULL,
                "Email"        character varying(256)      NOT NULL,
                "PasswordHash" text                        NOT NULL,
                "FullName"     character varying(256)      NULL,
                "CreatedAt"    timestamp with time zone    NOT NULL,
                CONSTRAINT "PK_users" PRIMARY KEY ("Id")
            );

            CREATE UNIQUE INDEX IF NOT EXISTS "IX_users_Email"
                ON users.users ("Email");
            """);
    }
}
