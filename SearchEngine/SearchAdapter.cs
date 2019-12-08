using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    interface SearchAdapter
    {
        bool Search(string FullPath, string SearchName, bool DoSearchByContent, string SearchContent);
    }
}
