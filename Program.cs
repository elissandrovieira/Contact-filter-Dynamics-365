using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace EmpresaZ.Dynamics365.Query
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var service = Connection.GetService();

            QueryExpression query = new QueryExpression("contact");
            query.ColumnSet.AddColumns("fullname", "parentcustomerid", "jobtitle");

            query.AddLink("account", "parentcustomerid", "accountid", JoinOperator.Inner);

            query.LinkEntities[0].EntityAlias = "account";
            query.LinkEntities[0].LinkCriteria.AddCondition("industrycode", ConditionOperator.Equal, "34");

            EntityCollection accountContacts = service.RetrieveMultiple(query);

            for (var i = 0; i < accountContacts.Entities.Count(); i++)
            {

                var fullname = accountContacts.Entities[i]["fullname"].ToString();
                var jobTitle = accountContacts.Entities[i]["jobtitle"].ToString();
                EntityReference refAccount = (EntityReference)accountContacts.Entities[i]["parentcustomerid"];


                QueryExpression accountQuery = new QueryExpression("account");
                accountQuery.ColumnSet.AddColumns("name", "industrycode");
                accountQuery.Criteria.AddCondition("accountid", ConditionOperator.Equal, refAccount.Id);
                EntityCollection accountFromContact = service.RetrieveMultiple(accountQuery);

                var accountName = accountFromContact.Entities.First()["name"].ToString();

                if (jobTitle == "Gerente de Vendas")
                {
                 
                    Console.WriteLine($"Nome da Conta: {accountName}");
                    Console.WriteLine($"Indústria: Manufatura");
                    Console.WriteLine($"Nome do contato relacionado: {fullname}");
                    Console.WriteLine($"Cargo: {jobTitle}");
                    Console.WriteLine($"--------------------------------------------------------------------");
                }
            }

            if (accountContacts.Entities.Count() == 0)
            {
                Console.WriteLine("Nenhum contato encontrado");
            }

            Console.ReadKey();


        }
    }
}
