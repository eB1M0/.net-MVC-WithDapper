using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcWithDapper.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
