using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.WPF.Common.Exceptions
{
    public class ThemeNotFoundException : Exception
    {
        public ThemeNotFoundException(string themeName) : base($"Theme with name \"{themeName}\" is not found!")
        {

        }
    }
}
