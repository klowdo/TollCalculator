using System.Collections.Generic;

namespace TollCalculator.Domain.Services
{
    public interface IFeeRuleRepository
    {
        IList<IPassFeeRuleSpecification> GetAll();
    }
}
