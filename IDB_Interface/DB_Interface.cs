using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDB_Interface
{
    public interface DB_Interface
    {
        void Close();
        bool Conect(string cadenaConexion);
        void CreateCommandsSP(string archivo);
        System.Data.DataSet ExecuteCommand(System.Data.Common.DbCommand comando);
        System.Data.DataSet ExecuteCommand(string comando);
        void ExecuteCommandAsync(System.Data.Common.DbCommand comando, System.ComponentModel.RunWorkerCompletedEventHandler hand);
        void ExecuteCommandAsync(string comando, System.ComponentModel.RunWorkerCompletedEventHandler hand);
        System.Data.Common.DbCommand getCommand(string name);
        void setInterval(double d);
    }
}
