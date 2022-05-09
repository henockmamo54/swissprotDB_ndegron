using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwissProtDBreader
{
    public class DataReader
    {

        public List<spDataModel> loadDatabase()
        {
            string path = "H:/Warehouse/Data/ProteinSequenceDatabases/SwissProt_2021_02.fasta";
            List<spDataModel> spData = new List<spDataModel>();
            //Console.WriteLine("test");

            if (File.Exists(path))
            {
                Console.WriteLine("==> file found");

                try
                {
                    //read all the lines
                    string[] lines = System.IO.File.ReadAllLines(path);
                    lines = lines.Where(x => x.Length > 0).ToArray();

                    for (int i = 0; i < lines.Length - 1;)
                    {
                        var protein_descritption = lines[i];
                        i = i + 1;
                        var protein_seq = "";
                        while (!lines[i].Contains('>') && (i < lines.Length))
                        {
                            protein_seq += lines[i];
                            i = i + 1;

                            if (i >= lines.Length) break;
                        }

                        spDataModel sp = new spDataModel();
                        sp.Seq = protein_seq;
                        sp.Description = protein_descritption.Split('|')[1].Trim();
                        spData.Add(sp);
                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                }

            }


            return spData;


        }

        public List<ProteinData> loadProteinRateConst()
        {
            string path = "C:/Workplace/C++/d2ome_v2/v2/v2/bin/Debug/compare.csv";
            List<ProteinData> proteinDataList = new List<ProteinData>();


            if (File.Exists(path))
            {
                Console.WriteLine("==> file found");

                try
                {
                    //read all the lines
                    string[] lines = System.IO.File.ReadAllLines(path);
                    lines = lines.Where(x => x.Length > 0).ToArray();

                    for (int i = 1; i < lines.Length - 1; i++)
                    {
                        var content = lines[i].Trim().Split(',');

                        if (content.Length > 1)
                        {
                            try
                            {
                                ProteinData proteinData = new ProteinData();
                                proteinData.name = content[0].Trim();
                                proteinData.RateConstant = double.Parse(content[1].Trim());
                                proteinDataList.Add(proteinData);
                            }
                            catch (Exception ex) { continue; }
                        }

                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                }

            }

            return proteinDataList;

        }

        public List<utmbProtienData> loadutmbProtienData()
        {

            //string path = "C:/Workplace/Python/d2omeplusAnalysis/d2omePlusAnalysis/Protein_List.utmb.0311_2022.csv";
            string path = "Protein_List.utmb.0311_2022.csv";
            List<utmbProtienData> proteinDataList = new List<utmbProtienData>();


            if (File.Exists(path))
            {
                Console.WriteLine("==> file found");

                try
                {
                    //read all the lines
                    string[] lines = System.IO.File.ReadAllLines(path);
                    lines = lines.Where(x => x.Length > 0).ToArray();

                    for (int i = 1; i < lines.Length - 1; i++)
                    {
                        var content = lines[i].Trim().Split(',');

                        if (content.Length > 1)
                        {
                            try
                            {
                                utmbProtienData proteinData = new utmbProtienData();
                                proteinData.yourlist = content[0].Trim();
                                proteinData.Entry = content[1].Trim();
                                proteinData.Entry_name = content[2].Trim();
                                proteinData.Status = content[3].Trim();
                                proteinData.Protein_names = content[4].Trim(); //.Replace("##&##",",")
                                proteinData.Gene_names = content[5].Trim();
                                proteinData.Organism = content[6].Trim();
                                proteinData.Length = content[7].Trim();
                                proteinDataList.Add(proteinData);
                            }
                            catch (Exception ex) { continue; }
                        }

                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                }

            }

            return proteinDataList;
        }

    }
}
