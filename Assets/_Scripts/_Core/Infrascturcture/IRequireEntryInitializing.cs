namespace GravityPong.Infrasturcture
{
    public interface IRequireEntryInitializing
    {
        void Initialize();
    }
    public interface IRequireEntryInitializing<TDependency>
    {
        void Initialize(TDependency dependency);
    }
    public interface IRequireEntryInitializing<TDependency1, TDependency2>
    {
        void Initialize(TDependency1 dependency1, TDependency2 dependency2);
    }
}