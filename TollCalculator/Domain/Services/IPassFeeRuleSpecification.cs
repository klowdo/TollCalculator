using TollCalculator.Domain.Models;

namespace TollCalculator.Domain.Services
{
    public interface IPassFeeRuleSpecification:ISpecification<Occurrence>
    {
        Money Fee { get; }
    }
}
