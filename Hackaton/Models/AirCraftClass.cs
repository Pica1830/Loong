using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Models
{
    public class AirCraftClass
    {
        [Key]
        public string Aircraft_Class {get ;set;}
        public int Max_Seats { get; set; }
    }
}
