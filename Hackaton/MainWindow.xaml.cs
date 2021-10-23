using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hackaton.Models;
using ClosedXML.Excel;
using Microsoft.Win32;
using System.IO;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

namespace Hackaton
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Aircraft_Stand> aircraft_s;
        List<GgReice> Reices = new List<GgReice>();
        List<TimeTable> MaxTable;
        List<GroupStands> stands;
        List<Handling_Rate> handling_Rate;
        int buscostperminute = 0;
        int awayaircraftstand = 0;
        int jetbridgeaway = 0;
        int aircrafttaxxing = 0;
        int row = 1;
        double fullpr;
        DateTime time_start;
        DateTime time_end;

        //static IXLWorkbook xL = new XLWorkbook();
        //IXLWorksheet worksheet = xL.Worksheets.Add("My sheet");
        List<TimeTable> timeTables;
        List<Models.Aircraft_Stand> aircraft_Stands = new List<Aircraft_Stand>();
        List<TimeTable> numbers = new List<TimeTable>();
        public MainWindow()
        {
            InitializeComponent();
            
        }
        class Information
        {
            public int id { get; set; }
            public List<Aircraft_Stand> Aircraft_Stands { get; set; } = new List<Models.Aircraft_Stand>();
        }
        void AddExcell(TimeTable table)
        {            
            //worksheet.Cell(row, 1).Value = "рейс " + table.flight_number;
            row++;
            if (table.flight_AD == "A")
            {

                    Counting("только прилет", table);
                    numbers.Add(table);
            }
            else
            {
                Counting("только вылет", table);
                numbers.Add(table);
            }
        }
        void Counting(string word, TimeTable table)
        {
            string terminal = "f" + table.flight_Terminal;
            int terminalTime = 0;
            double passajirsFirst = 0;
            int handTimeBus = 0;
            int handTimeTeletr = 0;
            if (table.flight_AC_PAX_capacity_total <= 120)
            {
                handTimeBus = 50;
                handTimeTeletr = 40;
            }
            else if (table.flight_AC_PAX_capacity_total <= 220)
            {
                handTimeBus = 65;
                handTimeTeletr = 50;
            }
            else
            {
                handTimeBus = 80;
                handTimeTeletr = 60;
            }
            GgReice ggReice = new GgReice();
            switch (word)
            {
                //case "2 раза":
                //    DateTime dt = timeTables.Where(p => p.flight_number - 1 == table.flight_number && p.flight_AD == "D" && p.flight_AL_Synchron_code == table.flight_AL_Synchron_code && p.flight_AC_Synchron_code == table.flight_AC_Synchron_code).FirstOrDefault().flight_datetime;
                //    minute = (dt.Hour - table.flight_datetime.Hour) * 60 + dt.Minute - table.flight_datetime.Minute;
                //    passajirsFirst = table.flight_PAX;
                //    passajirsSecond = timeTables.Where(p => p.flight_number - 1 == table.flight_number).FirstOrDefault().flight_PAX;
                //    vskolkoStoron = 2;
                //    ggReice.timePrilet = (table.flight_datetime.Hour * 60) + table.flight_datetime.Minute;
                //    ggReice.timeVilet = (dt.Hour) * 60 + dt.Minute;
                //    break;
                case "только вылет":
                    passajirsFirst = table.flight_PAX;
                    break;
                case "только прилет":
                    passajirsFirst = table.flight_PAX;
                    break;
            }
            foreach (Aircraft_Stand stand in aircraft_Stands)
            {
                if (stand.JetBridge_on_Arrival != table.flight_ID && stand.JetBridge_on_Arrival != "N")
                {
                    continue;
                }
                    switch (terminal)
                {
                    case "f1":
                        terminalTime = stand.f1;
                        break;
                    case "f2":
                        terminalTime = stand.f2;
                        break;
                    case "f3":
                        terminalTime = stand.f3;
                        break;
                    case "f4":
                        terminalTime = stand.f4;
                        break;
                    case "f5":
                        terminalTime = stand.f5;
                        break;
                }
                GroupStands grBus = new GroupStands();
                grBus.Metka = "Bus";
                GroupStands grTeletrap = new GroupStands();
                grTeletrap.Metka = "Teletrap";
                GroupStands grBusTeletr = new GroupStands();
                grBusTeletr.Metka = "BusTeletrap";
                switch (word)
                {
                    case "только вылет":
                        grBus.timeEnd = (table.flight_datetime.Hour * 60) + table.flight_datetime.Minute-stand.Taxiing_Time+1; //время когда стоянка уже свободна
                        grBus.timeBegin = grBus.timeEnd - handTimeBus-1;

                        grTeletrap.timeEnd = grBus.timeEnd;
                        grTeletrap.timeBegin = grTeletrap.timeEnd - handTimeTeletr-1;

                        grBusTeletr.timeEnd = grBus.timeEnd;
                        grBusTeletr.timeBegin = grBus.timeBegin;

                        grBus.Handling_Time = handTimeBus + stand.Taxiing_Time + terminalTime;

                        grTeletrap.Handling_Time = stand.Taxiing_Time + handTimeTeletr;
                        break;
                    case "только прилет":
                        grBus.timeBegin = (table.flight_datetime.Hour * 60) + table.flight_datetime.Minute;
                        grBus.timeEnd = grBus.timeBegin+ handTimeBus+stand.Taxiing_Time+1;

                        grTeletrap.timeBegin = grBus.timeBegin;
                        grTeletrap.timeEnd = grBus.timeBegin + handTimeTeletr + stand.Taxiing_Time + 1;


                        grBusTeletr.timeBegin = grBus.timeBegin;
                        grBusTeletr.timeEnd = grBus.timeEnd;

                        grBus.Handling_Time = handTimeBus + stand.Taxiing_Time;
                        grTeletrap.Handling_Time = stand.Taxiing_Time + handTimeTeletr;
                        break;
                }
                //worksheet.Cell(row, 1).Value = stand.Aircraft_Stand_Id;
                //worksheet.Cell(row, 2).Value = (buscostperminute * terminalTime*Math.Ceiling(passajirsFirst/80)) + stand.Taxiing_Time * aircrafttaxxing + (grBus.Handling_Time - stand.Taxiing_Time) * awayaircraftstand;
                //if (stand.JetBridge_on_Arrival != "N")
                //    if (stand.Terminal == table.flight_Terminal.ToString())
                //    {
                //        //worksheet.Cell(row, 3).Value = stand.Taxiing_Time * aircrafttaxxing + (grTeletrap.Handling_Time - stand.Taxiing_Time) * jetbridgeaway;
                //    }
                //worksheet.Cell(row, 4).Value = (buscostperminute * terminalTime * Math.Ceiling(passajirsFirst / 80)) + stand.Taxiing_Time * aircrafttaxxing + (grBus.Handling_Time - stand.Taxiing_Time) * jetbridgeaway;
                grBus.id_group = stands.Where(p => p.Aircraft_Stands[0] == stand).FirstOrDefault().id_group;
                grTeletrap.id_group = grBus.id_group;
                grBusTeletr.id_group = grBus.id_group;
                grBus.Aircraft_Stands = stands.Where(p => p.Aircraft_Stands[0] == stand).FirstOrDefault().Aircraft_Stands;
                grBusTeletr.Aircraft_Stands = grBus.Aircraft_Stands;
                grTeletrap.Aircraft_Stands = grBus.Aircraft_Stands;
                grBus.price = (buscostperminute * terminalTime * Math.Ceiling(passajirsFirst / 80)) + stand.Taxiing_Time * aircrafttaxxing + handTimeBus * awayaircraftstand;
                grBusTeletr.price = (buscostperminute * terminalTime * Math.Ceiling(passajirsFirst / 80)) + stand.Taxiing_Time * aircrafttaxxing + handTimeBus * jetbridgeaway;
                if (stand.JetBridge_on_Arrival != "N")
                    if (stand.Terminal == table.flight_Terminal.ToString())
                    {

                            grTeletrap.price = stand.Taxiing_Time * aircrafttaxxing + handTimeTeletr * jetbridgeaway;
                            ggReice.GroupStands.Add(grTeletrap);
                        
                    }
                ggReice.GroupStands.Add(grBus);
                ggReice.GroupStands.Add(grBusTeletr);
                row++;
            }
            ggReice.TimeTables.Add(table);
            //foreach (GroupStands gg in ggReice.GroupStands)
            //{
            //    if (gg.priceBus < minimum)
            //        minimum = (double)gg.priceBus;
            //    if (gg.priceTeletrap < minimum)
            //        minimum = (double)gg.priceTeletrap;
            //    if (gg.priceBusInPlaceTeletrap < minimum)
            //        minimum = (double)gg.priceBusInPlaceTeletrap;
            //}
            ggReice.GroupStands = ggReice.GroupStands.OrderBy(p => p.price).ToList();
            Reices.Add(ggReice);
        }
        






        public void FullPharsh()
        {
            Reices = Reices.OrderBy(p => p.GroupStands[0].timeBegin).ToList();
            foreach(GgReice reice in Reices)
            {
                if(reice.TimeTables[0].flight_number == 200)
                { }
                foreach (GroupStands stands in reice.GroupStands)
                {
                    bool reiceAll = false;
                    foreach (Aircraft_Stand aircraft_Stand in stands.Aircraft_Stands)
                    {
                        Aircraft_Stand std = aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id).FirstOrDefault();
                        if (reiceAll)
                            break;
                        if(std.Freedom && (std.EndTime <= stands.timeBegin || std.EndTime == null))
                        {
                            if (reice.TimeTables[0].flight_AC_PAX_capacity_total > 220 && aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id).FirstOrDefault().Terminal != "")
                            {
                                try
                                {
                                    if (aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id - 1).FirstOrDefault().TimeTable.flight_AC_PAX_capacity_total > 220 && aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id - 1).FirstOrDefault().Terminal != "")
                                    {
                                        continue;
                                    }
                                }
                                catch { }
                                try
                                {
                                    if (aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id + 1).FirstOrDefault().TimeTable.flight_AC_PAX_capacity_total > 220 && aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id + 1).FirstOrDefault().Terminal != "")
                                    {
                                        continue;
                                    }
                                }
                                catch { }
                            }
                            foreach (Aircraft_Stand airs in aircraft_s)
                                if (airs.EndTime <= stands.timeBegin)
                                {
                                    if(airs.Aircraft_Stand_Id ==1)
                                    { }
                                    //airs.TimeTable = null;
                                    //airs.EndTime = null;
                                    airs.Freedom = true;
                                }
                            TimeTable tb = reice.TimeTables[0];
                            tb.Aircraft_Stand_Id = aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id).FirstOrDefault().Aircraft_Stand_Id;
                            aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id).FirstOrDefault().Freedom = false;
                            aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id).FirstOrDefault().EndTime = stands.timeEnd;
                            aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id).FirstOrDefault().TimeTable = tb;
                            aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id).FirstOrDefault().FullPrice = stands.price;
                            fullpr += (double)stands.price;
                            reiceAll = true;
                            break;
                        }
                        else
                        {
                            bool exit = false;
                            GgReice rice = reice;
                            foreach (GroupStands _stands in rice.GroupStands)
                            {
                                    Aircraft_Stand std1 = aircraft_s.Where(p => p.Aircraft_Stand_Id == _stands.Aircraft_Stands[0].Aircraft_Stand_Id).FirstOrDefault();
                                    if (std1.Freedom)
                                    {
                                        GgReice gg = Reices.Where(p => p.TimeTables[0] == std.TimeTable).FirstOrDefault();
                                        foreach (GroupStands stands1 in gg.GroupStands)
                                        {
                                                if (stands1.Aircraft_Stands[0].Freedom)
                                                {
                                                    if (std.FullPrice + _stands.price < stands1.price + stands.price)
                                                    {
                                                        exit = true;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        TimeTable tb = rice.TimeTables[0];
                                                        tb.Aircraft_Stand_Id = aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id).FirstOrDefault().Aircraft_Stand_Id;
                                                        aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id).FirstOrDefault().Freedom = false;
                                                        aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id).FirstOrDefault().EndTime = stands.timeEnd;
                                                        aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id).FirstOrDefault().TimeTable = tb;
                                                        aircraft_s.Where(p => p.Aircraft_Stand_Id == aircraft_Stand.Aircraft_Stand_Id).FirstOrDefault().FullPrice = stands.price;
                                                TimeTable tb1 = gg.TimeTables[0];
                                                tb.Aircraft_Stand_Id = aircraft_s.Where(p => p.Aircraft_Stand_Id == stands1.Aircraft_Stands[0].Aircraft_Stand_Id).FirstOrDefault().Aircraft_Stand_Id;
                                                aircraft_s.Where(p => p.Aircraft_Stand_Id == stands1.Aircraft_Stands[0].Aircraft_Stand_Id).FirstOrDefault().Freedom = false;
                                                aircraft_s.Where(p => p.Aircraft_Stand_Id == stands1.Aircraft_Stands[0].Aircraft_Stand_Id).FirstOrDefault().EndTime = stands.timeEnd;
                                                aircraft_s.Where(p => p.Aircraft_Stand_Id == stands1.Aircraft_Stands[0].Aircraft_Stand_Id).FirstOrDefault().TimeTable = tb;
                                                aircraft_s.Where(p => p.Aircraft_Stand_Id == stands1.Aircraft_Stands[0].Aircraft_Stand_Id).FirstOrDefault().FullPrice = stands.price;
                                                exit = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (exit)
                                                break;
                                    }
                                }
                        }

                    }                   
                    if (reiceAll)
                        break;
                }
            }
            using (StreamWriter streamReader = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}result.csv"))
            {
                var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ",", Encoding = Encoding.UTF8 };
                using (CsvWriter csvReader = new CsvWriter(streamReader, config))
                {
                    csvReader.WriteRecords(MaxTable);
                }
            }
        }

        private void clTimeTable(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile1 = new OpenFileDialog();
            openFile1.ShowDialog();
            using (StreamReader strea = new StreamReader(openFile1.FileName))
            {
                var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ",", Encoding = Encoding.UTF8 };
                using (var csvReade = new CsvReader(strea, config))
                {
                    timeTables = csvReade.GetRecords<TimeTable>().ToList();
                    MaxTable = timeTables;
                }
            }
            TimeTable_label.Content = "Файл TimeTable загружен";
            TimeTable_btn.Content = "Заменить Time Table";
            TimeTable_ic.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
            TimeTable_ic.Foreground = new SolidColorBrush(Colors.Green);
        }

        private void clAircfraft_Stand(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile2 = new OpenFileDialog();
            openFile2.ShowDialog();
            using (StreamReader strea = new StreamReader(openFile2.FileName))
            {
                var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ",", Encoding = Encoding.UTF8 };
                using (var csvReade = new CsvReader(strea, config))
                {
                    aircraft_s = csvReade.GetRecords<Aircraft_Stand>().ToList();
                }
            }
            stands = GroupStands.GetGroups(aircraft_s);
            Aircfraft_Stand_label.Content = "Файл Aircfraft Stand загружен";
            Aircfraft_Stand_btn.Content = "Заменить Aircfraft Stand";
            Aircfraft_Stand_ic.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
            Aircfraft_Stand_ic.Foreground = new SolidColorBrush(Colors.Green);
        }

        private void clHandlingTime(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile2 = new OpenFileDialog();
            openFile2.ShowDialog();
            using (StreamReader strea = new StreamReader(openFile2.FileName))
            {
                var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ",", Encoding = Encoding.UTF8 };
                using (var csvReade = new CsvReader(strea, config))
                {
                    handling_Rate = csvReade.GetRecords<Handling_Rate>().ToList();
                }
            }
            buscostperminute = handling_Rate[0].Value;
            awayaircraftstand = handling_Rate[1].Value;
            jetbridgeaway = handling_Rate[2].Value;
            aircrafttaxxing = handling_Rate[3].Value;
            Handling_Rate_label.Content = "Файл Handling Rate загружен";
            Handling_Rate_btn.Content = "Заменить Handling Rate Stand";
            Handling_Rate_ic.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
            Handling_Rate_ic.Foreground = new SolidColorBrush(Colors.Green);
        }

        private void clRun(object sender, RoutedEventArgs e)
        {
            //time_start = DateTime.Now;
            //worksheet.Cell(1, 3).Value = "Стоимость по телетрапу";
            //worksheet.Cell(1, 2).Value = "Стоимость с автобусом";
            //worksheet.Cell(1, 4).Value = "Стоимость с автобусом на стоянке с телетрапом";


            foreach (GroupStands vr in stands)
            {
                aircraft_Stands.Add(vr.Aircraft_Stands.FirstOrDefault());
            }
            //List<Models.AirCraftClass> airCraftClasses = Models.context.AGetContext().AirCraftClasses.ToList();
            List<Information> information = new List<Information>();
            foreach (TimeTable timeTable in timeTables)
            {
                if (numbers.Contains(timeTable))
                    continue;
                // information.Add()
                AddExcell(timeTable);
            }
            //xL.SaveAs($"{AppDomain.CurrentDomain.BaseDirectory}serv.xlsx");
            FullPharsh();
            Done_ic.Visibility = Visibility.Visible;
            //time_end = DateTime.Now;

            //int res = time_end.Second - time_start.Second;
        }
    }

}
