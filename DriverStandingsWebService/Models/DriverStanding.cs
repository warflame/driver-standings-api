namespace DriverStandingsWebService.Models
{
    public class DriverStanding
    {
        public int? POS { get; set; }
        public  string First_Name { get; set; }
        public  string Last_Name { get; set; }
        public string Full_Name => $"{First_Name} {Last_Name}";
        public  string Driver_Country_Code { get; set; }
        public  string Season_Team_Name { get; set; }
        public  double Season_Points { get; set; }
    }
}
