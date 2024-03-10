/* Quinton Nelson
 * 3/10/2024
 * ViewModel to return a list of technicians and hold a selected technician
 */

using SportsPro.Models;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.ViewModels
{
    public class TechnicianViewModel
    {
        [Required(ErrorMessage = "Please select a technician.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a technician.")]
        public int SelectedTechnicianId { get; set; }
        public string SelectedTechnicianName { get; set; } = null!;
        public List<Technician> Technicians { get; set; } = new List<Technician>();
    }
}
