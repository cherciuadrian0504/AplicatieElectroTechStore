using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace ElectroTechStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele categoriei este obligatoriu.")]
        [StringLength(100)]
        [Display(Name = "Categorie")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Descriere Categorie")]
        public string? Description { get; set; }

        // Relație 1-la-Mulți: O categorie conține mai multe produse
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}