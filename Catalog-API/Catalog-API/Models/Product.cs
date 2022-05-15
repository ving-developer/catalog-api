using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Catalog_API.Models
{
    [Table("Product")]//Specifies the table name. By default it will be the class name
    public class Product
    {
        [Key]//Sets the property as the table's primary key
        [Column("Id")]//Sets the property name in the database
        public int ProductId { get; set; }

        [Required]//Makes a required field in the database table
        [StringLength(80)]//Sets 80 as maximum character size
        public string Name { get; set; }

        [Required]//Makes a required field in the database table
        [StringLength(255)]//Sets 255 as maximum character size
        public string  Description { get; set; }

        [Required]//Makes a required field in the database table
        [Column(TypeName = "decimal(10,2)")]//Says that the field type in the table will be decimal(10,2)
        [Range(0, float.MaxValue, ErrorMessage = "The field Price must be greater than 0.")]//Specify allowed range of values to Price field
        public float Price { get; set; }

        [Required]//Makes a required field in the database table
        [StringLength(255)]//Says that the field type in the table will be varchar(255)
        public string ImageUrl { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The field Stock must be greater than 0.")]//Specify allowed range of values to Price field
        public float Stock { get; set; }

        public DateTime RegisterDate { get; set; }

        public int CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
    }
}
