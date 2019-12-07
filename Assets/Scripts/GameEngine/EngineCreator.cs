using MetaInfo;

namespace GameEngine
{
    public static class EngineCreator
    {
        public static Engine CreateEngine()
        {
            return new EngineImpl(StuffClass.IASA);
        }

        public static Engine CreateEngine(StuffClass stuffClass)
        {
            return new EngineImpl(stuffClass);
        }
    }
}