namespace TollCalculator.Implementation
{
    public interface IVisitor<in TElement>
    {
        void Visit(TElement element);
    }
}