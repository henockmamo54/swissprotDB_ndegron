using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwissProtDBreader
{
    public class Program
    {
        static void Main(string[] args)
        {
            DataReader d = new DataReader();
            var spdb = d.loadDatabase();
            var protienData = d.loadProteinRateConst();

            //fast protein
            var f_protien = protienData.Where(x => x.RateConstant >= 0.8).Take(200).ToList();
            var type1 = new List<string>();
            var type2 = new List<string>();
            var type3 = new List<string>();
            var nottype2or1 = new List<string>();
            var n_or_c = 0;

            var no_sp = 0;

            foreach (var protien in f_protien)
            {
                var seq = spdb.Where(x => x.Description.Contains(protien.name)).ToList();
                if (seq.Any())
                {
                    string temp_seq = seq.First().Seq.ToString();
                    string first_seq = temp_seq.Substring(0,20);
                    //string last_seq = temp_seq.Skip(temp_seq.Length - 20).Take(20).ToString();

                    var is_type1 = (first_seq.Contains("MK") || first_seq.Contains("MR") || first_seq.Contains("MH"));
                    var is_type2 = (first_seq.Contains("ML") || first_seq.Contains("MF") || first_seq.Contains("MW") || first_seq.Contains("MY") || first_seq.Contains("MI"));
                    var is_type3 = (first_seq.Contains("MN") || first_seq.Contains("MQ") );


                    if (is_type1) type1.Add(protien.name) ;
                    if (is_type2) type2.Add(protien.name);
                    if (is_type3) type3.Add(protien.name);

                    if (is_type1 || is_type2 || is_type3) n_or_c++;
                    if (!(is_type1 || is_type2)) nottype2or1.Add(protien.name); ;

                }
                else no_sp++;

            }

            Console.WriteLine("Total = " + f_protien.Count + " type1 = " + type1.Count() + " type2 " + type2.Count() + " type1 or type2 " + n_or_c + " np_sp " + no_sp);

            //======================================

            #region slow

            ////slow protein
            //var s_protien = protienData.Where(x => x.RateConstant < 0.1 & x.RateConstant > 0).OrderBy(x => x.RateConstant).Take(200).ToList();
            //var s_n = 0;
            //var s_c = 0;
            //var s_n_or_c = 0;

            //var s_no_sp = 0;

            //foreach (var protein in s_protien)
            //{
            //    var seq = spdb.Where(x => x.Description.Contains(protein.name)).ToList();
            //    if (seq.Any())
            //    {
            //        var temp_seq = seq.First().Seq;
            //        var first_seq = temp_seq.Take(20);
            //        var last_seq = temp_seq.Skip(temp_seq.Length - 20).Take(20);

            //        if (first_seq.Contains('R') || first_seq.Contains('K')) s_n++;
            //        if (last_seq.Contains('R') || last_seq.Contains('K')) s_c++;
            //        if (first_seq.Contains('R') || first_seq.Contains('K') || last_seq.Contains('R') || last_seq.Contains('K')) s_n_or_c++;

            //    }
            //    else { s_no_sp++; System.Console.WriteLine(protein.name); }

            //}


            //Console.WriteLine("Total = " + s_protien.Count + " n = " + s_n + " c " + s_c + " n or c " + s_n_or_c + " np_sp " + s_no_sp);

            #endregion
        }

    }
}
