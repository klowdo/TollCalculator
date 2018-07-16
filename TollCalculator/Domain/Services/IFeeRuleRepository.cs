using System.Collections.Generic;

namespace TollCalculator.Domain
{
    public interface IFeeRuleRepository
    {
        IList<IPassFeeRuleSpecification> GetAll();
    }
}
