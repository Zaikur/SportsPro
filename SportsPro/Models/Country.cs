using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Country
    {
		public string CountryID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a country name.")]
        public string Name { get; set; } = string.Empty;
    }
}
