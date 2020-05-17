using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MediaLibraryMobile.Enums;

namespace MediaLibraryMobile.Repository
{
    public class MainRepository
    {
        public static IDictionary<Pages, string> GetMenuItems()
        {
            return Enum.GetValues(typeof(Pages)).Cast<Pages>()
                                                .Select(item => new { Key = item, Value = item.ToString() })
                                                .ToDictionary(item => item.Key, item => item.Value);
        }
    }
}
