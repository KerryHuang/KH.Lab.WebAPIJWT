using Dapper.Extensions.Monitor;

namespace KH.Lab.WebAPI_Dapper_Caching
{
    public class MyMonitorHandler : IMonitorHandler
    {
        public async Task OnSlowSqlCommandAsync(string methodName, string sqlOrSqlName, object param, long duration)
        {
            throw new Exception("error");
            Console.WriteLine("#######################");
            await Task.Delay(5 * 1000);
            Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@");
        }
    }
}
