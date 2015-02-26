[assembly: WebActivator.PreApplicationStartMethod(typeof(RielAp.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(RielAp.Web.App_Start.NinjectWebCommon), "Stop")]

namespace RielAp.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using RielAp.Domain.Repositories;
    using RielAp.Web.Infrastructure.Abstract;
    using RielAp.Web.Infrastructure;
    using RielAp.Domain.Repositories.Implementations;
    using RielAp.Domain.Models;
    using RielAp.Web.Utils;
    using RielAp.Parser.Loader;
    using RielAp.Parser.Helper;
    using RielAp.Parser.Parsers;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ILoader>().To<VashmagazinHttpClientLoader>();
            kernel.Bind<ILoger>().To<ConsoleLoger>();
            kernel.Bind<IAnnouncementsParser>().To<VashmagazinAnnouncementsParserParallel>();
            // put additional bindings here
            kernel.Bind<IUsersRepository>().To<UsersRepository>();
            //kernel.Bind<INotesRepository>().To<NotesRepository>();
            kernel.Bind<IPasswordEncrypter>().To<MD5PasswordEncrypter>();
            kernel.Bind<IPasswordValidator>().To<UserPasswordValidator>();
            kernel.Bind<IPasswordValidator>().To<AdminPasswordValidator>().WhenInjectedInto(typeof(RielAp.Web.Areas.Admin.Controllers.AccountController));
            kernel.Bind<IAuthProvider>().To<FormAuthProvider>();
            //ninjectKernel.Bind<IStreetRepository>().To<StreetRepository>();
            kernel.Bind<IAnnouncementsRepository>().To<AnnouncementsRepository>();
            kernel.Bind<IPhotosRepository>().To<PhotosRepository>();
            kernel.Bind<IEmailsRepository>().To<EmailsRepository>();
            kernel.Bind<IFeedbacksRepository>().To<FeedbacksRepository>();
            kernel.Bind<IMobileNumbersRepository>().To<MobileNumbersRepository>();
            kernel.Bind<IProfilesRepository>().To<ProfilesRepository>();
            kernel.Bind<IOrdersRepository>().To<OrdersRepository>();
            kernel.Bind<IRolesRepository>().To<RolesRepository>();
            kernel.Bind<IVashmagazinRepository>().To<VashmagazinRepository>();
            kernel.Bind<IStatisticRepository>().To<StatisticRepository>();
            kernel.Bind<ISitemapGenerator>().To<SitemapGenerator>();

            

            //ninjectKernel.Bind<IApartmentManager>().To<ApartmentManager>();
            kernel.Bind<RealtyDBContext>().ToSelf().InRequestScope();
        }

        public static TResult GetInstance<TResult>() {
            return bootstrapper.Kernel.Get<TResult>();
        }
    }
}
