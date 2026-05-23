using Microsoft.EntityFrameworkCore;

namespace Vega.OrderService.Data;

public static class DbInit
{
    public static void Apply(OrderDbContext db)
    {
        db.Database.ExecuteSqlRaw("""
            CREATE SCHEMA IF NOT EXISTS orders;

            CREATE TABLE IF NOT EXISTS orders.orders (
                "Id"                   uuid                        NOT NULL,
                "UserId"               uuid                        NOT NULL,
                "Total"                numeric(10,2)               NOT NULL,
                "Currency"             character varying(8)        NOT NULL,
                "Status"               integer                     NOT NULL,
                "FailureReason"        text                        NULL,
                "PaymentTransactionId" uuid                        NULL,
                "CreatedAt"            timestamp with time zone    NOT NULL,
                "UpdatedAt"            timestamp with time zone    NOT NULL,
                CONSTRAINT "PK_orders" PRIMARY KEY ("Id")
            );

            CREATE INDEX IF NOT EXISTS "IX_orders_UserId"
                ON orders.orders ("UserId");

            CREATE TABLE IF NOT EXISTS orders.order_items (
                "Id"          uuid                    NOT NULL,
                "OrderId"     uuid                    NOT NULL,
                "ProductId"   uuid                    NOT NULL,
                "ProductName" character varying(256)  NOT NULL,
                "UnitPrice"   numeric(10,2)           NOT NULL,
                "Quantity"    integer                 NOT NULL,
                CONSTRAINT "PK_order_items" PRIMARY KEY ("Id"),
                CONSTRAINT "FK_order_items_orders" FOREIGN KEY ("OrderId")
                    REFERENCES orders.orders ("Id") ON DELETE CASCADE
            );
            """);
    }
}
