namespace Zem80.Core.Debugger
{
    public enum DebugResponseType
    {
        StepNext,
        Run,
        Stop,
        None
    }

    public class DebugResponse
    {
        public static DebugResponse StepNext() => new DebugResponse(DebugResponseType.StepNext);
        public static DebugResponse RunTo(ushort runToAddress) => new DebugResponse(DebugResponseType.Run, runToAddress);
        public static DebugResponse Stop() => new DebugResponse(DebugResponseType.Stop);
        public static DebugResponse None => new DebugResponse(DebugResponseType.None);

        public DebugResponseType Type { get; private set; }
        public ushort? RunToAddress { get; private set; }

        private DebugResponse(DebugResponseType type, ushort? runToAddress = null)
        {
            Type = type;
            RunToAddress = runToAddress;
        }
    }
}
