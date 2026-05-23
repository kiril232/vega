using Microsoft.EntityFrameworkCore;

namespace Vega.ProductService.Data;

public static class DbInit
{
    public static void Apply(ProductDbContext db)
    {
        db.Database.ExecuteSqlRaw("""
            CREATE SCHEMA IF NOT EXISTS products;

            CREATE TABLE IF NOT EXISTS products.products (
                "Id"          uuid                        NOT NULL,
                "Name"        character varying(256)      NOT NULL,
                "Description" character varying(4000)     NULL,
                "Category"    character varying(128)      NOT NULL,
                "Price"       numeric(10,2)               NOT NULL,
                "Stock"       integer                     NOT NULL,
                "ImageUrl"    character varying(1024)     NULL,
                "CreatedAt"   timestamp with time zone    NOT NULL,
                CONSTRAINT "PK_products" PRIMARY KEY ("Id")
            );

            CREATE INDEX IF NOT EXISTS "IX_products_Category"
                ON products.products ("Category");
            """);
    }
}
