using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CitiesInformations.Api.Entities
{
    public class City
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<PointOfInterest> PointOfInterests { get; set; }

        = new List<PointOfInterest>();

        public City(string name)
        {
            Name = name;
        }
    }

}
