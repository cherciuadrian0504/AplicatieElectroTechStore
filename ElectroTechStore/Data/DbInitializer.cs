using ElectroTechStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectroTechStore.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Se asigură că baza de date are migrările aplicate
            await context.Database.MigrateAsync();

            // 1. Creare Roluri: Admin și Client
            string[] roleNames = { "Admin", "Client" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Creare cont implicit de Admin
            var adminEmail = "admin@electrotech.ro";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // 3. Creare cont implicit de Client
            var clientEmail = "client@electrotech.ro";
            var clientUser = await userManager.FindByEmailAsync(clientEmail);
            if (clientUser == null)
            {
                clientUser = new IdentityUser
                {
                    UserName = clientEmail,
                    Email = clientEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(clientUser, "Client123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(clientUser, "Client");
                }
            }

            // 4. Seeding Categorii și Produse inițiale (dacă tabelul e gol)
            if (!context.Categories.Any())
            {
                var catEmbedded = new Category
                {
                    Name = "Plăci de Dezvoltare & Embedded",
                    Description = "Microcontrolere, plăci Arduino, Raspberry Pi și FPGA."
                };
                var catPcParts = new Category
                {
                    Name = "Componente PC",
                    Description = "Procesoare, plăci video, memorii RAM și stocare SSD."
                };
                var catSensors = new Category
                {
                    Name = "Senzori & Electronică",
                    Description = "Senzori ultrasonici, module Wi-Fi, rezistoare și breadboard-uri."
                };

                context.Categories.AddRange(catEmbedded, catPcParts, catSensors);
                await context.SaveChangesAsync();

                context.Products.AddRange(
                    new Product
                    {
                        Name = "Placă de dezvoltare Arduino Uno R3 ATmega328P",
                        Description = "Placă ideală pentru prototipare rapidă, proiecte de automatizare și sisteme embedded.",
                        Specifications = "Microcontroler: ATmega328P | Tensiune operare: 5V | Pini Digitali I/O: 14 (6 PWM) | Pini Analogici: 6 | Memorie Flash: 32 KB",
                        Price = 115.50m,
                        Stock = 25,
                        Supplier = "Arduino Official Store",
                        DeliveryMethod = "Easybox / Curier Rapid",
                        CategoryId = catEmbedded.Id,
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/3/38/Arduino_Uno_-_R3.jpg"
                    },
                    new Product
                    {
                        Name = "Procesor AMD Ryzen 7 5800X, 3.8GHz, 36MB, Socket AM4",
                        Description = "Procesor cu 8 nuclee și 16 thread-uri pentru stații de lucru, compilare paralelă și gaming.",
                        Specifications = "Nuclee: 8 | Threads: 16 | Frecvență bază: 3.8 GHz | Frecvență Boost: 4.7 GHz | Cache L3: 32 MB | TDP: 105W",
                        Price = 949.99m,
                        Stock = 8,
                        Supplier = "AMD Distribution RO",
                        DeliveryMethod = "Curier Rapid",
                        CategoryId = catPcParts.Id,
                        ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/7/7c/AMD_Ryzen_7_3700X_processor_on_white_background.jpg"
                    },
                    new Product
                    {
                        Name = "Kit Senzor Ultrasonic HC-SR04 + Breadboard 830 puncte",
                        Description = "Pachet pentru măsurarea distanței și prototipare circuite fără lipire.",
                        Specifications = "Tensiune: 5V DC | Curent repaus: sub 2mA | Unghi detecție: 15 grade | Distanță măsurare: 2cm - 400cm",
                        Price = 29.00m,
                        Stock = 50,
                        Supplier = "RoboFun Electronics",
                        DeliveryMethod = "Easybox / Ridicare Personală",
                        CategoryId = catSensors.Id
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}