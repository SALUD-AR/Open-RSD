using System.Reflection;

namespace Msn.InteropDemo.AppServices.Implementation.Mapping
{
    public static class AutoMapperConfiguration
    {
        private static AutoMapper.MapperConfiguration config = null;
        private static bool _isConfiured = false;
        private static object configLock = new object();
        private static object mapperLock = new object();

        public static void Initialize()
        {
            if (_isConfiured) return;

            lock (configLock)
            {
                if (_isConfiured) return;

                config = new AutoMapper.MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

                _isConfiured = true;
            }
        }

        public static AutoMapper.IMapper CreateMapper()
        {
            lock (mapperLock)
            {
                if (!_isConfiured)
                {
                    Initialize();
                }

                return config.CreateMapper();
            }
        }

    }
}
