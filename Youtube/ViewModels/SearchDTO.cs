using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Youtube.ViewModels
{
    public class SearchDTO
    {
        public string searchText { get; set; }
        public string checkedSortVideo { get; set; }
        public string checkedSortUser { get; set; }
        public string checkedSortComment { get; set; }
        public string ascDesc { get; set; }
        public bool checkedVideo { get; set; }
        public bool checkedUser { get; set; }
        public bool checkedComment { get; set; }
    }
}
