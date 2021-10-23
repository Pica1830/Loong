using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Models
{
    public class Handling_Time
    {
        [Key]
        public string Aircraft_Class { get; set; }
        public int JetBridge_Handling_Time { get; set; }
        public int Away_Handling_Time { get; set; }
    }
}
