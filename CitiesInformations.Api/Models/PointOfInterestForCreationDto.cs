using System.ComponentModel.DataAnnotations;

namespace CitiesInformations.Api.Models
{
    public class PointOfInterestForCreationDto
    {

        [Required(ErrorMessage = "You Should Provide a name value")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
