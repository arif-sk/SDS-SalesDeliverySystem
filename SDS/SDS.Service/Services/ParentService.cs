using Caretaskr.Domain.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDS.Service.Services
{
    public class ParentService
    {
        protected readonly IGenericUnitOfWork _genericUnitOfWork;

        public ParentService(IGenericUnitOfWork genericUnitOfWork)
        {
            _genericUnitOfWork = genericUnitOfWork;
        }

    }
}
