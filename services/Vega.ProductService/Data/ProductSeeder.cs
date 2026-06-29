using Vega.ProductService.Domain;

namespace Vega.ProductService.Data;

public static class ProductSeeder
{
    public static void Seed(ProductDbContext db)
    {
        if (db.Products.Any()) return;

        // some sample inventory
        db.Products.AddRange(
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Vega Wormgard 100ml",
                Description = "broad spectrum dewormer for cattle and sheep",
                Category = "Livestock",
                Price = 18.90m,
                Stock = 42,
                ImageUrl = "/img/wormgard.png",
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "AgriCal Mineral Mix 25kg",
                Description = "calcium and trace mineral supplement for dairy herds",
                Category = "Feed Additives",
                Price = 34.50m,
                Stock = 15,
                ImageUrl = "/img/agrical.png",
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "PoultryShield Vaccine 500 doses",
                Description = "newcastle disease vaccine, refrigerate at 2-8C",
                Category = "Vaccines",
                Price = 56.00m,
                Stock = 8,
                ImageUrl = "/img/poultryshield.png",
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "FieldGuard Insecticide 1L",
                Description = "contact insecticide for grain crops",
                Category = "Crop Protection",
                Price = 22.40m,
                Stock = 30,
                ImageUrl = "/img/fieldguard.png",
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "HoofCare Spray 400ml",
                Description = "antiseptic hoof spray for cattle",
                Category = "Livestock",
                Price = 12.75m,
                Stock = 60,
                ImageUrl = "/img/hoofcare.png",
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "GreenGrow NPK 20-10-10 10kg",
                Description = "balanced npk fertilizer for vegetable crops",
                Category = "Fertilizers",
                Price = 19.95m,
                Stock = 80,
                ImageUrl = "/img/greengrow.png",
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "VetSyringe 20ml (pack of 10)",
                Description = "reusable veterinary syringes",
                Category = "Equipment",
                Price = 9.50m,
                Stock = 120,
                ImageUrl = "/img/vetsyringe.png",
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "BeeKeeper Varroa Strips",
                Description = "miticide strips for beehive treatment",
                Category = "Apiculture",
                Price = 28.00m,
                Stock = 22,
                ImageUrl = "/img/varroa.png",
                CreatedAt = DateTime.UtcNow
            }
        );

        db.SaveChanges();
    }
}
