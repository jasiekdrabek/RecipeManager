using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeManager.Model
{
    [Table("Users")]
    public partial class User
    {
        public User()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Login { get; set; }

        [Required]
        [MaxLength(40)]
        public string Password { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
