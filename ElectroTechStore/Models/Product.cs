using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectroTechStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele produsului este obligatoriu.")]
        [StringLength(150)]
        [Display(Name = "Nume Produs")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrierea este obligatorie.")]
        [Display(Name = "Descriere")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specificațiile tehnice sunt obligatorii.")]
        [Display(Name = "Specificații Tehnice")]
        public string Specifications { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prețul este obligatoriu.")]
        [Range(0.01, 100000.00, ErrorMessage = "Prețul trebuie să fie mai mare decât 0.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Preț (RON)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stocul este obligatoriu.")]
        [Range(0, 100000, ErrorMessage = "Stocul nu poate fi negativ.")]
        [Display(Name = "Stoc Disponibil")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Furnizorul este obligatoriu.")]
        [StringLength(100)]
        [Display(Name = "Furnizor")]
        public string Supplier { get; set; } = string.Empty;

        [Required(ErrorMessage = "Modalitatea de livrare este obligatorie.")]
        [StringLength(100)]
        [Display(Name = "Modalitate de Livrare")]
        public string DeliveryMethod { get; set; } = string.Empty; // ex: "Curier Rapid", "Easybox", "Ridicare Personală"

        [Display(Name = "URL Imagine (opțional)")]
        public string? ImageUrl { get; set; }

        // Foreign Key către Categorie
        [Required(ErrorMessage = "Selectarea unei categorii este obligatorie.")]
        [Display(Name = "Categorie")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}