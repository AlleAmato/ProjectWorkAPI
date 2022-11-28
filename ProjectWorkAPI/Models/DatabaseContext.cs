using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;

namespace ProjectWorkAPI.Models
{
    public class DatabaseContext : DatabaseEntities
    {
        public DatabaseContext() : base()
        {
            Configuration.LazyLoadingEnabled = false;
        }
    }
}