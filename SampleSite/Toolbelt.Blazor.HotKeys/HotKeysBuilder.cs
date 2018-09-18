using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolbelt.Blazor.HotKeys
{
    public class HotKeysBuilder
    {
        internal HotKeysBuilder()
        {
        }

        public HotKeysContext CreateContext()
        {
            return new HotKeysContext();
        }
    }
}
