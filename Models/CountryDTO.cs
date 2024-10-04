using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccese.Models
{
    public class CountryDTO
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public CountryDTO(int countryID , string countryName)
        {
            CountryID = countryID;
            CountryName = countryName;
        }
    }
}
