using TollCalculator.Domain.Models;

namespace TollCalculator.Domain.Services
{
    public interface IPassFeeRuleSpecification:ISpecification<PassBy>
    {
        Money Fee { get; }
    }
}
