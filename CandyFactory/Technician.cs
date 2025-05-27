namespace CandyFactory
{
    public interface ITechnician
    {
        void HandleRepairEvent(object? sender, string message);
    }

    public class Technician : ITechnician
    {
        public void HandleRepairEvent(object? sender, string message)
        {
            // Реализация ремонта
        }
    }
}