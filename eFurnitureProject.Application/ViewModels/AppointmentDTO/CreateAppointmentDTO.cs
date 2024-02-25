using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.AppointmentDTO
{
    public class CreateAppointmentDTO
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        public int Status { get; set; }
        public int Time { get; set; }
        public List<AppointmentDetailDTO> AppointmentDetails { get; set; }
    }
}
