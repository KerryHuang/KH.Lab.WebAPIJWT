using Autofac;
using Dapper.Extensions.SQLite;
using System.Reflection;

namespace KH.Lab.WebAPI_Dapper_Caching
{
    public class AutofacModuleRegister : Autofac.Module
    {
        /// <summary>
        /// 重寫Autofac管道Load方法，在這裡註冊注入
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddDapperForSQLite("DefaultConnection");

            ////程式集註入業務服務
            //var IRepositoryApp = Assembly.Load("Presco.CAS.Repository");
            //var RepositoryApp = Assembly.Load("Presco.CAS.Repository");
            ////根據名稱約定（服務層的介面和實現均以Service結尾），實現服務介面和服務實現的依賴
            //builder.RegisterAssemblyTypes(IRepositoryApp, RepositoryApp)
            //  .Where(t => t.Name.EndsWith("Repository") || t.Name.EndsWith("Service"))
            //  .AsImplementedInterfaces();

        }
    }
}
