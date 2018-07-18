namespace TollCalculator.Implementation
{
    public interface IAcceptVisitor<out TElement>
    {
        void Accept(IVisitor<TElement> visitor);
    }
}