using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TollCalculator.Domain.Services;

namespace TollCalculator.Implementation
{
    public class FeeVisitor:IVisitor<VehicleTollContext>
    {
        private readonly IFeeRuleRepository _feeRuleRepository;

        public FeeVisitor(IFeeRuleRepository feeRuleRepository) {
            _feeRuleRepository = feeRuleRepository;
        }
        public void Visit(VehicleTollContext element) {
            foreach (var occurrence in element.Occurrences)
            {
                var matcingRule = _feeRuleRepository.GetAll()
                    .FirstOrDefault(spec => spec.IsSatisfied(occurrence));

                if (matcingRule != null)
                {
                    element.FeeOccurrence.Add((occurrence, matcingRule.Fee));
                }
            }
        }
    }
}
