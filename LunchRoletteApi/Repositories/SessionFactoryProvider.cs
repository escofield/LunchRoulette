using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using System.Web;

namespace LunchRoletteApi.Repositories
{

    public static class RepoMeta
    {
        public static string connectionString = "data source=dev1;User id=jj07035;Password=reem7035;";
        //public static string connectionString = "Data Source=localhost:1521/Default;Persist Security Info=True;User ID=LunchRoulette;Password=Default1;";
    }
    public class SessionFactoryProvider
    {
        class SqlStatementInterceptor : EmptyInterceptor
        {
            public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
            {
                System.Diagnostics.Debug.WriteLine(sql);
                return sql;
            }
        }

        private static readonly object LockObject = new object();
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                lock (LockObject)
                {
                    return _sessionFactory ?? (_sessionFactory = Build());
                }
            }
        }

        private static string CurrentSessionContextClass
        {
            get
            {
                return
                    HttpContext.Current == null
                        ? "call"
                        : "web";
            }
        }

        /// <summary>
        ///     Creates the SessionFactory with all required configurations specific to Oracle Database.
        /// </summary>
        /// <returns>
        ///     The SessionFactory object.
        /// </returns>
        public static ISessionFactory Build()
        {
            // Create a new configuration
            var configuration = new Configuration();
            // Uncomment this line to output the sql in Visual Studio's debug output window
            //configuration.SetInterceptor(new SqlStatementInterceptor());
            configuration.DataBaseIntegration(db =>
            {
                db.ConnectionString = RepoMeta.connectionString;
                db.Driver<OracleClientDriver>();
                db.Dialect<Oracle10gDialect>();
                db.LogSqlInConsole = false; /* If you need to debug queries you can set this property to "True" */
            })
            .SessionFactory();

            // Configure the Current Session Context
            //  In order to implement session management in ASP.Net NHibernate uses a property called "current_session_context_class"
            //  A value "web" tells NHibernate to store an ISession object in the HttpContext.Items collection.
            //  NOTES:  If you need to use the session in your Unit Testing create app setting with key = "CurrentSessionContextClass"
            //          and value = "call".
            configuration.SetProperty("current_session_context_class", CurrentSessionContextClass);

            // Add Model Mappings
            
            configuration.AddMapping(ModelMapperRapper.CompileMapping());

            return configuration.BuildSessionFactory();
        }

        /// <summary>
        ///     Gets the current session.
        /// </summary>
        /// <returns>
        ///     The current session.
        /// </returns>
        public static ISession GetCurrentSession()
        {
            try
            {
                // If there is no session bound to the current session context, bind it!!!
                if (!CurrentSessionContext.HasBind(SessionFactory))
                    CurrentSessionContext.Bind(SessionFactory.OpenSession());

                return SessionFactory.GetCurrentSession();
            }
            catch
            {
                // If error is thrown, unbind the old session and bind a new one!!!
                CurrentSessionContext.Unbind(SessionFactory);
                CurrentSessionContext.Bind(SessionFactory.OpenSession());

                return SessionFactory.GetCurrentSession();
            }
        }

        /// <summary>
        ///     Begins a database transaction.
        /// </summary>
        /// <remarks>
        ///     You will need to implement Action Filter to begin and use one transaction per action.
        ///     ActionFilterAttribute - OnActionExecuting is an ideal place to call this method.
        /// </remarks>
        public static void BeginTransaction()
        {
            GetCurrentSession().BeginTransaction();
        }

        /// <summary>
        ///     Commits a database transaction.
        /// </summary>
        /// <remarks>
        ///     You will need to implement Action Filter to commit the underlying transaction.
        ///     ActionFilterAttribute - OnActionExecuted is an ideal place to call this method.
        /// </remarks>
        public static void CommitTransaction()
        {
            using (var session = CurrentSessionContext.Unbind(SessionFactory))
            {
                if (session == null) return;
                if (session.Transaction == null) return;
                if (!session.Transaction.IsActive) return;
                session.Transaction.Commit();
            }
        }

        /// <summary>
        ///     Forces the database transaction to roll back.
        /// </summary>
        /// <remarks>
        ///     You will need to implement Exception Filter to roll back from the underlying transaction when exception occurs.
        ///     ExceptionFilterAttribute - OnException is an ideal place to call this method.
        /// </remarks>
        public static void RollbackTransaction()
        {
            using (var session = CurrentSessionContext.Unbind(SessionFactory))
            {
                if (session == null) return;
                if (session.Transaction == null) return;
                if (!session.Transaction.IsActive) return;
                session.Transaction.Rollback();
            }
        }
    }
}
