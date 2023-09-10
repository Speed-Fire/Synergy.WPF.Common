using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Synergy.WPF.Common.Utility;

namespace Synergy.WPF.Common.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection RegisterSynergyWPFCommon(this IServiceCollection services)
        {
            services.AddSingleton<AppThemeController>();

            return services;
        }
    }
}
