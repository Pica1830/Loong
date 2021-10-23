using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Models
{
    public class GroupStands
    {
        public int id_group { get; set; }
        public string Metka { get; set; }
        //public double? priceBus { get; set; }
        //public double? priceTeletrap { get; set; }
        //public double? priceBusInPlaceTeletrap { get; set; }
        public double? price { get; set; }
        //public int? timeBeginBus { get; set; }
        //public int? timeEndBus { get; set; }
        public int? timeBegin { get; set; }
        public int? timeEnd { get; set; }

        //public int? timeBeginTeletrap { get; set; }
        //public int? timeEndTeletrap { get; set; }
        public int? Handling_Time { get; set; }
        //public int? Handling_Time_Bus { get; set; }
        //public int? Handling_Time_Teletrap { get; set; }
        public List<Aircraft_Stand> Aircraft_Stands { get; set; } = new List<Aircraft_Stand>();
        public List<TimeTable> TimeTables = new List<TimeTable>();
        public static List<GroupStands> GetGroups(List<Aircraft_Stand> air)
        {
            List<GroupStands> stands = new List<GroupStands>();
            //OpenFileDialog openFile1 = new OpenFileDialog();
            //openFile1.ShowDialog();
            //using (StreamReader strea = new StreamReader(openFile1.FileName))
            //{
            //    var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ",", Encoding = Encoding.UTF8 };
            //    using (var csvReade = new CsvReader(strea, config))
            //    {
                    List<Aircraft_Stand> aircraft_s = new List<Aircraft_Stand>();
                    Aircraft_Stand oldStand = new Aircraft_Stand();
                    int count = 1;
                            stands.Add(new GroupStands { id_group = count });
                    foreach (Aircraft_Stand std in air)
                    {
                        if (std.Aircraft_Stand_Id == 1)
                            oldStand = std;
                        if(std.JetBridge_on_Arrival == oldStand.JetBridge_on_Arrival && std.f1 ==oldStand.f1 && oldStand.f2 == std.f2 && oldStand.Terminal == std.Terminal && oldStand.Taxiing_Time == std.Taxiing_Time)
                        {
                            aircraft_s.Add(std);
                            stands[count-1].Aircraft_Stands.Add(std);
                            oldStand = std;
                        }
                        else
                        {
                            count++;
                            stands.Add(new GroupStands { id_group = count });
                            stands[count-1].Aircraft_Stands.Add(std);
                            oldStand = std;
                        }
                    }
                    return stands;
            //    }
            //}
        }
    }
}
