using MetaInfo;
using System;

namespace GameEngine
{
    public static class EngineCreator
    {
        public static Engine CreateEngine()
        {
            return new EngineImpl();
        }

        public static Engine CreateEngine(StuffClass stuffClass)
        {
            throw new NotImplementedException();
        }
    }
}