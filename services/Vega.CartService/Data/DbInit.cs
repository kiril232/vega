using Microsoft.EntityFrameworkCore;

namespace Vega.CartService.Data;

public static class DbInit
{
    public static void Apply(CartDbContext db)
    {
        db.Database.ExecuteSqlRaw("""
            CREATE SCHEMA IF NOT EXISTS carts;

            CREATE TABLE IF NOT EXISTS carts.carts (
                "Id"        uuid                        NOT NULL,
                "UserId"    uuid                        NOT NULL,
                "CreatedAt" timestamp with time zone    NOT NULL,
                "UpdatedAt" timestamp with time zone    NOT NULL,
                CONSTRAINT "PK_carts" PRIMARY KEY ("Id")
            );

            CREATE UNIQUE INDEX IF NOT EXISTS "IX_carts_UserId"
                ON carts.carts ("UserId");

            CREATE TABLE IF NOT EXISTS carts.cart_items (
                "Id"            uuid                        NOT NULL,
                "CartId"        uuid                        NOT NULL,
                "ProductId"     uuid                        NOT NULL,
                "ProductName"   character varying(256)      NOT NULL,
                "ImageUrl"      character varying(1024)     NULL,
                "PriceSnapshot" numeric(10,2)               NOT NULL,
                "Quantity"      integer                     NOT NULL,
                CONSTRAINT "PK_cart_items" PRIMARY KEY ("Id"),
                CONSTRAINT "FK_cart_items_carts" FOREIGN KEY ("CartId")
                    REFERENCES carts.carts ("Id") ON DELETE CASCADE
            );

            CREATE UNIQUE INDEX IF NOT EXISTS "IX_cart_items_CartId_ProductId"
                ON carts.cart_items ("CartId", "ProductId");
            """);
    }
}
