using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwissProtDBreader
{
    public class utmbProtienData
    {
        //yourlist Entry   Entry name  Status Protein names Gene names Organism    Length

        public string yourlist { get; set; }
        public string Entry { get; set; }
        public string Entry_name { get; set; }
        public string Status { get; set; }
        public string Protein_names { get; set; }
        public string Gene_names { get; set; }
        public string Organism { get; set; }
        public string Length { get; set; }
        public int NumberOfTrypticPeptide { get; set; }

    }
}
