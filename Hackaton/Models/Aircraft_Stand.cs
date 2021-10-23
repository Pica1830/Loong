using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Models
{
    public class Aircraft_Stand
    {
        [Key]
        [Name("Aircraft_Stand")]
        public int Aircraft_Stand_Id { get; set; }
        public string JetBridge_on_Arrival { get; set; }
        public string JetBridge_on_Departure {get; set;}
        [Name("1")]
        public int f1 { get; set; }
        [Name("2")]
        public int f2 { get; set; }
        [Name("3")]
        public int f3 { get; set; }
        [Name("4")]
        public int f4 { get; set; }
        [Name("5")]
        public int f5 { get; set; }
        public string Terminal { get; set; }
        public int Taxiing_Time { get; set; }
        [Ignore]
        public double? FullPrice { get; set; }
        [Ignore]
        public bool Freedom { get; set; } = true;
        [Ignore]
        public int? EndTime { get; set; }
        public List<TimeTable> TimeTables { get; set; } = new List<TimeTable>();
        [Ignore]
        public GroupStands GroupStands { get; set; }
        [Ignore]
        public TimeTable TimeTable { get; set; }
    }
}
