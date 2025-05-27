namespace CandyFactory
{
    public interface ICandyFactory
    {
        string Name { get; }
        void LoadSugar(int amount);
    }
}