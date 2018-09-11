using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotoLogic;
using TotoPlayer;

namespace DistributionCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // TotoBet[] Bets  = new TotoBet[TotoConstants.ResultsCount];
        TotoConnector totoConnector = new TotoConnector();
            var pool = totoConnector.GetTotoPool("145");
            pool.Evaluate();
            pool.AddBet();
            pool.RemoveBet();
            /*
            double[,] probs = new double[,] 
            { 
                { 0.20, 0.25, 0.20, 0.35 },
                { 0.38, 0.30, 0.14, 0.18 },
                { 0.31, 0.24, 0.21, 0.24 },
                { 0.30, 0.24, 0.32, 0.14 },
                { 0.18, 0.21, 0.39, 0.22 },
                { 0.28, 0.15, 0.25, 0.32 },
                { 0.34, 0.35, 0.13, 0.18 },
                { 0.38, 0.05, 0.34, 0.23 },
                { 0.40, 0.20, 0.13, 0.27 },
                { 0.17, 0.23, 0.37, 0.23 },
                { 0.17, 0.20, 0.13, 0.50 },
                { 0.26, 0.26, 0.12, 0.36 },
                { 0.23, 0.34, 0.15, 0.28 },
                { 0.25, 0.37, 0.14, 0.24 },
                { 0.29, 0.09, 0.31, 0.31 },
            };

            double[,] pools = new double[,]
            {
                { 0.20, 0.25, 0.21, 0.34 },
                { 0.36, 0.30, 0.15, 0.19 },
                { 0.30, 0.24, 0.22, 0.24 },
                { 0.30, 0.24, 0.32, 0.14 },
                { 0.20, 0.21, 0.37, 0.22 },
                { 0.26, 0.16, 0.25, 0.33 },
                { 0.31, 0.34, 0.15, 0.20 },
                { 0.31, 0.17, 0.29, 0.23 },
                { 0.36, 0.20, 0.15, 0.29 },
                { 0.17, 0.23, 0.36, 0.24 },
                { 0.23, 0.19, 0.15, 0.43 },
                { 0.25, 0.26, 0.13, 0.36 },
                { 0.23, 0.33, 0.17, 0.27 },
                { 0.26, 0.37, 0.15, 0.22 },
                { 0.27, 0.16, 0.27, 0.30 },
            };

            int[] mod = { 1, 4, 11 };

            TotoEvaluator totoEvaluator = new TotoEvaluator(probs, pools, mod);


            for (int i = 0; i < 15; i++)
                Console.WriteLine($"{totoEvaluator.EVs[i, 0]:P3}   {totoEvaluator.EVs[i, 1]:P3} \n" +
                    $"{totoEvaluator.EVs[i, 2]:P3}   {totoEvaluator.EVs[i, 3]:P3} \n" +
                    $"----------------------------");
            */
            /*
              var probabilities = Enumerable.Range(0, 15).Select(e => 0.35).ToArray();
          //  double[] probabilities = { 0.2, 0.25, 0.25, 0.2, 0.2, 0.2, 0.2, 0.12, 0.25, 0.25, 0.28, 0.22, 0.13, 0.28, 0.24 };
            //decimal[] probabilities = { 0.2m, 0.25m, 0.25m, 0.01m, 0.01m, 0.01m, 0.01m, 0.12m, 0.25m, 0.25m, 0.28m, 0.22m, 0.13m, 0.28m, 0.24m };
            int[] mod = { 0, 1, 2 };
          //  int[] mod = { 0, 0, 0 };
            GameEvaluator ge = new GameEvaluator(probabilities, mod);

            double ev = 0;

            for(int i = 0; i < 15; i++)
            {
                ev += ge.Profits[i, 0] * (1 - probabilities[i]);
                ev += ge.Profits[i, 1] * probabilities[i];
            }

            ev /= 15;

            foreach(var n in ge.Profits)
            {
                Console.WriteLine(n);
            }*/

            Console.ReadLine();
            /* double[] result = new double[16];


             Stopwatch stopWatch = new Stopwatch();
             stopWatch.Start();
             for (int i = 0; i < 10000; i++)
             {
                 result = new double[16];
                 probFunc(0, 0, 1);
             }
             stopWatch.Stop();
             // Get the elapsed time as a TimeSpan value.
             TimeSpan ts = stopWatch.Elapsed;

             // Format and display the TimeSpan value.
             string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                 ts.Hours, ts.Minutes, ts.Seconds,
                 ts.Milliseconds / 10);
             Console.WriteLine("RunTime " + elapsedTime);



             Console.ReadLine();

             void probFunc(int currentIndex, int count, double prob)
             {
                 if (currentIndex < probabilities.Length)
                 {
                     probFunc(currentIndex + 1, count + 1, probabilities[currentIndex] * prob);
                     probFunc(currentIndex + 1, count, (1 - probabilities[currentIndex]) * prob);
                 }

                 else result[count] += prob;
             }*/
        }
    }
}
