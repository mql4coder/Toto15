using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotoLogic
{
    public class TotoBet : PropertyChangedBase
    {
        public TotoBet(double probability, double pool)
        {
            Probability = probability;
            Pool = pool;
        }
        
        public double Probability { get; set; }
        public double Pool { get; set; }
        public bool IsBest { get; set; }

        private double ev = 0;
        public double EV
        {
            get => ev;
            set
            {
                ev = value;
                OnPropertyChanged();
            }
        }

        private bool selected = false;
        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged();
                OnSelectedChanged();
            }
        }

        public event EventHandler<bool> SelectedChanged;
        private void OnSelectedChanged() => SelectedChanged?.Invoke(this, selected);
    }
}
