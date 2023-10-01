using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Synergy.WPF.Common.Extensions
{
    public static class NavigationExtensions
    {
        public static bool NavigateNoJournal(this NavigationService service, Func<object> rootGetter)
        {
            JournalEntry entry = null;

            service.Navigate(null);
            if(service.CanGoBack)
                entry = service.RemoveBackEntry();

            if(service.Navigate(rootGetter.Invoke()))
            {
                return true;
            }
            else if(entry != null)
            {
                service.Navigate(entry);
            }

            return false;
        }

        public static bool NavigateNoJournal(this Frame frame, Func<object> rootGetter)
        {
            return frame.NavigationService.NavigateNoJournal(rootGetter);
        }
    }
}
