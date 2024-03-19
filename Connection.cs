using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace EmpresaZ.Dynamics365.Query
{
    public class Connection
    {

        public static IOrganizationService Service { get; set; }

        public static IOrganizationService GetService()
        {
            if (Service == null) {

                var url = "https://org9a10be3e.crm2.dynamics.com";
                var user = "ElissandroVieira@TeofiloVieira.onmicrosoft.com";
                var password = "Luanavieir4.";

                CrmServiceClient crmServiceClient = new CrmServiceClient(
                    "AuthType=Office365;" +
                    $"Url={url};" +
                    $"Username={user};" +
                    $"Password={password}"
                );

                Service = crmServiceClient.OrganizationWebProxyClient;
            }
            

            return Service;
        }

    }
}
