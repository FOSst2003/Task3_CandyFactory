using CandyFactory.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CandyFactory
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int _totalCandyCount;
        public int TotalCandyCount
        {
            get => _totalCandyCount;
            set { _totalCandyCount = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CandyFactoryModel> Factories { get; set; }
        public ICommand AddFactoryCommand { get; }
        public ICommand LoadSugarCommand { get; }
        public ICommand RemoveFactoryCommand { get; }

        private int _factoryCounter = 1;

        public MainViewModel()
        {
            Factories = new ObservableCollection<CandyFactoryModel>();
            AddFactoryCommand = new RelayCommand(AddFactory);
            LoadSugarCommand = new RelayCommand(LoadSugar);
            RemoveFactoryCommand = new RelayCommand(RemoveFactory);

            AddFactory(null);
        }

        private void AddFactory(object? param)
        {
            var model = new CandyFactoryModel
            {
                Name = $"Фабрика {_factoryCounter++}",
                SugarLevel = 100,
                Status = "Работает",
                IsWorking = true
            };

            var factory = new CandyProductionLine(model.Name);
            model.Factories.Add(factory);

            factory.NeedSugar += (sender, message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    model.Status = "Нужен сахар";
                });
            };
            factory.NeedRepair += (sender, message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    model.Status = "Авария";
                });
            };
            factory.RepairFinished += (sender, message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    model.Status = "Работает";
                });
            };
            factory.ProduceCandy += (sender, count) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    model.CandyCount += count;
                    TotalCandyCount += count;
                });
            };

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(500);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var f in Factories)
                        {
                            foreach (var productionLine in f.Factories)
                            {
                                f.SugarLevel = productionLine.SugarLevel;
                            }
                        }
                    });
                }
            });

            Factories.Add(model);
        }

        private void LoadSugar(object? param)
        {
            if (param is CandyFactoryModel model)
            {
                foreach (var factory in model.Factories)
                {
                    factory.LoadSugar(100);
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    model.SugarLevel = 100;
                    model.Status = "Заправлен";
                });

                Task.Delay(1000).ContinueWith(t =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        model.Status = "Работает";
                    });
                });
            }
        }

        private void RemoveFactory(object? param)
        {
            if (param is CandyFactoryModel model)
            {
                foreach (var factory in model.Factories)
                {
                    factory.Dispose();
                }
                Factories.Remove(model);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}