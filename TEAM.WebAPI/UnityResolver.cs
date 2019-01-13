using log4net;

using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

using Unity;
using Unity.Exceptions;

namespace TEAM.WebAPI
{
    public class UnityResolver : IDependencyResolver
    {
        #region Private variable declarations.

        private readonly IUnityContainer _container;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UnityResolver));

        #endregion

        #region Constructors.

        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                _logger.Error("Container is null.");
                throw new ArgumentException("Container cannot be null.");
            }
            _container = container;
        }

        #endregion

        #region IDependencyResolver Interface Implementation.

        public IDependencyScope BeginScope()
        {
            IUnityContainer child = _container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException ex)
            {
                _logger.Error(ex);
                return new List<object>();
            }
        }

        #endregion

        #region Private method declarations.

        private void Dispose(bool disposing)
        {
            _container.Dispose();
        }

        #endregion
    }
}