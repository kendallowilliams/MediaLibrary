using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Models.Interfaces
{
    public interface IListItem<T_ID, T_Value>
    {
        T_ID ID { get; set; }
        string Name { get; set; }
        T_Value Value { get; set; }
        bool IsSelected { get; set; }
    }
}
