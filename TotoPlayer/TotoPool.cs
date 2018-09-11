using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotoLogic;
using MoreLinq;

namespace TotoPlayer
{
    public class TotoPool : PropertyChangedBase
    {
        public string ID { get; set; }
        public TotoMatch[] Matches { get; set; } = new TotoMatch[TotoConstants.MatchesCount];
        public int Multiplier { get; set; } = 1;

        private int selected = 0;
        public int Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged();
            }
        }

        private int combinations = 0;
        public int Combination
        {
            get => combinations;
            set
            {
                combinations = value;
                OnPropertyChanged();
            }
        }

        private double cost = 0;
        public double Cost
        {
            get => cost;
            set
            {
                cost = value;
                OnPropertyChanged();
            }
        }

        public void InitializeBetsList()
        {
            foreach (var match in Matches)
                foreach (var bet in match.Bets)
                    bet.SelectedChanged += (sender, e) => UpdateCombinations();
        }

        private void UpdateCombinations()
        {
            var res = 1;
            foreach (var match in Matches)
                res *= match.Bets.Count(e => e.Selected);
            Combination = res;
            Cost = Combination * TotoConstants.CombinationPrice;
        }

        public void Evaluate()
        {
            double[,] probabilities = new double[TotoConstants.MatchesCount, TotoConstants.ResultsCount];
            double[,] pools = new double[TotoConstants.MatchesCount, TotoConstants.ResultsCount];
            List<int> matchOfTheDayIndexes = new List<int>();

            for (int i = 0; i < TotoConstants.MatchesCount; i++)
            {
                if (Matches[i].IsSpecial) matchOfTheDayIndexes.Add(i);
                for (int n = 0; n < TotoConstants.ResultsCount; n++)
                {
                    probabilities[i, n] = Matches[i].Bets[n].Probability;
                    pools[i, n] = Matches[i].Bets[n].Pool;
                }
            }
            
            TotoEvaluator totoEvaluator = new TotoEvaluator(probabilities, pools, matchOfTheDayIndexes);

            for (int i = 0; i < TotoConstants.MatchesCount; i++)
            {
                int bestIndex = 0;
                for (int n = 0; n < TotoConstants.ResultsCount; n++)
                {
                    Matches[i].Bets[n].EV = totoEvaluator.EVs[i, n];
                    if (Matches[i].Bets[n].EV > Matches[i].Bets[bestIndex].EV) bestIndex = n;
                }
                Matches[i].Bets[bestIndex].IsBest = true;
                Matches[i].Bets[bestIndex].Selected = true;
            }

            Selected = 15;
        }

        public void AddBet()
        {
            TotoBet max = Matches.SelectMany(e => e.Bets).Where(e => !e.Selected).MaxBy(e => e.EV).SingleOrDefault();
            if (max == null) return;
            max.Selected = true;
            Selected++;
        }

        public void RemoveBet()
        {
            TotoBet min = Matches.SelectMany(e => e.Bets).Where(e => e.Selected && !e.IsBest).MinBy(e => e.EV).SingleOrDefault();
            if (min == null) return;
            min.Selected = false;
            Selected--;
        }
    }

}

