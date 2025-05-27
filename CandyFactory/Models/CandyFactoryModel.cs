using System.ComponentModel;

namespace CandyFactory.Models
{
    public class CandyFactoryModel : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private int _sugarLevel;
        private string _status = string.Empty;

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

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}