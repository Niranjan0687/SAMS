using System.ComponentModel.DataAnnotations;

namespace SmsAPI.Models
{
    public class Person
    {
        [Key]
        public int BusinessEntityID { get; set; }

        [Required]
        [StringLength(2)]
        public string PersonType { get; set; }

        public bool NameStyle { get; set; }

        [StringLength(8)]
        public string? Title { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(10)]
        public string? Suffix { get; set; }

        [Range(0, 2)]
        public int EmailPromotion { get; set; }
    }
}
