using System.Collections.Generic;
using System.ComponentModel;

namespace CandyFactory.Models
{
    public class CandyFactoryModel : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private int _sugarLevel;
        private string _status = string.Empty;
        private int _candyCount;
        private bool _isWorking = true;

        public List<CandyProductionLine> Factories { get; } = new();

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public int SugarLevel
        {
            get => _sugarLevel;
            set { _sugarLevel = value; OnPropertyChanged(); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public int CandyCount
        {
            get => _candyCount;
            set { _candyCount = value; OnPropertyChanged(); }
        }

        public bool IsWorking
        {
            get => _isWorking;
            set { _isWorking = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}