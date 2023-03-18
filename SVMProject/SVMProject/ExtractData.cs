using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMProject
{
    internal class DataManipulation
    {
        public string[] lines = File.ReadAllLines(@"C:\Users\STEFANIA\Desktop\Facultate\IA\PROIECT 2023\nursery.data");
        public int[,] valuesClasified;
        public int[,] valuesNonRecommend = new int[37, 8];
        public int[,] valuesRecommend = new int[37, 8];
        public int[,] valuesVeryRecommend = new int[22, 8];
        public int[,] valuesPriority = new int[46, 8];
        int NonRecomLines = 0;
        int RecomLines = 0;
        int VeryRecomLines = 0;
        int PriorityLines = 0;
        int noInstances;
        public void fileRead()
        {
            //string[] lines = File.ReadAllLines(@"C:\Users\STEFANIA\Desktop\Facultate\IA\PROIECT 2023\nursery.data");

            valuesClasified = new int[lines.Length, 9];
            //int[,] valuesNonRecommend = new int[37, 8];
            //int[,] valuesRecommend = new int[37, 8];
            //int[,] valuesVeryRecommend = new int[22, 8];
            //int[,] valuesPriority = new int[46, 8];

            noInstances = lines.Length;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');

                switch (values[0]) // parents =  ocupatia parintilor
                {
                    case "usual":
                        valuesClasified[i, 0] = 1;
                        break;
                    case "pretentious":
                        valuesClasified[i, 0] = 2;
                        break;
                    case "great_pret":
                        valuesClasified[i, 0] = 3;
                        break;
                    default:
                        break;
                }

                switch (values[1]) // has_nurs = Cresa pt copii
                {
                    case "proper":
                        valuesClasified[i, 1] = 1;
                        break;
                    case "less_proper":
                        valuesClasified[i, 1] = 2;
                        break;
                    case "improper":
                        valuesClasified[i, 1] = 3;
                        break;
                    case "critical":
                        valuesClasified[i, 1] = 4;
                        break;
                    case "very_crit":
                        valuesClasified[i, 1] = 5;
                        break;
                    default:
                        break;
                }

                switch (values[2]) // form =  forma familiei
                {
                    case "complete":
                        valuesClasified[i, 2] = 1;
                        break;
                    case "completed":
                        valuesClasified[i, 2] = 2;
                        break;
                    case "incomplete":
                        valuesClasified[i, 2] = 3;
                        break;
                    case "foster":
                        valuesClasified[i, 2] = 4;
                        break;
                    default:
                        break;
                }

                switch (values[3]) // children =  numarul de copii
                {
                    case "1":
                        valuesClasified[i, 3] = 1;
                        break;
                    case "2":
                        valuesClasified[i, 3] = 2;
                        break;
                    case "3":
                        valuesClasified[i, 3] = 3;
                        break;
                    case "more":
                        valuesClasified[i, 3] = 4;
                        break;
                    default:
                        break;
                }
                switch (values[4]) // housing = starea locuintei
                {
                    case "convenient":
                        valuesClasified[i, 4] = 1;
                        break;
                    case "less_conv":
                        valuesClasified[i, 4] = 2;
                        break;
                    case "critical":
                        valuesClasified[i, 4] = 3;
                        break;
                    default:
                        break;
                }
                switch (values[5]) // finance = situatia financiara
                {
                    case "convenient":
                        valuesClasified[i, 5] = 1;
                        break;
                    case "inconv":
                        valuesClasified[i, 5] = 2;
                        break;
                    default:
                        break;
                }
                switch (values[6]) // social = conditii sociale
                {
                    case "non-prob": //non-problematic
                        valuesClasified[i, 6] = 1;
                        break;
                    case "slightly_prob":
                        valuesClasified[i, 6] = 2;
                        break;
                    case "problematic":
                        valuesClasified[i, 6] = 3;
                        break;
                    default:
                        break;
                }
                switch (values[7]) // health = conditii de sanatate
                {
                    case "recommended":
                        valuesClasified[i, 7] = 1;
                        break;
                    case "priority":
                        valuesClasified[i, 7] = 2;
                        break;
                    case "not_recom":
                        valuesClasified[i, 7] = 3;
                        break;
                    default:
                        break;
                }


                switch (values[8])
                {
                    case "not_recom":
                        valuesClasified[i, 8] = -1;
                        break;

                    case "recommend":
                        valuesClasified[i, 8] = 0;
                        break;

                    case "very_recom":
                        valuesClasified[i, 8] = 1;
                        break;

                    case "priority":
                        valuesClasified[i, 8] = 2;
                        break;

                    default:
                        break;
                }


                //Console.WriteLine("valoare = " + valuesClasified[0] + " " + valuesClasified[1] + " " + valuesClasified[2] + " " + valuesClasified[3] + " " + valuesClasified[4] + " " + valuesClasified[5] + " " + valuesClasified[6] + " " + valuesClasified[7] + " " + valuesClasified[8]);

            } //end for

            // Clasificare date in mai multi vectori in functie de ultimul parametru (very-recom, recom, non-recom, priority)
            for (int k = 0; k < lines.Length - 1; k++)
            {
                switch (valuesClasified[k, 8])
                {
                    case -1:
                        for (int j = 0; j < 7; j++)
                        {
                            valuesNonRecommend[NonRecomLines, j] = valuesClasified[k, j];
                            //Console.WriteLine("NR " + valuesNonRecommend[k, j] + " ");
                        }
                        NonRecomLines++;
                        break;
                    case 0:
                        for (int j = 0; j < 7; j++)
                        {
                            valuesRecommend[RecomLines, j] = valuesClasified[k, j];
                            //Console.WriteLine("R " + valuesRecommend[k, j] + " ");
                        }
                        RecomLines++;
                        break;
                    case 1:
                        for (int j = 0; j < 7; j++)
                        {
                            valuesVeryRecommend[VeryRecomLines, j] = valuesClasified[k, j];
                            //Console.WriteLine("VR " + valuesVeryRecommend[k, j] + " ");
                        }
                        VeryRecomLines++;
                        break;
                    case 2:
                        for (int j = 0; j < 7; j++)
                        {
                            valuesPriority[PriorityLines, j] = valuesClasified[k, j];
                            //Console.WriteLine("P " + valuesPriority[k, j] + " ");
                        }
                        PriorityLines++;
                        break;
                    default:
                        break;
                } //end switch
            } //end for
        }


        public void writeInFile()
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Users\STEFANIA\Desktop\Facultate\IA\PROIECT 2023\out.data"))
            {
                sw.WriteLine("Datele din populatie dupa ce au fost cuantificate.");
                for (int k = 0; k < lines.Length; k++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        sw.Write(valuesClasified[k, j] + " ");
                    }
                    sw.WriteLine();
                }
                Console.WriteLine("Am scris in fisierul out.data!");
            }

            //Clasificarea datelor in fisiere diferite in functie de recomandare (non-recom, recom, very-recom, priority
            using (StreamWriter sw = new StreamWriter(@"C:\Users\STEFANIA\Desktop\Facultate\IA\PROIECT 2023\outDataNonRecomm.data"))
            {
                sw.WriteLine("Populatia de date din categoria non-recommended.");
                sw.WriteLine("Numarul de populatii non-recomandate este " + NonRecomLines);
                for (int t = 0; t < valuesNonRecommend.GetLength(0); t++)
                {
                    for (int s = 0; s < valuesNonRecommend.GetLength(1); s++)
                    {
                        sw.Write(valuesNonRecommend[t, s] + " ");
                    }
                    sw.WriteLine();
                }
                Console.WriteLine("Am scris in fisierul outDataNonRecomm.data!");
            } //end using 

            using (StreamWriter sw = new StreamWriter(@"C:\Users\STEFANIA\Desktop\Facultate\IA\PROIECT 2023\outDataRecomm.data"))
            {
                sw.WriteLine("Populatia de date din categoria recommended.");
                sw.WriteLine("Numarul de populatii recomandate este " + NonRecomLines);
                for (int t = 0; t < valuesRecommend.GetLength(0); t++)
                {
                    for (int s = 0; s < valuesRecommend.GetLength(1); s++)
                    {
                        sw.Write(valuesRecommend[t, s] + " ");
                    }
                    sw.WriteLine();
                }
                Console.WriteLine("Am scris in fisierul outDataRecomm.data!");
            } //end using

            using (StreamWriter sw = new StreamWriter(@"C:\Users\STEFANIA\Desktop\Facultate\IA\PROIECT 2023\outDataVeryRecomm.data"))
            {
                sw.WriteLine("Populatia de date din categoria very recommended.");
                sw.WriteLine("Numarul de populatii recomandate este " + VeryRecomLines);
                for (int t = 0; t < valuesVeryRecommend.GetLength(0); t++)
                {
                    for (int s = 0; s < valuesVeryRecommend.GetLength(1); s++)
                    {
                        sw.Write(valuesVeryRecommend[t, s] + " ");
                    }
                    sw.WriteLine();
                }
                Console.WriteLine("Am scris in fisierul outDataVeryRecomm.data!");
            } //end using

            using (StreamWriter sw = new StreamWriter(@"C:\Users\STEFANIA\Desktop\Facultate\IA\PROIECT 2023\outDataPriority.data"))
            {
                sw.WriteLine("Populatia de date din categoria priority.");
                sw.WriteLine("Numarul de populatii recomandate este " + PriorityLines);
                for (int t = 0; t < valuesPriority.GetLength(0); t++)
                {
                    for (int s = 0; s < valuesPriority.GetLength(1); s++)
                    {
                        sw.Write(valuesPriority[t, s] + " ");
                    }
                    sw.WriteLine();
                }
                Console.WriteLine("Am scris in fisierul outDataPriority.data!");
            } //end using
            Console.WriteLine("--------------------------------------------------------------------------------");
        }

        public void procent()
        {
            int ProcentNonRecomLines;
            int ProcentRecomLines;
            int ProcentVeryRecomLines;
            int ProcentPriorityLines;

            ProcentNonRecomLines = (int)((double)(NonRecomLines * 100) / (double)(noInstances));
            ProcentRecomLines = (int)((double)(RecomLines * 100) / (double)(noInstances));
            ProcentVeryRecomLines = (int)((double)(VeryRecomLines * 100) / (double)(noInstances));
            ProcentPriorityLines = (int)((double)(PriorityLines * 100) / (double)(noInstances));

            Console.WriteLine("Procentajul pentru gradinitele nerecomandate: " + ProcentNonRecomLines + "%.");
            Console.WriteLine("Procentajul pentru gradinitele recomandate: " + ProcentRecomLines + "%.");
            Console.WriteLine("Procentajul pentru gradinitele foarte recomandate: " + ProcentVeryRecomLines + "%.");
            Console.WriteLine("Procentajul pentru gradinitele prioritare: " + ProcentPriorityLines + "%.");
            Console.WriteLine("--------------------------------------------------------------------------------");
        }
    }
}
