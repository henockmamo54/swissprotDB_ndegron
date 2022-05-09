using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SwissProtDBreader
{
    public class Program
    {
        static void Main(string[] args)
        {
            int min_number_AminoAcidSeq = 8;
            int max_number_AminoAcidSeq = 35;

            DataReader d = new DataReader();
            var spdb = d.loadDatabase();
            //var protienData = d.loadProteinRateConst();
            var protienData = d.loadutmbProtienData();


            List<utmbProtienData> matchedutmbData = new List<utmbProtienData>();
            int count = 0;
            string sequenceChars = "";
            int previndex = 0;
            int dif = 0;

            List<string> temp = new List<string>();

            foreach (var protien in protienData)
            {
                var seq = spdb.AsParallel().Where(x => x.Description.Contains(protien.Entry)).ToList();
                if (seq.Any())
                {
                    count = 0;
                    sequenceChars = seq[0].Seq;
                    previndex = 0;

                    for (int i = 0; i < sequenceChars.Length; i++)
                    {
                        if (sequenceChars[i] == 'K' || sequenceChars[i] == 'R')
                        {
                            dif = i - previndex + 1;
                            if (dif >= min_number_AminoAcidSeq && dif <= max_number_AminoAcidSeq)
                            {
                                count++;
                                temp.Add(sequenceChars.Substring(previndex, i + 1 - previndex));
                            }
                            previndex = i + 1;
                        }
                    }
                    protien.NumberOfTrypticPeptide = count;
                    matchedutmbData.Add(protien);
                }
            }

            //CreateCSV(matchedutmbData, "test.csv");
            prepareCSV(matchedutmbData);


            #region degrons
            /*
             * 
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
                    string first_seq = temp_seq.Substring(0, 20);
                    //string last_seq = temp_seq.Skip(temp_seq.Length - 20).Take(20).ToString();

                    var is_type1 = (first_seq.Contains("MK") || first_seq.Contains("MR") || first_seq.Contains("MH"));
                    var is_type2 = (first_seq.Contains("ML") || first_seq.Contains("MF") || first_seq.Contains("MW") || first_seq.Contains("MY") || first_seq.Contains("MI"));
                    var is_type3 = (first_seq.Contains("MN") || first_seq.Contains("MQ"));


                    if (is_type1) type1.Add(protien.name);
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
            */
            #endregion
        }

        public static void prepareCSV(List<utmbProtienData> data)
        {
            TextWriter tw = new StreamWriter("test.csv");
            string fileContent = "yourlist,Entry,Entry_name,Status,Protein_names,Gene_names,Organism,Length,NumberOfTrypticPeptide\n";
            foreach (var x in data)
            {
                fileContent += x.yourlist + "," + x.Entry + "," + x.Entry_name + "," + x.Status + "," + x.Protein_names + "," + x.Gene_names +
                    "," + x.Organism + "," + x.Length + "," + x.NumberOfTrypticPeptide + "\n";
            }

            tw.WriteLine(fileContent);
            tw.Close();

        }

        public static void CreateCSV<T>(List<T> list, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                CreateHeader(list, sw);
                CreateRows(list, sw);
            }
        }

        private static void CreateHeader<T>(List<T> list, StreamWriter sw)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length - 1; i++)
            {
                sw.Write(properties[i].Name + ",");
            }
            var lastProp = properties[properties.Length - 1].Name;
            sw.Write(lastProp + sw.NewLine);
        }

        private static void CreateRows<T>(List<T> list, StreamWriter sw)
        {
            foreach (var item in list)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length - 1; i++)
                {
                    var prop = properties[i];
                    sw.Write(prop.GetValue(item) + ",");
                }
                var lastProp = properties[properties.Length - 1];
                sw.Write(lastProp.GetValue(item) + sw.NewLine);
            }
        }
    }
}
