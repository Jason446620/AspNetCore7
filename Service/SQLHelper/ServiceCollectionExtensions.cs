using Microsoft.Extensions.DependencyInjection;
using Service.IService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.SQLHelper
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterSqlHelper(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            services.AddSingleton<ISqlHelperService, SqlHelperService>(sp => new SqlHelperService(connectionString));
        }
    }
}