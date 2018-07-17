# TollCalculator

This is my implementation of [Toll fee calculator 1.0](https://github.com/EvolveTechnology/toll-calculator)

### My Thoughts

- To much is handled in the main function.
- The code is hard to read.
- The fee rules is hardcoded.
- Code lacks tests.
- The contract does not describe what is returned. For example decimal
- The Car and Motorbike implementations in this case does not add any benefits

## My strategies

- Make more testable.
- Break out fee rule and just narrow it down to the requirements of the TollCalculator.
- Make more extensible (TollFeeRules).
- Make Value types for better readability and encapsulate logic.
- Try to make the hardest logic more readable.
- when occurrences are within time range pick out max of these.
- Write test that cover enough parts.
- Create an generic Vehicle class as base

## My Thoughts after

- Im not that happy with the grouping of timerange, it is not that readable but it will do the job...
- An calulators job is to combine functions and out values depending on the input. im thinking of breaking out more of the logic and just use the calculator as a Factory that assemble all functions.
- Write tests that autogenerates range of tests. start middle and end of time range etc...
