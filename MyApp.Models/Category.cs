using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public int OrderPlace { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
