using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog_API.Models
{
    [Table("Category")]//Specifies the table name. By default it will be the class name
    public class Category
    {
        public Category()
        {
            Products = new Collection<Product>();
        }

        [Key]//Sets the property as the table's primary key
        [Column("Id")]//Sets the property name in the database
        public int CategoryId { get; set; }

        [Required]//Makes a required field in the database table
        [StringLength(80)]//Sets 80 as maximum character size
        public string Name { get; set; }

        [Required]//Makes a required field in the database table
        [StringLength(300)]//Sets 300 maximum character size
        public string ImageUrl { get; set; }

        public ICollection<Product> Products { get; set; }//Informs the EntityFramework that there is a 1-N relationship with the Product table
    }
}
