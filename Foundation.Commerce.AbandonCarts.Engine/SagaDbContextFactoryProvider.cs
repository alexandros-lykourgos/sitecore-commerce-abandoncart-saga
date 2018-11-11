using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Commerce.AbandonCarts.Engine
{
    public class SagaDbContextFactoryProvider
    {
        private CommercePipelineExecutionContext _context;
        public SagaDbContextFactoryProvider(CommercePipelineExecutionContext context)
        {
            _context = context;
        }
        
        private string GetConnectionString()
        {
        
            try
            {
                var policy = _context.GetPolicy<Policies.AbandonCartsPolicy>();
                using (var connection = new SqlConnection(policy.ConnectionString))
                {
                    // It worked, we can save this as our connection string
                    return policy.ConnectionString;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string ConnectionString => GetConnectionString();
    }
}
