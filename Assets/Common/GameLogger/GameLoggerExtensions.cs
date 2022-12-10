using UnityEngine;

namespace SeedWork.GameLogs
{
    public static class GameLoggerExtensions 
    {
        public static GameLogger CreateLogger(this MonoBehaviour monoBehaviour, string name, LoggerOptions options = null)
        {
            var logger = new GameLogger(name, monoBehaviour.gameObject, options, GetGameTime);
            logger.Subscribe();
            return logger;
            // ToDo: Move to LoggingFactory (Yes, Logging, not Logger, because it is supposed to handle the LifeCycle for all factoried Loggers) perhaps there is a better name for this
            // LoggerLifeCycler?  _loggerLifeCycler.CreateLogger("StateMachine") _loggerLifeCycler["StateMachine"] idk ... 
        }

        public static float GetGameTime() => Time.time;
    }
}
