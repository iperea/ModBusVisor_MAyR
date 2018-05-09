using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using ClasesDeUso;
using IConectorInterface;
using IIOProtocol;
using System.Xml.Linq;
using DataModel;

namespace SCADAWinForms
{



    public partial class Form1 : Form
    {

        //Gestión de envios.
        int count = 0;
        int reenvio = 0;

        const byte RCM = 0x01;
        const byte RDM = 0x02;
        const byte RRM = 0x03;
        const byte RIM = 0x04;
        const byte WCS = 0x05;
        const byte WRS = 0x06;
        const byte WCM = 0x0F;
        const byte WRM = 0x10;

        ListaPeticiones listaPeticiones;

        List<ConectionConfig> listaConectores;
        List<ConectionConfig> listaProtocolos;

        IConector conexionEstablecida;
        IIOProtocolInterface protocoloEnUso;


        Modelo modelo_datos;

        Report form_report;

        Timer tm;

        /// <summary>
        /// Constructo. Este metodo se llama cuando se lanza la apliación.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            //Contador usado para el envio y recepción de tramas.
            count = 0;

            //Crea el modelo de datos.
            CreateDataModel();


            //------------------------------------
            //Carga de los conectores y protocolos
            //------------------------------------

            //Se cargan los conectores (Se usa el primero de la lista)
            listaConectores = ConectionConfig.LoadConfig("config.xml","conector");
            //Se cargan los protocolos 
            listaProtocolos = ConectionConfig.LoadConfig("config.xml","protocol");


            //CONEXION
            try
            {
                //Se cargan los conectores 
                conexionEstablecida= (IConector) listaConectores[0].getInstance();
                //Se establece la conexión.

                //Inicialización de la lista de peticiones
                listaPeticiones = new ListaPeticiones(conexionEstablecida);


                //Se cargan los protocolos (Se usa el primero de la lista)
                protocoloEnUso=  (IIOProtocolInterface) listaProtocolos[0].getInstance();

                //Se prepara la lista de peticiones
                RellenarPeticiones(listaPeticiones);
              //Se enciende el timer.

                conexionEstablecida.Connect(listaConectores[0].Config);
                listaPeticiones.Start();


                tm = new Timer();
                tm.Interval = 50;
                tm.Tick += new EventHandler(ActualizaInterfaz);
                tm.Start();

            }
            catch (Exception ex)
            {
                //Si ha habido algún error en la conexión se cierra y se muestra el mensaje.
                if (conexionEstablecida != null)
                {
                    conexionEstablecida.Close();
                    conexionEstablecida = null;
                }
                MessageBox.Show("Error..... " + ex.Message);
            }
        
        }

        /// <summary>
        /// Metodo que actualiza la BD (evento para el metodo tick de un System.WinForms.Timer)
        /// </summary>
        /// <param name="sender">Objeto que llama al metodo.</param>
        /// <param name="e">Argumentos.</param>
        private void ActualizaInterfaz(object sender, EventArgs e)
        {
            modelo_datos.UpdateInterface();
        }

        /// <summary>
        /// Procedimiento que se lanza al cerrar el formulario y que cerra los recursos abiertos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                listaPeticiones.Stop();
                modelo_datos.CloseDBConection();
                conexionEstablecida.StopReceive();
                conexionEstablecida.Close();
            }
            catch (Exception) { }
        }


        /// <summary>
        /// Permite mostrar una trama en la interfaz visual a modo de monitor de red.
        /// </summary>
        /// <param name="ini">Cadena inicial del mensaje.</param>
        /// <param name="t">Trama.</param>
        /// <param name="fin">Cdena final del mensaje.</param>
        private void MostrarTramaEnInterfaz(string ini, byte[] t, string fin){

            //Crea un String de vista de la trama para podre mostrarlo en la interfaz
            string envio = "";
            StringBuilder sb = new StringBuilder(t.Length * 2);
            for (int i = 0; i < t.Length; i++)
            {
                sb.AppendFormat("{0:x2} - ", t[i]);
            }
            envio = sb.ToString();

            lb_mensajes.Items.Insert(0, ini+ envio+ fin);
        }


        /// <summary>
        /// Limpia la lista de tramas recividas y envías del interfaz grafico.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            lb_mensajes.Items.Clear();
        }


        ///*************************************************************************
        ///                     
        ///              FUNCIONES IMPORTANTES PARA EL ESTUDIANTE
        ///                     
        ///*************************************************************************
      

        //-------------------------------------------
        //
        //    Generacion de la lsita de peticiones
        //
        //-------------------------------------------

        /// <summary>
        /// Genera una lista de peticiones lista de peticiones que quedará almacenada con las peticiones que el sistema puede realizar.
        /// </summary>
        /// <param name="listaPeticiones">Lista de peticoines que se desea rellenar.</param>
        private void RellenarPeticiones(ListaPeticiones listaPeticiones)
        {

            //----------------------------
            //Peticion de lectura de BITS:
            //----------------------------
            //La direccion es SSRRRRT (SS) direccion del esclavo, RRRR dirección de memoria 
            //y T es el tipo (C coil, D entrada Digital, R Registro, I entrada analogica)
            string dir = "070002C";
            int c = 2;
            byte[] trama = null;

            //Se llama al protocolo en uso para que genere la trama que correspondería
            trama = protocoloEnUso.ReadBit(dir, c);

            //Se añade la petición a la lista StatePetción(trama,  modelo de datos, función que procesará los datos recibidos,
            //servidor de protocolo al que pertenece la trama)
            listaPeticiones.Add( new StatePeticion(trama, modelo_datos, ProcesarPilotos, protocoloEnUso));

            //----------------------------
            //Peticion de lectura de Analogicos (Registros):
            //----------------------------
            ///TODO: Aquí el alumno tendrá que repetir la estructura anterior, pero esta vez deberá añadir a la lista de peticiones 
            ///las peticiones necesarias para obtener los valores analógicos.
        }


        //-------------------------------------------
        //
        //       Procesamiento de la respuesta
        //
        //-------------------------------------------
       

        /// <summary>
        /// Delegado para procesar las respuestas de lecura de los pilotos.
        /// </summary>
        /// <param name="m">Modelo que contiene el estado de los botones.</param>
        /// <param name="response">Datos de la respuesta.</param>
        public void ProcesarPilotos(Modelo m, byte[] response)
        {
            m.Set(0, ((response[response.Length - 1] & 0x01) > 0));
            ///TODO:Procesar el segundo piloto
        }

        /// <summary>
        /// Delegado para procesar las respuestas de lectura de las barras.
        /// </summary>
        /// <param name="m">Modelo de datos donde están representados los valores de las barras.</param>
        /// <param name="response">Datos de la respuesta.</param>
        public  void ProcesarAnalog(Modelo m, byte[] response)
        {
            ///TODO: En este punto hay que procesar los valores Analógicos recibidos en la respuesta (como una cadena de bytes)
            ///y procesarlos para obtener el valor real que se está representando.
        }


        //-------------------------------------------
        //
        //       Actulaización de la interfaz
        //
        //-------------------------------------------


        /// <summary>
        /// Creacion del modelo de datos y lincado de las funcioens de actualización del interfaz grafico.
        /// </summary>
        private void CreateDataModel()
        {
            //Creación del modelo
            ///TODO:Esta creación habrá que actualizarla para incluir los valores analgocios (reales y codificados)
            modelo_datos = new Modelo(2);

            //Se añaden los manejadores para actualizar la interfaz grafica
            modelo_datos.AddHandler(ActualizarBarrasProgreso);
            modelo_datos.AddHandler(ActualizarBotons);
            modelo_datos.AddHandler(ActualizarTextBox);

            modelo_datos.OpenDBConection();
        }


        /// <summary>
        /// Actualiza las Barras de Progreso del interfaz Grafico
        /// </summary>
        /// <param name="m">Modelo de datos donde están represntados los valores de las barras.</param>
        public void ActualizarTextBox(Modelo m)
        {
            ///TODO: Hay que mostrar los valores reales de los analogicos que hay actualizados en el Modelo.
            tb_analogica1.Text = "";
            tb_analogica2.Text = "";
        }


        /// <summary>
        /// Actualiza la vista de la interfaz con el estado actual de los botones.
        /// </summary>
        /// <param name="m">Modelo que contiene el estado de los botones.</param>
        public void ActualizarBotons(Modelo m)
        {
            if (m.DigitalInput[0] == true)
            {
                bt_pulsado1.ImageIndex = 0;
            }
            ///TODO: Con el código de arriba se selecciona la imagen a mostrar por el piloto cuando el valor digital de la primera variable
            ///booleana del modelo es true. Habrá que añadir el resto de código para que cuando sea false se utilice una imagen diferente en
            ///la representación del piloto.
        }

        /// <summary>
        /// Actualiza las Barras de Progreso del interfaz Grafico
        /// </summary>
        /// <param name="m">Modelo de datos donde están represntados los valores de las barras.</param>
        public void ActualizarBarrasProgreso(Modelo m)
        {
            ///TODO:Este delegado debe actualizar las barras de progreso a modo de porcentaje sobre los valores analógicos leídos.
            ///Para ello habrá que tener en cuenta los rangos máximos y mínimos de estos valores.
            ///Pista: Los objetos a modificar son: pb_analogica1 y pb_analogica2 (progress bar)
        }


        //-------------------------------------------
        //
        //                  Mandos
        //
        //-------------------------------------------

        private void bt_encender1_Click(object sender, EventArgs e)
        {

            string dir;
            byte[] trama;
            bool[] dato;

            //-----------------------------------------------------
            //Peticion pra la escritura de un variable de tipo Coil:
            //-----------------------------------------------------
            //La direccion "dir" debe ser una cadena con el formato SSRRRRT, dónde (SS) es la direccion del esclavo, 
            //RRRR es la dirección de memoria a leer o escribir
            //y T es el tipo (C coil, D entrada Digital, R Registro, I entrada analogica) (recuerda que para modbus esto es como si formase parte del direccionamiento de la memoria)

            Button b = (Button)sender;
            if(b.Name=="bt_encender1"){
                dir = "070000C";
            }
            ///TODO:Hacer el otro botón


            //Se llama al protocolo en uso para que genere la trama que correspondería
            ///TODO: Ejemplo de construcción de una trama:
            //trama = protocoloEnUso.WriteBit(dir, c, dato);

            //Se añade la petición a la lista StatePetción(trama,  modelo de datos, función que procesará los datos recibidos,
            //servidor de protocolo al que pertenece la trama)
            try
            {
                ///TODO: Añadir a la lista de peticiones la trama creada para encender un Coil
                ///Primero creamos la petición:
                ///StatePeticion p=new StatePeticion(trama, modelo_datos, "delegado que procesará la respuesta", "protocolo empleado", "¿es volatil?")
                ///listaPeticiones.Add(p);
                ///Explicación:
                ///El delegado será la función que procesará la respuestas
                ///El protocolo empleado será el objeto que implementa el protocolo al que pertenece la trama "trama"
                ///Volatil: Una petición es volatil si una vez enviada debe eliminarse de la lista de peticiones.
                
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Delegado para procesar las respuestas de lecura de los botones.
        /// </summary>
        /// <param name="m">Modelo que contiene el estado de los botones.</param>
        /// <param name="response">Datos de la respuesta.</param>
        public void ProcesarRespuestaBoton(Modelo m, byte[] response)
        {
            ///TODO: Este debe ser el delegado a ejecutar cuando se recibe una respuesta a una trama de escritura en una variable digital tipo botón
            ///como la comentada en la función anterior.
            ///Los botones en el automata surten efecto al realizar un flanco de subida y uno de bajada, por lo que para simular este comportambiento
            ///una vez que se ha enviado una trama para escribir un uno en la variable Coil del automata, se debe mandar una escritura de un cero
            ///en la misma variable. Por tanto esto último se realizará en esta función que será ejecutada cuando llegue la respuesta de la lista de peticiones.
            ///

        }

        private void bt_ActualizarBD_Click(object sender, EventArgs e)
        {
            modelo_datos.ActualizaDatosSistemaDB();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form_report = new Report(modelo_datos);
            form_report.Show();
        }

    }
}
