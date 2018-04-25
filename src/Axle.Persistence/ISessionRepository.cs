namespace Axle.Persistence 
{
    public interface ISessionRepository
    {
        void AddSession(string sessionId, string userId);
        string GetSession(string sessionId);
        void RemoveSession(string sessionId);
    }
}