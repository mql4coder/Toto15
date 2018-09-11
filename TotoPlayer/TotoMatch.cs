using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotoLogic;

namespace TotoPlayer
{
    public class TotoMatch
    {
        public string HostsName { get; set; }
        public string VisitorsName { get; set; }
        public string LeagueName { get; set; }
        public DateTime StartTime { get; set; }
        public decimal HandicapBasis { get; set; }
        public decimal TotalBasis { get; set; }
        public bool IsSpecial { get; set; }
        public TotoBet[] Bets { get; set; } = new TotoBet[TotoConstants.ResultsCount];
    }
}
