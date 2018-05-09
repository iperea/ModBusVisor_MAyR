using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDB_Interface;
using ClasesDeUso;
using System.Data.Common;
using System.Data;

namespace DataModel
{
    //Delegado para actualizar la interfaz
    public delegate void CustomUpdateHandler(Modelo m);

    //Modelo de datos a representar
    public class Modelo
    {
        //Entradas digitales (luces)
        public bool[] DigitalInput;

        //Entradas Analogicas (potenciometros)
        public int[] AnalogInput;

        //Entradas Analogicas en formato Real (potenciometros)
        public double[] AnalogInputReal;
        

        //Lista de funciones que actualizan la interfaz (las crea el usuario y las introduce mediante el metodo AddHandler).
        public List<CustomUpdateHandler> funcionesActualizarInterfaz;

        //Base de datos del sistema
        DB_Interface bd;
        ConectionConfig bdconfig;

        /// <summary>
        /// Constructor. Construye un modelo con i entrdas digitales y j entradas analógicas.
        /// </summary>
        /// <param name="i">Cantidad de entradas digitales de nuestro modelo.</param>
        /// <param name="j">Cantidad de entradas analogicas de nuestro modelo.</param>
        /// <param name="k">Cantidad de entradas analogicas reales de nuestro modelo.</param>
        public Modelo( int i)
        {
            DigitalInput = new bool[i];
            ///TODO: Instanciar las variables analogicas (valor codificado y valor real)
            funcionesActualizarInterfaz = new List<CustomUpdateHandler>();
            bdconfig = ConectionConfig.LoadConfig("config.xml", "database")[0];
            bd = (DB_Interface) bdconfig.getInstance();
        }


        /// <summary>
        /// Metodo que perimte añadir un nuevo manejador al modelo. Añade una nueva función para actualizar el interfaz de usuario.
        /// </summary>
        /// <param name="hand">Nombre de la función (tiene que ser de tipo CustomUpdateHandler).</param>
        public void AddHandler(CustomUpdateHandler hand)
        {
            funcionesActualizarInterfaz.Add(hand);
        }

        /// <summary>
        /// Eliminea un manejador de la lista.
        /// </summary>
        /// <param name="hand">Manejador a eliminar (es el nombre de la función)</param>
        public void removeHandler(CustomUpdateHandler hand)
        {
            funcionesActualizarInterfaz.Remove(hand);
        }

        /// <summary>
        /// Permite establecer el valor de una entrdada digital a traves de una función externa al modelo.
        /// </summary>
        /// <param name="i">Nuemro de la entrada que se desea cambiar.</param>
        /// <param name="d">Valor que se desea establecer.</param>
        public void Set(int i, bool d)
        {
            DigitalInput[i] = d;
        }

        /// <summary>
        /// Permite establecer el valor de una entrdada analogica a traves de una función externa al modelo.
        /// </summary>
        /// <param name="i">Numero de la entrada que se desea cambiar.</param>
        /// <param name="d">Valor que se desea establecer.</param>
        ///TODO: función Set para los valores analogicos codificados
        //public void Set(int i, int d)
        

        /// <summary>
        /// Permite establecer el valor de una entrdada analógica real a través de una función externa al modelo.
        /// </summary>
        /// <param name="i">Nuemro de la entrada que se desea cambiar.</param>
        /// <param name="d">Valor que se desea establecer.</param>
        ///TODO: función Set para los valores analogicos reales
        //public void Set(int i, double d)
        

        /// <summary>
        /// Permite actualizar la interfaz de usuario a partir de los datos del modelo.
        /// </summary>
        public void UpdateInterface()
        {
            foreach (CustomUpdateHandler hand in funcionesActualizarInterfaz)
            {
                hand(this);
            }
        }

        
        /// <summary>
        /// Establece la conexión con la BD.
        /// </summary>
        public void OpenDBConection()
        {
            bd.Conect(bdconfig.Config);
            bd.CreateCommandsSP("configBD.xml");
        }

        /// <summary>
        /// Cierra la conexión con la BD.
        /// </summary>
        public void CloseDBConection()
        {
            bd.Close();
        }

        /// <summary>
        /// Permite actualizar los datos del sistema en la BD.
        /// </summary>
        ///         
        public void ActualizaDatosSistemaDB(){

            String s = "SELECT \"actualizar_sistema\"(1 , '{ 1, 2 }' , '{ 0 , 0 }' ,"+ 
                "'{ 1, 2 }' , '{" + ((DigitalInput[0])?"1":"0") +" , "+ ((DigitalInput[1])?"1":"0") + "}' ," + 
                "'{ 1, 2 }' , '{" + AnalogInputReal[0].ToString().Replace(',','.') +" , "+ AnalogInputReal[1].ToString().Replace(',','.') + "}');";


            bd.ExecuteCommandAsync(s,null);


            /*
             * Otra forma de hacerlo más profesional sería algo así
             * 
            int[] ids = { 1, 2 };
            int[] bt = { 0, 0 };

            DbCommand comando = bd.getCommand("actualizar_sistema");
            comando.Parameters["idsis"].Value = 1;

            comando.Parameters["i_id_boton"].Value = ids;
            comando.Parameters["valor_boton"].Value = bt;

            comando.Parameters["i_id_led"].Value = ids;
            comando.Parameters["valor_led"].Value = DigitalInput;

            comando.Parameters["i_id_poten"].Value = ids;
            comando.Parameters["valor_pot"].Value = AnalogInput;

            
            bd.ExecuteCommandAsync(comando,null);
             * */
        }
        

    }

}
