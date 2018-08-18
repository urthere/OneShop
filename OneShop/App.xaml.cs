using System;
using System.Configuration;
using System.Windows;
using System.Data.Entity.Core.EntityClient;
using System.Data.Common;

namespace OneShop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string _db_ConnectionString;
        public static string DBConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_db_ConnectionString))
                {
                    var originalConnectionString = ConfigurationManager.ConnectionStrings["OneShopEntities"].ConnectionString;
                    var entityBuilder = new EntityConnectionStringBuilder(originalConnectionString);
                    var factory = DbProviderFactories.GetFactory(entityBuilder.Provider);
                    var providerBuilder = factory.CreateConnectionStringBuilder();
                    providerBuilder.ConnectionString = entityBuilder.ProviderConnectionString;
                    providerBuilder.Add("Password", "oneshop");
                    _db_ConnectionString = providerBuilder.ToString();
                }
                return _db_ConnectionString;
            }
        }
    }
}
