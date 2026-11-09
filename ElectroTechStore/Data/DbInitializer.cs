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

            // 4. Asigurare existență Categorii
            if (!await context.Categories.AnyAsync())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        Name = "Plăci de Dezvoltare & Embedded",
                        Description = "Microcontrolere, plăci Arduino, Raspberry Pi, ESP32 și arhitecturi FPGA."
                    },
                    new Category
                    {
                        Name = "Componente PC",
                        Description = "Procesoare, plăci video, memorii RAM, plăci de bază și stocare SSD."
                    },
                    new Category
                    {
                        Name = "Senzori & Electronică",
                        Description = "Senzori ultrasonici, module relee, servomotoare, afișaje OLED și componente pasive."
                    }
                );
                await context.SaveChangesAsync();
            }

            var catEmbedded = await context.Categories.FirstAsync(c => c.Name.Contains("Embedded"));
            var catPcParts = await context.Categories.FirstAsync(c => c.Name.Contains("Componente PC"));
            var catSensors = await context.Categories.FirstAsync(c => c.Name.Contains("Senzori"));

            // 5. Lista completă de 18 produse (6 produse per categorie)
            var allProducts = new List<Product>
            {
                // ==================== CATEGORIA 1: PLĂCI DE DEZVOLTARE & EMBEDDED (6 PRODUSE) ====================
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
                    Name = "Single Board Computer Raspberry Pi 5, 8GB RAM",
                    Description = "Mini-computer de înaltă performanță cu procesor quad-core ARM Cortex-A76, ideal pentru procesare video, robotică și servere edge.",
                    Specifications = "Procesor: Broadcom BCM2712 2.4GHz Quad-Core | RAM: 8GB LPDDR4X | Conectivitate: Dual-band Wi-Fi 802.11ac, Bluetooth 5.0, Gigabit Ethernet | Porturi: 2x micro-HDMI (4K 60Hz), 2x USB 3.0, PCIe 2.0",
                    Price = 469.99m,
                    Stock = 14,
                    Supplier = "Raspberry Pi Foundation",
                    DeliveryMethod = "Curier Rapid (24h)",
                    CategoryId = catEmbedded.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/c51a4a?text=Raspberry+Pi+5+8GB"
                },
                new Product
                {
                    Name = "Placă de dezvoltare ESP32-WROOM-32 Wi-Fi + Bluetooth Dual-Core",
                    Description = "Modul IoT cu consum redus de energie, Wi-Fi și Bluetooth BLE integrate, excelent pentru automatizări smart home și telemetrie.",
                    Specifications = "Arhitectură: Xtensa Dual-Core 32-bit LX6 | Frecvență: 240 MHz | Memorie Flash: 4 MB | SRAM: 520 KB | Tensiune logică: 3.3V | Interfețe: UART, SPI, I2C, PWM, ADC",
                    Price = 39.90m,
                    Stock = 42,
                    Supplier = "Espressif Systems",
                    DeliveryMethod = "Easybox / Curier Rapid",
                    CategoryId = catEmbedded.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/005eb8?text=ESP32-WROOM-32"
                },
                new Product
                {
                    Name = "Placă STM32 Nucleo-64 cu MCU STM32F401RE ARM Cortex-M4",
                    Description = "Placă profesională de evaluare și dezvoltare pe 32 de biți cu debugger ST-LINK/V2-1 integrat și conectori compatibili Arduino Uno V3.",
                    Specifications = "Microcontroler: STM32F401RET6 ARM Cortex-M4 cu FPU | Frecvență CPU: 84 MHz | Flash: 512 KB | SRAM: 96 KB | Debugger integrat: ST-LINK/V2-1 | Pini extensie: ST Morpho + Arduino",
                    Price = 98.00m,
                    Stock = 19,
                    Supplier = "STMicroelectronics",
                    DeliveryMethod = "Easybox / Curier Rapid",
                    CategoryId = catEmbedded.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/03234b?text=STM32+Nucleo-64"
                },
                new Product
                {
                    Name = "Placă de dezvoltare FPGA Terasic DE10-Lite (Intel MAX 10)",
                    Description = "Platformă hardware pentru proiectarea arhitecturilor digitale în VHDL/Verilog, dotată cu afișaje 7-segmente, switch-uri, LED-uri și accelerometru.",
                    Specifications = "FPGA: Intel MAX 10 10M50DAF484C7G | Elemente Logice: 50.000 LE | Memorie SDRAM: 64 MB | Afișaje: 6x 7-Segmente | Intrări: 10 Switch-uri, 2 Push Buttons | Ieșire Video: VGA 4-bit",
                    Price = 645.00m,
                    Stock = 6,
                    Supplier = "Terasic / Intel FPGA",
                    DeliveryMethod = "Curier Rapid (24h)",
                    CategoryId = catEmbedded.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/0071c5?text=FPGA+DE10-Lite"
                },
                new Product
                {
                    Name = "Placă Arduino Nano V3.0 ATmega328P compatibilă (USB-C)",
                    Description = "Versiune ultracompactă compatibilă breadboard, ideală pentru integrarea directă pe cablaje de test și proiecte cu spațiu restrâns.",
                    Specifications = "Microcontroler: ATmega328P | Interfață USB: CH340C cu port USB Type-C | Tensiune: 5V | Pini Digitali: 14 | Pini Analogici: 8 | Dimensiuni: 45 x 18 mm",
                    Price = 28.50m,
                    Stock = 60,
                    Supplier = "RoboFun Electronics",
                    DeliveryMethod = "Easybox / Ridicare Showroom",
                    CategoryId = catEmbedded.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/00878f?text=Arduino+Nano+V3"
                },

                // ==================== CATEGORIA 2: COMPONENTE PC (6 PRODUSE) ====================
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
                    Name = "Placă video ASUS Dual NVIDIA GeForce RTX 4060 EVO OC, 8GB GDDR6",
                    Description = "Placă grafică cu arhitectură NVIDIA Ada Lovelace, suport DLSS 3, Ray Tracing și nuclee CUDA pentru procesare paralelă și accelerare AI.",
                    Specifications = "Nuclee CUDA: 3072 | Memorie: 8 GB GDDR6 | Bus memorie: 128-bit | Frecvență Boost: 2535 MHz | Conector alimentare: 1x 8-pin PCIe | Ieșiri: 1x HDMI 2.1a, 3x DisplayPort 1.4a",
                    Price = 1649.90m,
                    Stock = 11,
                    Supplier = "ASUS Tek Distribution",
                    DeliveryMethod = "Curier Rapid (24h)",
                    CategoryId = catPcParts.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/76b900?text=GeForce+RTX+4060+8GB"
                },
                new Product
                {
                    Name = "SSD Samsung 990 PRO 1TB, PCIe Gen 4.0 x4, NVMe M.2",
                    Description = "Unitate de stocare solid-state ultra-rapidă pentru încărcarea instantanee a mediilor de dezvoltare, mașinilor virtuale și bazelor de date SQL.",
                    Specifications = "Capacitate: 1 TB | Interfață: PCIe 4.0 x4 NVMe 2.0 | Viteză citire: până la 7450 MB/s | Viteză scriere: până la 6900 MB/s | Format: M.2 2280 | Memorie Cache: 1GB LPDDR4",
                    Price = 529.00m,
                    Stock = 22,
                    Supplier = "Samsung Semiconductor",
                    DeliveryMethod = "Easybox / Curier Rapid",
                    CategoryId = catPcParts.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/1428a0?text=SSD+Samsung+990+PRO+1TB"
                },
                new Product
                {
                    Name = "Memorie RAM Corsair Vengeance LPX Black 32GB (2x16GB) DDR4 3600MHz CL18",
                    Description = "Kit dual-channel cu radiator din aluminiu anodizat pentru disiparea eficientă a căldurii și stabilitate ridicată în profil XMP 2.0.",
                    Specifications = "Capacitate: 32 GB (2 x 16 GB) | Tip: DDR4 | Frecvență: 3600 MHz | Latență CAS: CL18 | Tensiune alimentare: 1.35V | Profil: Intel XMP 2.0 / AMD EXPO",
                    Price = 369.50m,
                    Stock = 30,
                    Supplier = "Corsair Memory",
                    DeliveryMethod = "Easybox / Curier Rapid",
                    CategoryId = catPcParts.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/212529?text=Corsair+32GB+DDR4+3600MHz"
                },
                new Product
                {
                    Name = "Placă de bază GIGABYTE B550 GAMING X V2, Socket AM4, ATX",
                    Description = "Placă de bază cu VRM digital în 10+3 faze, două sloturi M.2 NVMe și suport PCIe 4.0 pentru procesoarele AMD Ryzen.",
                    Specifications = "Socket: AM4 | Chipset: AMD B550 | Format: ATX | Sloturi RAM: 4x DDR4 (max. 128GB) | Stocare: 2x M.2 NVMe, 4x SATA III | Rețea: Realtek Gigabit LAN",
                    Price = 549.99m,
                    Stock = 12,
                    Supplier = "GIGABYTE Official",
                    DeliveryMethod = "Curier Rapid (24h)",
                    CategoryId = catPcParts.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/ff6600?text=GIGABYTE+B550+GAMING+X"
                },
                new Product
                {
                    Name = "Sursă Modulară Seasonic Focus GX, 80 PLUS Gold, 750W",
                    Description = "Sursă de alimentare complet modulară cu eficiență certificată 80+ Gold, condensatori japonezi la 105°C și mod fanless la sarcină redusă.",
                    Specifications = "Putere: 750W | Certificare: 80 PLUS Gold (eficiență 90%) | Modularitate: Full Modulară | Ventilator: 120mm Fluid Dynamic Bearing | Protecții: OPP, OVP, UVP, OCP, OTP, SCP",
                    Price = 589.00m,
                    Stock = 9,
                    Supplier = "Seasonic Europe",
                    DeliveryMethod = "Curier Rapid (24h)",
                    CategoryId = catPcParts.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/b8860b?text=Seasonic+Focus+GX+750W"
                },

                // ==================== CATEGORIA 3: SENZORI & ELECTRONICĂ (6 PRODUSE) ====================
                new Product
                {
                    Name = "Kit Senzor Ultrasonic HC-SR04 + Breadboard 830 puncte",
                    Description = "Pachet pentru măsurarea distanței și prototipare circuite fără lipire.",
                    Specifications = "Tensiune: 5V DC | Curent repaus: sub 2mA | Unghi detecție: 15 grade | Distanță măsurare: 2cm - 400cm",
                    Price = 29.00m,
                    Stock = 50,
                    Supplier = "RoboFun Electronics",
                    DeliveryMethod = "Easybox / Ridicare Personală",
                    CategoryId = catSensors.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/0d6efd?text=HC-SR04+%2B+Breadboard"
                },
                new Product
                {
                    Name = "Senzor Digital Temperatură și Umiditate DHT22 (AM2302)",
                    Description = "Senzor calibrat digital cu precizie ridicată pentru monitorizarea mediului în proiecte cu microcontrolere Arduino, AVR și ESP32.",
                    Specifications = "Tensiune alimentare: 3.3V - 5.5V DC | Interval temperatură: -40°C până la +80°C (precizie ±0.5°C) | Interval umiditate: 0-100% RH (precizie ±2%) | Protocol: Digital One-Wire",
                    Price = 24.50m,
                    Stock = 38,
                    Supplier = "Aosong Electronics",
                    DeliveryMethod = "Easybox / Curier Rapid",
                    CategoryId = catSensors.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/198754?text=Senzor+DHT22+AM2302"
                },
                new Product
                {
                    Name = "Servomotor Micro TowerPro SG90 9g (180 grade) cu Set Eleron",
                    Description = "Servomotor ușor și compact cu control PWM, ideal pentru brațe robotice, sisteme pan-tilt de orientare și machete automatizate.",
                    Specifications = "Tensiune operare: 4.8V - 6.0V | Cuplu: 1.8 kgf·cm (la 4.8V) | Viteză reacție: 0.1 s/60° | Unghi rotație: 180 grade | Greutate: 9 grame",
                    Price = 14.90m,
                    Stock = 85,
                    Supplier = "Optimus Digital",
                    DeliveryMethod = "Easybox / Ridicare Showroom",
                    CategoryId = catSensors.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/0dcaf0?text=Servomotor+SG90+9g"
                },
                new Product
                {
                    Name = "Afișaj OLED Grafic 0.96 inch I2C, 128x64 pixeli (Albastru/Galben)",
                    Description = "Ecran OLED luminos cu contrast ridicat care nu necesită iluminare de fundal și se conectează prin doar 2 pini de date (SDA, SCL).",
                    Specifications = "Controler: SSD1306 | Interfață: I2C (adresa 0x3C) | Rezoluție: 128 x 64 pixeli | Diagonala: 0.96 inch | Tensiune alimentare: 3.3V - 5V",
                    Price = 21.00m,
                    Stock = 44,
                    Supplier = "Waveshare Components",
                    DeliveryMethod = "Easybox / Curier Rapid",
                    CategoryId = catSensors.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/6610f2?text=Display+OLED+0.96+I2C"
                },
                new Product
                {
                    Name = "Modul Releu 4 Canale 5V 10A cu Izolare prin Optocuplor",
                    Description = "Modul de comandă pentru acționarea consumatorilor de putere (230V AC sau 30V DC) direct din pinii digitali ai microcontrolerului.",
                    Specifications = "Număr canale: 4 | Tensiune comandă: 5V DC (activ pe nivel LOW) | Sarcină maximă releu: 250V AC / 10A sau 30V DC / 10A | Protecție: Izolare galvanică cu optocuploare",
                    Price = 26.00m,
                    Stock = 33,
                    Supplier = "Optimus Digital",
                    DeliveryMethod = "Easybox / Curier Rapid",
                    CategoryId = catSensors.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/dc3545?text=Modul+Releu+4+Canale"
                },
                new Product
                {
                    Name = "Trusă 600 Rezistoare Peliculă Metalică 1% + 100 LED-uri 5mm Asortate",
                    Description = "Set complet de componente pasive și optoelectronice organizat pe valori standardizate pentru laborator și montaje pe breadboard.",
                    Specifications = "Rezistoare: 600 buc. (30 valori de la 10Ω la 1MΩ, 1/4W, toleranță 1%) | LED-uri 5mm: 100 buc. (Roșu, Galben, Verde, Albastru, Alb) | Tensiune LED: 2.0V - 3.2V (20mA)",
                    Price = 35.00m,
                    Stock = 70,
                    Supplier = "RoboFun Electronics",
                    DeliveryMethod = "Easybox / Ridicare Showroom",
                    CategoryId = catSensors.Id,
                    ImageUrl = "https://placehold.co/500x360/f8f9fa/fd7e14?text=Kit+Rezistoare+%2B+LED-uri"
                }
            };

            
            foreach (var prod in allProducts)
            {
                var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Name == prod.Name);
                if (existingProduct == null)
                {
                    context.Products.Add(prod);
                }
                else if (string.IsNullOrEmpty(existingProduct.ImageUrl))
                {
                    existingProduct.ImageUrl = prod.ImageUrl;
                }
            }

            await context.SaveChangesAsync();
        }
    }
}