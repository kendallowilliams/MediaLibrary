using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Models.Interfaces
{
    public class ListItem<T_ID, T_Value> : IListItem<T_ID, T_Value>
    {
        public ListItem() { }

        public ListItem(T_ID id, string name, T_Value value)
        {
            ID = id;
            Name = name;
            Value = value;
        }

        public T_ID ID { get; set; }
        public string Name { get; set; }
        public T_Value Value { get; set; }
    }
}
