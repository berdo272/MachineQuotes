namespace MachineQuotes.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MachineQuotes.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MachineQuotes.Models.ApplicationDbContext context)
        {
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = new ApplicationUserManager(userStore);
            ApplicationUser admin = new ApplicationUser { Email = "it@sussextool.com", UserName = "it@sussextool.com" };

            userManager.Create(admin, password: "H@/L140pd");
            roleManager.Create(new IdentityRole { Name = "admin" });
            userManager.AddToRole(admin.Id, "admin");

            List<Option> Options = new List<Option>
            {
                new Option
                {
                    Group = "Air Blow",
                    Description = "Cutting Air Blow",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "Air Gun",
                    Description = "",
                    IsStandard = false,
                    Price = 975,
                },
                new Option
                {
                    Group = "Air Conditioner",
                    Description = "",
                    IsStandard = false,
                    Price = 2340,
                },
                new Option
                {
                    Group = " APC(SHUTTLE OR DUAL)",
                    Description = "T-Slot",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "ATC",
                    Description = "30EA",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "ATC",
                    Description = "40EA",
                    IsStandard = false,
                    Price = 0,
                },
                new Option
                {
                    Group = "Auto Door",
                    Description = "",
                    IsStandard = false,
                    Price = 2288,
                },
                new Option
                {
                    Group = "Auto Power Cut Off Device",
                    Description = "NFB Shunt Type",
                    IsStandard = false,
                    Price = 650,
                },
                new Option
                {
                    Group = "Auto Power Cut Off Device",
                    Description = "ELB Type",
                    IsStandard = false,
                    Price = 2340,
                },
                new Option
                {
                    Group = "Auto Tool Measuring Device",
                    Description = "Laser Renishaw(NC4)",
                    IsStandard = false,
                    Price = 8450,
                },
                new Option
                {
                    Group = "Auto Tool Measuring Device",
                    Description = "Laser Renishaw(TRS-2)",
                    IsStandard = false,
                    Price = 3380,
                },
                new Option
                {
                    Group = "Auto Tool Measuring Device",
                    Description = "Laser Marposs(ML75)",
                    IsStandard = false,
                    Price = 12337,
                },
                new Option
                {
                    Group = "Auto Tool Measuring Device",
                    Description = "Touch Marposs(T18-E32)",
                    IsStandard = false,
                    Price = 3295,
                },
                new Option
                {
                    Group = "Auto Tool Measuring Device ",
                    Description = "Touch Proximity SW",
                    IsStandard = false,
                    Price = 2197,
                },
                new Option
                {
                    Group = "Auto Work Measuring Device (Optical)",
                    Description = "Renishaw OMP40",
                    IsStandard = false,
                    Price = 8957,
                },
                new Option
                {
                    Group = "Auto Work Measuring Device (Optical)",
                    Description = "Renishaw OMP60",
                    IsStandard = false,
                    Price = 7943,
                },
                new Option
                {
                    Group = "Auto Work Measuring Device (Optical)",
                    Description = "Renishaw RMP60",
                    IsStandard = false,
                    Price = 13182,
                },
                new Option
                {
                    Group = "Auto Work Measuring Device (Optical)",
                    Description = "Renishaw RMP600",
                    IsStandard = false,
                    Price = 13182,
                },
                new Option
                {
                    Group = "Auto Work Measuring Device (Optical)",
                    Description = "Marposs E83C-TXL110",
                    IsStandard = false,
                    Price = 11830,
                },
                new Option
                {
                    Group = "Auto Work Measuring Device (Optical)",
                    Description = "Marposs T25-E86N",
                    IsStandard = false,
                    Price = 12675,
                },
                new Option
                {
                    Group = "Chip Conveyor",
                    Description = "Side Hinged",
                    IsStandard = false,
                    Price = 6825,
                },
                new Option
                {
                    Group = "Chip Conveyor",
                    Description = "Side Scraper",
                    IsStandard = false,
                    Price = 6825,
                },
                new Option
                {
                    Group = "Chip Box",
                    Description = "Fixed Type",
                    IsStandard = false,
                    Price = 552,
                },
                new Option
                {
                    Group = "Chip Box",
                    Description = "Swing Type (Swing Large)",
                    IsStandard = false,
                    Price = 1326,
                },
                new Option
                {
                    Group = "Chip Box",
                    Description = "Large Capacity",
                    IsStandard = false,
                    Price = 1105,
                },
                new Option
                {
                    Group = "Coolant",
                    Description = "Bed Flushing",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "Coolant",
                    Description = "Jet/Niagara",
                    IsStandard = false,
                    Price = 1657,
                },
                new Option
                {
                    Group = "Coolant",
                    Description = "Oil Skimmer",
                    IsStandard = false,
                    Price = 2145,
                },
                new Option
                {
                    Group = "Coolant",
                    Description = "Tank Elimination *MINUS OPTION*",
                    IsStandard = false,
                    Price = -600,
                },
                new Option
                {
                    Group = "HYD/Air Supply Device",
                    Description = "HYD. FIXTURE 45 kg/cm2 (Up to 70 kg/cm2)  2 x 2 Ports",
                    IsStandard = false,
                    Price = 2925,
                },                
                new Option
                {
                    Group = "HYD/Air Supply Device",
                    Description = "HYD. FIXTURE 45 kg/cm2 (Up to 70 kg/cm2)  2 x 3 Ports",
                    IsStandard = false,
                    Price = 4875,
                },
                new Option
                {
                    Group = "HYD/Air Supply Device",
                    Description = "HYD. FIXTURE 45 kg/cm2 (Up to 70 kg/cm2)  2 x 4 Ports",
                    IsStandard = false,
                    Price = 6045,
                },
                new Option
                {
                    Group = "HYD/Air Supply Device",
                    Description = "HYD. FIXTURE 45 kg/cm2 (Up to 70 kg/cm2)  2 x 3 Ports (Semi type)",
                    IsStandard = false,
                    Price = 2145,
                },
                new Option
                {
                    Group = "HYD/Air Supply Device",
                    Description = "HYD. FIXTURE 45 kg/cm2 (Up to 70 kg/cm2)  2 x 5 Ports (2 X 6 Ports for HS series)",
                    IsStandard = false,
                    Price = 7020,
                },
                new Option
                {
                    Group = "HYD/Air Supply Device",
                    Description = "HYD. FIXTURE 45 kg/cm2 (Up to 70 kg/cm2)   Extra Hyd. Unit",
                    IsStandard = false,
                    Price = 2399,
                },
                new Option
                {
                    Group = "HYD/Air Supply Device",
                    Description = "PNEUMATIC FIXTURE  2 x 2 Ports",
                    IsStandard = false,
                    Price = 1950,
                },
                new Option
                {
                    Group = "HYD/Air Supply Device",
                    Description = "PNEUMATIC FIXTURE  2 x 3 Ports",
                    IsStandard = false,
                    Price = 2925,
                },
                new Option
                {
                    Group = "HYD/Air Supply Device",
                    Description = "PNEUMATIC FIXTURE  2 x 4 Ports",
                    IsStandard = false,
                    Price = 3900,
                },
                new Option
                {
                    Group = "HYD/Air Supply Device",
                    Description = "PNEUMATIC FIXTURE  2 x 5 Ports",
                    IsStandard = false,
                    Price = 4875,
                },
                new Option
                {
                    Group = "High Column Option",
                    Description = "180MM",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "Controller",
                    Description = "NC F-31i / S840D",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "MPG Handle(REMOTE TYPE)",
                    Description = "1-Axis(Handle 1EA)",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "MPG Handle(REMOTE TYPE)",
                    Description = "3-Axis(Handle 3EA)",
                    IsStandard = false,
                    Price = 1950,
                },
                new Option
                {
                    Group = "Oil Mist Collector",
                    Description = "Young AC11000 (No Filter)",
                    IsStandard = false,
                    Price = 1840,
                },
                new Option
                {
                    Group = "Oil Mist Collector",
                    Description = "YHB YMC400(Water Type)",
                    IsStandard = false,
                    Price = 2340,
                },
                new Option
                {
                    Group = "Oil Mist Collector",
                    Description = "YHB YMC420(Water Type)",
                    IsStandard = false,
                    Price = 0,
                },
                new Option
                {
                    Group = "Oil Mist Collector",
                    Description = "YHB YOC400(No Water)",
                    IsStandard = false,
                    Price = 2589,
                },
                new Option
                {
                    Group = "Oil Mist Collector Accessory",
                    Description = "YHB Extra Filter",
                    IsStandard = false,
                    Price = 343,
                },
                new Option
                {
                    Group = "Patrol Lamp",
                    Description = "LED - R, G, Y (3 Color)",
                    IsStandard = false,
                    Price = 398,
                },
                new Option
                {
                    Group = "Patrol Lamp",
                    Description = "LED - R, G, Y, (3 Color) + Buzzer",
                    IsStandard = false,
                    Price = 420,
                },
                new Option
                {
                    Group = "Spindle Thru Coolant",
                    Description = "5.5kgf W/O Drum Filter",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "Spindle Thru Coolant",
                    Description = "20kgf  W/T COOL JET",
                    IsStandard = false,
                    Price = 13520,
                },
                new Option
                {
                    Group = "Spindle Thru Coolant",
                    Description = "30kgf(20L)",
                    IsStandard = false,
                    Price = 13858,
                },
                new Option
                {
                    Group = "Spindle Thru Coolant",
                    Description = "70kgf(15L)  W/O DRUM FILTER",
                    IsStandard = false,
                    Price = 16055,
                },
                new Option
                {
                    Group = "Spindle Thru Coolant",
                    Description = "70kgf(30L)  DOUBLE",
                    IsStandard = false,
                    Price = 10647,
                },
                new Option
                {
                    Group = "Scale, Linear",
                    Description = "Heidenhain X-Axis",
                    IsStandard = false,
                    Price = 7605,
                },
                new Option
                {
                    Group = "Scale, Linear",
                    Description = "Heidenhain Y-Axis",
                    IsStandard = false,
                    Price = 7605,
                },
                new Option
                {
                    Group = "Scale, Linear",
                    Description = "Heidenhain Z-Axis",
                    IsStandard = false,
                    Price = 5577,
                },
                new Option
                {
                    Group = "Spindle Motor Power Up (MAIN)",
                    Description = "22/25 KW",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "Spindle Taper",
                    Description = "HSK-A63",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "Spindle Speed",
                    Description = "15,000 RPM",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "Spindle Oil Cooling Device",
                    Description = "Oil Con. Std. RPM",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "Spindle Oil Cooling Device",
                    Description = "Oil Con. Opt. RPM",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "Top Cover",
                    Description = "For Spindle Thru Coolant",
                    IsStandard = true,
                    Price = 0,
                },
                new Option
                {
                    Group = "Tool Monitoring System",
                    Description = "1 Channel, Marposs",
                    IsStandard = false,
                    Price = 8450,
                },
                new Option
                {
                    Group = "Tool Monitoring System",
                    Description = "2 Channel, Marposs",
                    IsStandard = false,
                    Price = 10985,
                },
                new Option
                {
                    Group = "Tool Monitoring System",
                    Description = "3 Channel, Marposs",
                    IsStandard = false,
                    Price = 0,
                },
                new Option
                {
                    Group = "Tool Monitoring System",
                    Description = "4 Channel, Marposs",
                    IsStandard = false,
                    Price = 0,
                },
                new Option
                {
                    Group = "Transformer",
                    Description = "60 KVA",
                    IsStandard = false,
                    Price = 2535,
                },
                new Option
                {
                    Group = "Transformer Accessory",
                    Description = "CABLE",
                    IsStandard = false,
                    Price = 388,
                },
                new Option
                {
                    Group = "Special Color",
                    Description = "Old Hyunai-Wia or Customer Special",
                    IsStandard = false,
                    Price = 1690,
                },
            };

            Options.ForEach(c => context.Options.Add(c));

            List<Machine> Machines = new List<Machine>
            {
                new Machine
                {
                    MachineName = "Hi-Mold750/5A",
                    Price = 388089,
                    Options = Options,
                },
            };



            Machines.ForEach(b => context.Machines.Add(b));

            context.SaveChanges();

        }
    }
}
