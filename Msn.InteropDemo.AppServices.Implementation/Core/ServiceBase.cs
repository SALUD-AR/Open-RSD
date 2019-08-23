using Msn.InteropDemo.AppServices.Implementation.Mapping;
using System;

namespace Msn.InteropDemo.AppServices.Implementation.Core
{
    public abstract class ServiceBase : IDisposable
    {
        private bool disposed;
        private AutoMapper.IMapper _mapper;

        public AutoMapper.IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    _mapper = AutoMapperConfiguration.CreateMapper();
                }

                return _mapper;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (disposed) return;

                disposed = true;

                _mapper = null;
            }
        }
    }
}
