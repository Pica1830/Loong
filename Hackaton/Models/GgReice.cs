using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Models
{
    public class GgReice
    {
        public List<TimeTable> TimeTables { get; set; } = new List<TimeTable>();
        public List<GroupStands> GroupStands { get; set; } = new List<GroupStands>();
    }
}
