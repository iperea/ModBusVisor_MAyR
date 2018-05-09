using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using IDB_Interface;
using System.Xml.Linq;
using System.Data.Common;
using System.Reflection;
using System.ComponentModel;

namespace IDB_PosgreDB
{


    class Parametro
    {

        public Parametro()
        {
        }

        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Comando { get; set; }
        public bool isArray { get; set; }
    };

    class Comando{
        public Comando(NpgsqlCommand c, RunWorkerCompletedEventHandler h){
            comandoBD=c;
            manejador=h;
        }
        public NpgsqlCommand  comandoBD {get; set;}
        public RunWorkerCompletedEventHandler manejador {get; set;}
    };

    public class NpgSQL_PosgreSQLDB : DB_Interface
    {
        NpgsqlConnection conn = null;
        Dictionary<String, DbCommand> comandosDB;


        //Objetos Especiales
        private BackgroundWorker bw_lectura; //Implementa un hilo para realizar lecturas de comunicación.
        
        List<Comando> lista;
        System.Timers.Timer t;

        public NpgSQL_PosgreSQLDB(){
            conn = new NpgsqlConnection();
            comandosDB = null;
            bw_lectura = new BackgroundWorker();
            bw_lectura.DoWork += new DoWorkEventHandler(EjecucionComandoAsincrona);
            lista=new List<Comando>();
            t = new System.Timers.Timer();
            t.Elapsed += new System.Timers.ElapsedEventHandler(timerEvent);
            t.Interval = 100;
            t.Start();
        }

        public void setInterval(double d){
            t.Interval=d;
        }

        public bool Conect(String cadenaConexion)
        {
            bool res = true;
            try
            {
                //Ejemplo de cadena de conexió
                //"Server=127.0.0.1;Port=5432;User Id=joe;Password=secret;Database=joedata;"
                conn.ConnectionString = cadenaConexion;
                conn.Open();
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }


        public DataSet ExecuteCommand(string comando)
        {
            NpgsqlCommand command = new NpgsqlCommand(comando, conn);

            DataSet ds = new DataSet();

            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            da.SelectCommand = command;
            da.Fill(ds);
            return ds;
        }

        public DataSet ExecuteCommand(System.Data.Common.DbCommand comando)
        {
            NpgsqlCommand command = (NpgsqlCommand)comando;
            DataSet ds = new DataSet();

            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            da.SelectCommand = command;
            da.Fill(ds);
            return ds;
        }


        public void ExecuteCommandAsync(DbCommand comando, RunWorkerCompletedEventHandler hand)
        {
            NpgsqlCommand command = (NpgsqlCommand)comando;
            command.CommandText = "\"" + command.CommandText + "\"";
            lista.Add(new Comando(command, hand));
        }

        public void ExecuteCommandAsync(string comando, RunWorkerCompletedEventHandler hand)
        {            
            NpgsqlCommand command = new NpgsqlCommand(comando, conn);    

            lista.Add(new Comando(command,hand));


        }


        private void timerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            t.Stop();
            if (!bw_lectura.IsBusy)
            {
                bw_lectura.RunWorkerAsync();
            }
            t.Start();
        }


        private void EjecucionComandoAsincrona(object sender, DoWorkEventArgs e)
        {
            try
            {
                if(lista.Count>0){
                    
                    DataSet ds = new DataSet();                    
                    Comando c=lista[0];

                    lock(lista){
                        lista.RemoveAt(0);
                    }
                    if (c.manejador != null)
                    {
                        bw_lectura.RunWorkerCompleted += c.manejador;
                    }
                    else
                    {
                        bw_lectura.RunWorkerCompleted += funcionVacia;
                    }
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter();
                    da.SelectCommand = c.comandoBD;                        
                    da.Fill(ds);
                    e.Result=ds;
                }

            }
            catch (Exception) { }
            finally
            {
            }

        }

        void funcionVacia(object sender, RunWorkerCompletedEventArgs e){
        }

        public DbCommand getCommand(string name)
        {
            NpgsqlCommand c = (NpgsqlCommand)comandosDB[name];
            NpgsqlCommand ret = new NpgsqlCommand(c.CommandText, c.Connection);

            foreach (NpgsqlParameter p in c.Parameters)
            {
                ret.Parameters.Add(new NpgsqlParameter(p.ParameterName, p.NpgsqlDbType));
            }
            ret.CommandType = c.CommandType;

            return (DbCommand)ret;
        }





        public void CreateCommandsSP(string archivo)
        {
            try
            {
                if (conn != null)
                {
                    //Diccionario donde se cargaran las configuraciones
                    comandosDB = new Dictionary<String, DbCommand>();

                    XElement xmlconf = XElement.Load(archivo);
                    //Obtener todas las interfaces a crear
                    var commands = from c in xmlconf.Descendants("command")
                                   select new NpgsqlCommand
                                   {
                                       CommandText = (string)c.Element("name"),
                                       Connection = conn,
                                       CommandType = (CommandType)((((bool)c.Element("prepared"))==true)?CommandType.StoredProcedure:CommandType.TableDirect)

                                   };



                    foreach (NpgsqlCommand c in commands)
                    {
                        comandosDB.Add(c.CommandText, c);

                        var par = from p in xmlconf.Descendants("parameter")
                                  select new Parametro
                                  {
                                      Nombre = (string)p.Element("name"),
                                      Tipo = (string) p.Element("type"),
                                      Comando = (string) p.Parent.Element("name"),
                                      isArray = (bool)p.Element("array")

                                  };


                        foreach (Parametro p in par)
                        {
                            NpgsqlParameter param = new NpgsqlParameter();

                            param.ParameterName = p.Nombre;

                            Type tip = typeof(NpgsqlDbType);
                            FieldInfo[] f1 =tip.GetFields(BindingFlags.Public | BindingFlags.Static);
                            
                            int i = 0;
                            for (; i < f1.Length && f1[i].ToString().Split(' ').Last() != p.Tipo; i++) ;
                            param.NpgsqlDbType = (NpgsqlDbType)(f1[i].GetValue(f1[i]));
                            
                            if (p.isArray)
                            {
                                param.NpgsqlDbType = param.NpgsqlDbType | NpgsqlDbType.Array;
                            }

                            comandosDB[c.CommandText].Parameters.Add(param);
                        }
                    }
                }
                else
                {
                    throw new Exception("La conexión a la Base de datos no se ha realizado, por favo conecte antes de crear los comandos");
                }                
            }
            catch (Exception)
            {

            }
        }



        public void Close()
        {
            t.Stop();
            conn.Close();
            conn = null;
        }
    }
}
