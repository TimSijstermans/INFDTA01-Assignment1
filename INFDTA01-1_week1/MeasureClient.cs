namespace INFDTA01_1_week1
{
    class MeasureClient
    {
        private readonly IMeasureSimularity _simularityInterface;

        public MeasureClient(IMeasureSimularity strategy)
        {
            this._simularityInterface = strategy;
        }

        public double Measure(UserPreferences user1, UserPreferences user2)
        {
            return _simularityInterface.Measure(user1, user2);
        }
    }
}
