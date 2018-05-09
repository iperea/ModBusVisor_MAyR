using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace ClasesDeUso
{
    public class ConectionConfig
    {
        /// <summary>
        /// Nombre del elemento de configuración (este lo asigna el usuario a su antojo)
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Nombre del paquete del ensamblado (nombre de la dll)
        /// </summary>
        public String Paket { get; set; }
        /// <summary>
        /// Nombre de la clase a cargar (como se llama el objeto que se desea crear)
        /// </summary>
        public String Clase { get; set; }
        /// <summary>
        /// String con parámetros de configuración que se le puede pasar al objeto.
        /// </summary>
        public String Config { get; set; }
        /// <summary>
        /// Si el objeto de la configuración está acrivo o no.
        /// </summary>
        private Boolean Active { set; get; }
        /// <summary>
        /// Instancia cargada del objeto.
        /// </summary>
        private Object instancia { set; get; }


        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public ConectionConfig()
        {
        }

        /// <summary>
        /// Devuelve true si el objeto está activo y false si no lo está. Cuando un objeto está activo tiene cargado en su
        /// puede obtenerse la instancia que lo implementa a través del metodo getInstance().
        /// </summary>
        /// <returns>true si el objeto está activo y false si no lo está.</returns>
        public bool isActive()
        {
            return Active;
        }

        /// <summary>
        /// Devuelve la instancia del objeto de configuración.
        /// </summary>
        /// <returns>Instancia del objeto de configuración o null en caso de que el objeto no esté activo.</returns>
        public object getInstance()
        {
            return instancia;
        }

        /// <summary>
        /// Constructor sobrecargado, crea un bojeto de tipo configuracióny asigna valores a sus propiedades.
        /// </summary>
        /// <param name="n">Nombre del elemento de configuración (este lo asigna el usuario a su antojo)</param>
        /// <param name="p">Nombre del paquete del ensamblado (nombre de la dll)</param>
        /// <param name="c">Nombre de la clase a cargar (como se llama el objeto que se desea crear)</param>
        /// <param name="a">Si el objeto de la configuración está acrivo o no.</param>
        /// <param name="conf">String con parámetros de configuración que se le puede pasar al objeto.</param>
        public ConectionConfig(string n, string p, string c, bool a,string conf)
        {
            Name = n;
            Paket = p;
            Clase = c;
            Active = a;
            Config = conf;
            instancia = null;
        }


        /// <summary>
        /// Constructor sobrecargado, crea un bojeto de tipo configuracióny asigna valores a sus propiedades.
        /// </summary>
        public void Activar()
        {
            //------------------------------------------------
            //Carga del ensamblado para los conectores
            //------------------------------------------------
            Assembly a = Assembly.Load(Paket); //Diccionario de assemblies
            Type t = a.GetType(Paket + "." + Clase);
            object h = Activator.CreateInstance(t);
            instancia = h;
            //-------------------------------------------------
            // Fin de carga del ensamblado para el conector
            //------------------------------------------------
        }

        
        /// <summary>
        /// Constructor sobrecargado, crea un bojeto de tipo configuracióny asigna valores a sus propiedades.
        /// </summary>
        public void Desactivar()
        {
            instancia=null;
        }

        /// <summary>
        /// Permite crear una lista de objetos de configuración.
        /// Si el objeto está activo se procede a la carga de la dll correspondiente.
        /// </summary>
        /// <param name="archivo">Archivo de configuración.</param>
        /// <param name="elemento">Tipo de elementos a cargar, se corresponde con el tag que envuelve a los parámetros de configuración (En el ejemplo de configuración se correspondería con "conector" o "protocol")</param>
        /// <returns>Se devuelve la lista de elementos de configuración creados.</returns>
        public static List<ConectionConfig> LoadConfig(string archivo, string elemento)
        {
            //Diccionario donde se cargaran las configuraciones
            List<ConectionConfig> configuracionesCargadas = new List<ConectionConfig>();

            XElement xmlconf = XElement.Load(archivo);
            //Obtener todas las interfaces a crear
            var configuraciones = from c in xmlconf.Descendants(elemento)
                                  select new ConectionConfig
                                  {
                                      Name = (string)c.Element("name"),
                                      Paket = (string)c.Element("paket"),
                                      Clase = (string)c.Element("clase"),
                                      Config = (string)c.Element("config"),
                                      Active = (bool)Boolean.Parse((string)c.Element("active"))
                                      
                                  };

            foreach (ConectionConfig con in configuraciones)
            {
                if (con.Active)
                {
                    con.Activar();
                }
                configuracionesCargadas.Add(con);
            }
            return configuracionesCargadas;
        }


        /*
         *  EJEMPLO DE CONFIGURACION
         *  
         * <Conections>
          <conector>
            <name>Conversor Wifi</name>
            <paket>IConversorWiffi2RS485</paket>
            <clase>Conversor</clase>
            <active>true</active>
            <config>ip=10.0.0.10;port=4660;timeout=100</config>
          </conector>  
          <conector>
            <paket>IConversorTCPIP</paket>
            <clase>Conversor</clase>
            <name>Conversor TCP</name>
            <active>true</active>
            <config>ip=10.0.0.10;port=502;timeout=100</config>
          </conector>
          <protocol>
            <name>Modbus TCP</name>
            <paket>IProtocolModBusTCP</paket>
            <clase>Protocol</clase>
            <active>true</active>
            <config>ip=10.0.0.10;port=4660;timeout=100</config>
          </protocol>
          <protocol>
            <paket>IProtocolModBusRTU</paket>
            <clase>Protocol</clase>
            <name>ModBus Serie RTU</name>
            <active>true</active>
            <config>ip=10.0.0.10;port=502;timeout=100</config>
          </protocol>
        </Conections>
         * */
    }
}
