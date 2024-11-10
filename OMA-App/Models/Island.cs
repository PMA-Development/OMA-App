using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.Models
{
    public class Island
    {
        public int IslandID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public int TurbineID { get; set; }
    }
}
