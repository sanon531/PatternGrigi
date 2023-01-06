namespace PG.Battle
{
    public interface IObjectPoolSW<T> where T : class
    {
        int CountLeft { get; }

        void FillStack();

        T PickUp();
        
        void SetBack(T element);
    }
}