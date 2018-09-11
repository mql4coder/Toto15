using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TotoLogic
{
    public class TotoEvaluator
    {
       
        public double[,] EVs = new double[TotoConstants.MatchesCount, TotoConstants.ResultsCount];
        private double[,] probabilities = new double[TotoConstants.MatchesCount, TotoConstants.ResultsCount];
        private double[,] pools = new double[TotoConstants.MatchesCount, TotoConstants.ResultsCount];
        private List<int> matchOfTheDayIndexes = new List<int>();

        public TotoEvaluator(double[,] probabilities, double[,] pools, List<int> matchOfTheDayIndexes)
        {
            this.probabilities = probabilities;
            this.pools = pools;
            this.matchOfTheDayIndexes = matchOfTheDayIndexes;
            SetEVs();
        }

        private const int EvaluateGamesCount = 20000;

        private void SetEVs()
        {
            Parallel.For(0, EvaluateGamesCount, index => EvaluateRandomGame(1.0 / EvaluateGamesCount));
            //for (int i = 0; i < EvaluateGamesCount; i++)
             //   EvaluateRandomGame(1.0 / EvaluateGamesCount);
        }
        private readonly object addlock = new object();
        private readonly object randlock = new object();
        private Random rand = new Random();
        private void EvaluateRandomGame(double prob)
        {
            int[] resultIndexes = new int[TotoConstants.MatchesCount];
            double[] gamepools = new double[TotoConstants.MatchesCount];
            for(int i=0; i < TotoConstants.MatchesCount; i++)
            {
                double sum = 0;
                var rndDouble = randSafe();
                while(sum + probabilities[i, resultIndexes[i]] < rndDouble) { sum += probabilities[i, resultIndexes[i]]; resultIndexes[i]++;  }
                gamepools[i] = pools[i, resultIndexes[i]];
            }

            var ge = new GameEvaluator(gamepools, matchOfTheDayIndexes);
            ge.SetProfits();
            for(int i=0; i< TotoConstants.MatchesCount; i++)
                for(int r = 0; r < TotoConstants.ResultsCount; r++)
                {
                    var ev = r == resultIndexes[i] ? ge.Profits[i, 1] : ge.Profits[i, 0];
                    ev -= 1;
                    lock (addlock)
                    {
                        EVs[i, r] += ev * prob;
                    }
                }

            double randSafe()
            {
                lock (randlock)
                {
                    return rand.NextDouble();
                }
            }
        }
    }
}
