using System;
using System.Threading.Tasks;
using System.Windows;

namespace CandyFactory
{
    public class CandyProductionLine
    {
        public event EventHandler<string>? NeedSugar;
        public event EventHandler<string>? NeedRepair;
        public event EventHandler<string>? RepairFinished;
        public event EventHandler<int>? ProduceCandy;

        private Random _rnd = new Random();
        private int _sugarLevel = 100;
        private bool _isWorking = true;
        private readonly string _name;

        public string Name => _name;

        public int SugarLevel
        {
            get => _sugarLevel;
            private set => _sugarLevel = value;
        }

        public CandyProductionLine(string name)
        {
            _name = name;
            StartProduction();
        }

        private async void StartProduction()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(500);

                    if (_isWorking && _sugarLevel > 0)
                    {
                        _sugarLevel -= 5;
                        ProduceCandy?.Invoke(this, 50);
                    }

                    if (_sugarLevel <= 0)
                    {
                        NeedSugar?.Invoke(this, $"{_name}: Закончился сахар!");
                        _sugarLevel = 0;
                        _isWorking = false;
                    }

                    if (_sugarLevel > 0 && _rnd.Next(100) < 10)
                    {
                        NeedRepair?.Invoke(this, $"{_name}: Произошла авария!");
                        _isWorking = false;
                        StartAutoRepair();
                    }
                }
            });
        }

        private async void StartAutoRepair()
        {
            await Task.Delay(5000);
            FinishRepair();
        }

        public void LoadSugar(int amount)
        {
            _sugarLevel += amount;
            if (_sugarLevel > 100) _sugarLevel = 100;

            if (_sugarLevel > 0)
            {
                _isWorking = true;
            }
        }

        public void FinishRepair()
        {
            _isWorking = true;
            RepairFinished?.Invoke(this, $"{_name}: Ремонт завершён");
        }

        public void Dispose()
        {
            NeedSugar = null;
            NeedRepair = null;
            RepairFinished = null;
            ProduceCandy = null;
        }
    }
}