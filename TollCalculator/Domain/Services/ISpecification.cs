namespace TollCalculator.Domain.Services
{
    public interface ISpecification<in T>
    {
        bool IsSatisfied(T input);
    }
}
