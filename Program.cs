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
         
        private void Exercice()
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

                if (jobTitle == "Gerente de Vendas")
                {
                    EntityCollection accountFromContact = service.RetrieveMultiple(accountQuery);
                    var accountName = accountFromContact.Entities.First()["name"].ToString();

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

            System.Console.ReadKey();
        }
                          
        private void ExLinkEntitiesQE()
        {
            var service = Connection.GetService();

            QueryExpression query = new QueryExpression("contact");
            query.ColumnSet.AddColumn("fullname");

            query.AddLink("account", "parentcustomerid", "accountid", JoinOperator.Inner);

            query.LinkEntities[0].EntityAlias = "account";
            query.LinkEntities[0].LinkCriteria.AddCondition("alf_cnpj", ConditionOperator.Equal, "11.111.111/1111-11");

            EntityCollection accountContacts = service.RetrieveMultiple(query);

            for (var i = 0; i < accountContacts.Entities.Count(); i++)
            {

                var fullname = accountContacts.Entities[i]["fullname"].ToString();
                Console.WriteLine(fullname);
            }

            if (accountContacts.Entities.Count() == 0)
            {
                Console.WriteLine("Nenhum contato encontrado");
            }

            System.Console.ReadKey();
        }

        private void ExQueryExpression()
        {
            var service = Connection.GetService();

            QueryExpression query = new QueryExpression("account");
            query.ColumnSet.AddColumns("accountid", "name");
            query.Criteria.AddCondition("alf_cnpj", ConditionOperator.Equal, "00.000.000/0000-00");

            EntityCollection accountCollection = service.RetrieveMultiple(query);

            if (accountCollection.Entities.Count() > 0)
            {
                var name = accountCollection.Entities.First()["name"].ToString();

                Console.WriteLine(name);
                System.Console.ReadKey();

            }
            else
            {

                Console.WriteLine("Deu ruim");
                System.Console.ReadKey();
            }
        }

        private void ExLinq()
        {
            var service = Connection.GetService();

            var context = new OrganizationServiceContext(service);

            var accounts = from account in context.CreateQuery("account")
                           where (string)account["alf_cnpj"] == "00.000.000/0000-00"
                           select account;

            if (accounts.FirstOrDefault() != null)
            {
                var name = accounts.FirstOrDefault()["name"].ToString();
                Console.WriteLine(name);
            }
        }

        private void ExXml()
        {
            var service = Connection.GetService();

            string xml = @"
                <fetch top=""1"">
                   <entity name=""account"">
                      <attribute name=""name"" />
                      <filter>
                       <condition attribute=""alf_cnpj"" operator=""eq"" value=""00.000.000/0000-00"" />
                      </filter>
                   </entity>
                </fetch>
            ";

            service.RetrieveMultiple(new FetchExpression(xml));
        }
    }
}
