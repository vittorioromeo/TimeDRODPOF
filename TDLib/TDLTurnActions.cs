namespace TimeDRODPOF.TDLib
{
    public static class TDLTurnActions
    {
        public static readonly int[] Priorities = new[] {-100, -75, -50, 10, 50, 100};

        public static readonly int Start = Priorities[0];
        public static readonly int StartBrainCheck = Priorities[1];
        public static readonly int BeforeManager = Priorities[2];
        public static readonly int AfterManager = Priorities[3];
        public static readonly int EndChecks = Priorities[4];
        public static readonly int End = Priorities[5];
    }
}