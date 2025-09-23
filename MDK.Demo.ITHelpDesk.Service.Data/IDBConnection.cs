using System.Data;

namespace MDK.Demo.ITHelpDesk.Service.Data
{
    public interface IDBConnection
    {
        IDbConnection GetConnection();
    }
}