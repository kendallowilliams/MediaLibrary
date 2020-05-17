using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryDAL.DbContexts
{
    public partial class MediaLibraryEntities
    {
        public MediaLibraryEntities(string nameOrConnectionString) : base() 
        {
        }
    }
}
