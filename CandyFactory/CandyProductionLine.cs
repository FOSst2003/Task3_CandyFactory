using System;
using System.Threading.Tasks;

namespace CandyFactory
{
    public class CandyProductionLine
    {
        public event EventHandler<string>? NeedSugar;
        public event EventHandler<string>? NeedRepair;

        private Random _rnd = new Random();
        private int _sugarLevel = 100;
        private string _name = string.Empty;

        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        public CandyProductionLine(string name)
        {
            Name = name;
            StartProduction();
        }

        private async void StartProduction()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    _sugarLevel -= 5;

                    if (_sugarLevel <= 0)
                    {
                        NeedSugar?.Invoke(this, $"{Name}: Закончился сахар!");
                        _sugarLevel = 0;
                    }

                    if (_rnd.Next(100) < 5)
                    {
                        NeedRepair?.Invoke(this, $"{Name}: Произошла авария!");
                    }
                }
            });
        }

        public void LoadSugar(int amount)
        {
            _sugarLevel += amount;
        }
    }
}