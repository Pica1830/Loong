using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Models
{
    class context : DbContext
    {
        private static context _context;
        public context() : base("SheremetevoDB") { }
        public static context AGetContext() 
        {
            if (_context == null)
                _context = new context();
            return _context;
        }
        public DbSet<AirCraftClass> AirCraftClasses { get; set; }
        public DbSet<Aircraft_Stand> Aircraft_Stands { get; set; }
        public DbSet<Handling_Rate> Handling_Rates { get; set; }
        public DbSet<Handling_Time> Handling_Times { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
    }
}
