/**************************************************************************
 *                                                                        *
 *  Copyright:   (c) 2016-2020, Florin Leon                               *
 *  E-mail:      florin.leon@academic.tuiasi.ro                           *
 *  Website:     http://florinleon.byethost24.com/lab_ia.html             *
 *  Description: Evolutionary Algorithms                                  *
 *               (Artificial Intelligence lab 8)                          *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using System;

namespace SVMProject
{
    
    /// <summary>
    /// Clasa care reprezinta operatia de selectie
    /// </summary>
    public class Selection
    {
        private static Random _rand = new Random();

        public static Chromosome Tournament(Chromosome[] population)
        {
            //throw new Exception("Aceasta metoda trebuie implementata");
            int nr1 = _rand.Next(0, population.Length);
            int nr2 = _rand.Next(0, population.Length);
            while (nr1 == nr2)
                nr1 = _rand.Next(0, population.Length);
            if (population[nr1].Fitness > population[nr2].Fitness)
                return population[nr1];
            else
                return population[nr2];
        }

        public static Chromosome GetBest(Chromosome[] population)
        {
            // max dintre valorile returnate de ComputeFitness (conform formulei din Curs 12 / Carte (4.30)
            double max = population[0].Fitness;
            Chromosome rez = new Chromosome(population[0]);
            for (int i = 1; i < population.Length; i++)
                if (population[i].Fitness > max)
                {
                    max = population[i].Fitness;
                    rez = new Chromosome(population[i]);
                }

            return rez;
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care reprezinta operatia de incrucisare
    /// </summary>
    public class Crossover
    {
        private static Random _rand = new Random();

        public static Chromosome Arithmetic(Chromosome mother, Chromosome father, double rate)
        {
            //throw new Exception("Aceasta metoda trebuie implementata");
            double probabilitate = _rand.NextDouble();
            if (probabilitate >= rate)
            {
                probabilitate = _rand.NextDouble();
                if (probabilitate < 0.5)
                    return mother;
                else
                    return father;
            }

            double alfa = _rand.NextDouble();
            Chromosome child = new Chromosome(mother);
            for (int i = 0; i < child.Genes.Length; ++i)
                child.Genes[i] = alfa * mother.Genes[i] + (1 - alfa) * father.Genes[i];
            return child;
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care reprezinta operatia de mutatie
    /// </summary>
    public class Mutation
    {
        private static Random _rand = new Random();

        public static void Reset(Chromosome child, double rate)
        {
            //throw new Exception("Aceasta metoda trebuie implementata");
            for (int i = 0; i < child.Genes.Length; i++)
            {
                double probabilitate = _rand.NextDouble();
                if (rate > probabilitate)
                {
                    child.Genes[i] = child.MinValues[i] + probabilitate * (child.MaxValues[i] - child.MinValues[i]);
                }
            }
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care implementeaza algoritmul evolutiv pentru optimizare
    /// </summary>
    public class EvolutionaryAlgorithm
    {
        /// <summary>
        /// Metoda de optimizare care gaseste solutia problemei
        /// </summary>
        public Chromosome Solve(IOptimizationProblem p, int populationSize, int maxGenerations, double crossoverRate, double mutationRate, int[,] inputValues)
        {
            // throw new Exception("Aceasta metoda trebuie completata");

            Chromosome[] population = new Chromosome[populationSize];
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = p.MakeChromosome();
                p.ComputeFitness(population[i], inputValues);
            }

            for (int gen = 0; gen < maxGenerations; gen++)
            {
                Chromosome[] newPopulation = new Chromosome[populationSize];
                newPopulation[0] = Selection.GetBest(population); // elitism

                for (int i = 1; i < 120; i++)
                {
                    // selectare 2 parinti: Selection.Tournament
                    Chromosome mama = Selection.Tournament(population);
                    Chromosome tata = Selection.Tournament(population);
                    while (mama == tata)
                        mama = Selection.Tournament(population);

                    // generarea unui copil prin aplicare crossover: Crossover.Arithmetic
                    Chromosome copil = Crossover.Arithmetic(mama, tata, crossoverRate);

                    // aplicare mutatie asupra copilului: Mutation.Reset
                    Mutation.Reset(copil, mutationRate);

                    // calculare fitness pentru copil: ComputeFitness din problema p
                    p.ComputeFitness(copil, inputValues);

                    // introducere copil in newPopulation
                    newPopulation[i] = copil;
                }
                for (int i = 0; i < populationSize; i++)
                    population[i] = newPopulation[i];
                Console.Write("{0}% \r", (gen + 1));
            }

            return Selection.GetBest(population);
            //Console.WriteLine("Sunt in Solve");
            }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care reprezinta problema din prima aplicatie: rezolvarea ecuatiei
    /// </summary>
    public class Equation : IOptimizationProblem
    {
        public Chromosome MakeChromosome()
        {
            int nrLines = 120;
            double C = 120;

            // un cromozom are 120 de gene  care poate lua valori in intervalul (0, C)
            return new Chromosome(nrLines, Enumerable.Repeat<double>(0.0, nrLines).ToArray(), Enumerable.Repeat<double>(C, nrLines).ToArray());
        }

        public void ComputeFitness(Chromosome c, int[,] inputValues)
        {
            //folosim nucleul polinomial de grad 2 - Curs 12 pagina 50
            int[] xi = new int[8];   // 8 atribute (fara ultimul)
            int[] xj = new int[8];   // vectori in care vom avea valorile din inputValues pentru a le transmite ca paramtru in functia calculata de Kernel

 
            double sumaAlpha = 0;   // prima suma
            double s2 = 0;
            Kernel kernel = new Kernel();


            for (int i = 0; i < c.NoGenes; i++)
            {
                sumaAlpha += c.Genes[i];    //prima suma pentru alpha[i]
            }
            for (int i = 0; i < c.NoGenes; i++)
            {
                for (int j = 0; j < c.NoGenes; j++)
                {
                    for (int t = 0; t < 8; t++)
                    {
                        xi[t] = inputValues[i, t]; // primul parametru din functia PolinomialKernel trebuie sa fie x[i] conform formulei matematice 
                                                  // x[i] = x = inputValues[i, 8]. Deoarece parametru functiei este de tip vector, am copiat valorile intr-un vector x
                        xj[t] = inputValues[j, t]; // al doilea parametru din functia PolinomialKernel trebuie sa fie x[j] conform formulei matematice 
                                                  // x[j] = inputValues[j, 8]. Deoarece parametru functiei este de tip vector, am copiat valorile intr-un vector x
                    }
                    //c.Genes = alpha, inputValues = y conform formulei matematice din Cursul 12 pag 50
                    s2 += c.Genes[i] * c.Genes[j] * inputValues[i, 8] * inputValues[j, 8] * kernel.PolinomialKernel(xi, xj);
                }
            }
            c.Fitness = sumaAlpha - (1 / 2) * s2;
            //Console.WriteLine("Sunt in ComputeFitness");
        }
    }


    public class Kernel
    {
        public int PolinomialKernel(int[] xi, int[] xj) //Folosim nucleul polinomial de grad 2
        {
            int k = 0;
            for (int i = 0; i < xi.Length; i++)
            {
                k += xi[i] * xj[i];
            }
            return (k + 1) * (k + 1);
        }
    }

    //==================================================================================

    /// <summary>
    /// Programul principal care apeleaza algoritmul
    /// </summary>
    public class Program
    {
        private static void Main(string[] args)
        {


            DataManipulation extract = new DataManipulation();
            extract.fileRead();
            extract.writeInFile();
            extract.procent();

            int[,] valuesClasified = extract.valuesClasified;
            int[,] valuesNonRecommend = extract.valuesNonRecommend;
            int[,] valuesRecommend = extract.valuesRecommend;
            int[,] valuesVeryRecommend = extract.valuesVeryRecommend;
            int[,] valuesPriority = extract.valuesPriority;


            
            EvolutionaryAlgorithm ea = new EvolutionaryAlgorithm();
           Chromosome solution = ea.Solve(new Equation(), 120, 100, 0.95, 0.2, valuesClasified);

            int nrInstante = 120;
            double[] alfa = new double[nrInstante]; //nr de instante = 120
            Console.WriteLine("Coeficientii alfa sunt ");
            for(int i = 0; i< nrInstante; i++)
            {
                alfa[i] = solution.Genes[i];
                Console.WriteLine("Pe linia " + i + " avem coeficientul alpha " + alfa[i] + " ");
            }


            Console.WriteLine("--------------------------------------------------------------------------------");
            double[] w = new double[nrInstante];
            for (int i = 0; i < nrInstante; ++i)
            {
                w[i] = alfa[i] * valuesClasified[i, 8];
            }
            
            Console.WriteLine("Coeficientul w este ");
            for(int i = 0; i< nrInstante; i++)
            {
                Console.WriteLine("Pe linia " + i + " avem coeficientul w " + w[i] + " ");
            }
                Console.WriteLine("--------------------------------------------------------------------------------");
            int[] xi = new int[8];   // 8 atribute (fara ultimul)
            int[] xj = new int[8];   // vectori in care vom avea valorile din inputValues pentru a le transmite ca paramtru in functia calculata de Kernel

            // BIAS
            double bias = 0;
            double s2 = 0;
            Kernel kernel = new Kernel();

            for (int i = 0; i < nrInstante; i++)
            {
                for (int j = 0; j < nrInstante; j++)
                {
                    for (int t = 0; t < 8; t++)
                    {
                        xi[t] = valuesClasified[i, t]; // primul parametru din functia PolinomialKernel trebuie sa fie x[i] conform formulei matematice 
                                                   // x[i] = x = inputValues[i, 8]. Deoarece parametru functiei este de tip vector, am copiat valorile intr-un vector x
                        xj[t] = valuesClasified[j, t]; // al doilea parametru din functia PolinomialKernel trebuie sa fie x[j] conform formulei matematice 
                                                   // x[j] = inputValues[j, 8]. Deoarece parametru functiei este de tip vector, am copiat valorile intr-un vector x
                    }
                    //c.Genes = alpha, inputValues = y conform formulei matematice din Cursul 12 pag 50
                    s2 +=  valuesClasified[j, 8] * alfa[j] * kernel.PolinomialKernel(xi, xj);
                }
                bias += valuesClasified[i, 8] - s2;
            }
            bias /= nrInstante;
            Console.WriteLine("Bias este " + bias);
        } //end main 



    }


}// end namespace

