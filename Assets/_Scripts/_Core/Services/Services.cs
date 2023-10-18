using System;
using System.Collections.Generic;

namespace GravityPong
{
    public class Services
    {
        public static Services Instance { get; private set; }

        private static Dictionary<Type, IService> _services;

        public Services()
        {
            Instance = this;

            _services = new Dictionary<Type, IService>();
        }

        public void Register<TService>(IService obj) where TService : class, IService
        {
            if(!_services.ContainsKey(typeof(TService)))
            {
                _services.Add(typeof(TService), obj);
            }
        }
        public TService Get<TService>() where TService : class, IService
        {
            if(_services.TryGetValue(typeof(TService), out IService service))
            {
                return service as TService;
            }

            return null;
        }
    }
}
