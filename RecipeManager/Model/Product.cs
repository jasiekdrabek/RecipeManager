using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeManager.Model
{
    [Table("Products")]
    public partial class Product
    {
        [Key]
        public Guid Id{ get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public int Quantity { get; set; }
        public Guid UserId { get; set; }

        public virtual User Users { get; set; }
    }
}
