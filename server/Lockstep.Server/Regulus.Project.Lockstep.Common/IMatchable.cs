namespace Regulus.Project.Lockstep.Common
{
    public interface IMatchable
    {
        Regulus.Remoting.Value<int> Match();
    }
}