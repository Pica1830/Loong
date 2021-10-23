using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Models
{
    public class TimeTable
    {
        //[Key]
        //public int Id_TimeTable { get; set; }
        public string flight_AD { get; set; }
        public DateTime flight_datetime { get; set; }
        public string flight_AL_Synchron_code { get; set; }
        public int flight_number { get; set; }
        public string flight_ID { get; set; }
        [Name("flight_terminal_#")]
        public int flight_Terminal { get; set; }
        public string flight_AP {get; set;}
        public string flight_AC_Synchron_code { get; set; }
        public int flight_AC_PAX_capacity_total { get; set; }
        public double flight_PAX { get; set; }
        [Name("Aircraft_Stand")]
        public int? Aircraft_Stand_Id { get; set; }
        public List<GroupStands> Group_Stands { get; set; } = new List<GroupStands>();
    }
}
