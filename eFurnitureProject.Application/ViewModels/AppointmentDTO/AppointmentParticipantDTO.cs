using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.ViewModels.AppointmentDTO
{
    public class AppointmentParticipantDTO
    {
      
            public string UserID { get; set; }
            public string Role { get; set; }
            public string PhoneNumber { get; set; }
            public string? Email { get; set; }
        
    }
}
