using Zem80.Core.Instructions;

namespace Zem80.Core.CPU
{
    public interface IReadOnlyFlags
    {
        bool Carry { get; }
        bool HalfCarry { get; }
        bool ParityOverflow { get; }
        bool Sign { get; }
        FlagState State { get; }
        bool Subtract { get; }
        byte Value { get; }
        bool X { get; }
        bool Y { get; }
        bool Zero { get; }

        Flags Clone();

        bool SatisfyCondition(Condition condition);
    }
}