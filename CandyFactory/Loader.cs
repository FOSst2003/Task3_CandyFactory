namespace CandyFactory
{
    public class Loader
    {
        public void HandleSugarEvent(object? sender, string message)
        {
            if (sender is CandyProductionLine factory)
            {
                factory.LoadSugar(50);
            }
        }
    }
}