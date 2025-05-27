using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace CandyFactory
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Models.CandyFactoryModel> Factories { get; set; }
        public ICommand AddFactoryCommand { get; }
        public ICommand LoadSugarCommand { get; }

        private int _factoryCounter = 1;

        public MainViewModel()
        {
            Factories = new ObservableCollection<Models.CandyFactoryModel>();
            AddFactoryCommand = new RelayCommand(AddFactory);
            LoadSugarCommand = new RelayCommand(LoadSugar);

            // Подписка на события через рефлексию
            try
            {
                var factoryType = typeof(CandyProductionLine);
                var instance = Activator.CreateInstance(factoryType, "Фабрика 1");

                var eventInfo = factoryType.GetEvent("NeedSugar");
                var method = this.GetType().GetMethod("HandleSugarEvent");

                if (instance != null && eventInfo != null && method != null)
                {
                    var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType!, instance, method);
                    eventInfo.AddEventHandler(instance, handler);
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок рефлексии
            }
        }

        public void HandleSugarEvent(object? sender, string message)
        {
            // Обработка события
        }

        private void AddFactory(object? param)
        {
            var model = new Models.CandyFactoryModel
            {
                Name = $"Фабрика {_factoryCounter++}",
                SugarLevel = 100,
                Status = "Работает"
            };

            var factory = new CandyProductionLine(model.Name);
            factory.NeedSugar += (sender, message) =>
            {
                model.Status = "Нужен сахар";
            };
            factory.NeedRepair += (sender, message) =>
            {
                model.Status = "Авария";
            };

            Factories.Add(model);
        }

        private void LoadSugar(object? param)
        {
            if (param is Models.CandyFactoryModel model)
            {
                var factory = new CandyProductionLine(model.Name);
                factory.LoadSugar(50);
                model.SugarLevel = 50;
                model.Status = "Заправлен";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}