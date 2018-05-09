using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IConectorInterface;
using System.Net.Sockets;
using System.IO;
using System.ComponentModel;

namespace IConversorTCPIP
{
    
    public class ConversorTCPIP : IConector
    {
        /*
        const int KMAXBUFFER=1500;
        TcpClient tcpclnt;
        private static Stream stm;
        String ip;
        String port;
        byte state1;
        public List<byte[]> myData { get; set; }

        public bool Connected { get; set; }
        */
        //Variables internas de configuracion y funcionamiento
        private String ip;
        private String port;
        private byte state1;
        private bool isConnected;

        //Objetos de conexión
        private TcpClient tcpclnt; ///Objeto de conexión TCP
        private static Stream stm; ///Buffer que se emplea internamente para escribir y leer en la conexión TCP

        //Objetos Especiales
        private BackgroundWorker bw_lectura; //Implementa un hilo para realizar lecturas de comunicación.
        private ReceiveEventHandler reh; //Permite recibir un evento en la clase del usuario cuando se produce una recepción de datos.
 
        
        //Propiedades de uso

        /// <summary>
        /// True si el objeto está leyendo del canal y false en caso contrario.
        /// </summary>
        public bool isReciving { get; set; }

        /// <summary>
        /// Lista de respuestas recibidas.
        /// </summary>
        private List<byte[]> myData;

        /// <summary>
        /// Constructor del objeto.
        /// </summary>
        public ConversorTCPIP()
        {
            //Inicialización de variables internas
            stm = null;
            state1 = 0xF8;
            tcpclnt = new TcpClient();
            myData = new List<byte[]>();
            reh = null;
            isReciving = false;

            //Creación 
            bw_lectura = new BackgroundWorker();
            this.bw_lectura.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DoWork);
            this.bw_lectura.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bck_RunWorkerCompleted);
        }


        /// <summary>
        /// Permite realizar una conexión con la configuración introducida.
        /// </summary>
        /// <param name="conf">Configuración de la conexión.</param>
        public void Connect(String conf = null)
        {

            try
            {
                if (conf != null)
                {
                    string[] l1 = conf.Split(';');
                    foreach (string s in l1)
                    {
                        setConfig(s);
                    }
                }
                tcpclnt.Connect(ip, Int32.Parse(port)); // use the ipaddress as in the server program

                stm = tcpclnt.GetStream();
                isConnected = true;
            }
            catch (Exception ex)
            {
                isConnected = false;
                throw ex;
            }
        }

        /// <summary>
        /// Cierra la conexión del sockuet.
        /// </summary>
        public void Close()
        {
            try
            {
                tcpclnt.Close();
                tcpclnt = new TcpClient();
                isConnected = false;
            }
            catch (Exception) { }
        }

        
        /// <summary>
        /// Permite realizar el envío de una trama.
        /// </summary>
        /// <param name="trama">Trama a enviar.</param>
        /// <returns>Trama enviada con cabeceras añadiadas por el conector.</returns>
        public byte[] Enviar(byte[] trama)
        {
            stm.Write(trama, 0, trama.Length);
            stm.Flush();
            return trama;
        }
        
        /// <summary>
        /// Permite saber si el objeto está conectado.
        /// </summary>
        /// <returns></returns>
        public bool isConected()
        {
            return isConnected;
        }

        /// <summary>
        /// Inicia la recepción de datos.
        /// </summary>
        public void StartReceive()
        {
            if (!isReciving && !bw_lectura.IsBusy)
            {
                bw_lectura.RunWorkerAsync();
            }
            isReciving = true;
        }

        /// <summary>
        /// Inicia la recepción de datos.
        /// </summary>
        public void StopReceive()
        {
            isReciving = false;
        }


        /// <summary>
        /// Thread Responsable de las lecturas. Funciona mediante BackGround Worker.
        /// </summary>
        /// <param name="sender">BackGround Worker que llama al metodo.</param>
        /// <param name="e">Parámetros que se desean pasar al thread.</param>
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Lectura del socket
                byte[] myBuffer;

                // Se crea un buffer para los datos recibidos.
                myBuffer = new byte[tcpclnt.ReceiveBufferSize];
                 
                // Lectura de los datos de la conexión y almacenaje en el buffer
                int lData = stm.Read(myBuffer, 0, tcpclnt.ReceiveBufferSize);
                MemoryStream ms = new MemoryStream();
                ms.Write(myBuffer, 0, lData);
                //Inserción de los datos en el Stream de bytes.
                //myData.Write(myBuffer, 0, myBuffer.Length);
                myData.Add(ms.ToArray());

            }
            catch (Exception) { }
            finally
            {
            }

        }


        /// <summary>
        /// Permite leer los datos recibidos.
        /// </summary>
        /// <returns>Lista de tramas de respuesta recibidas, hasta el momento o null en caso de no haber recibido datos.</returns>
        public List<byte[]> getData()
        {
            List<byte[]> copyofdata = null;
            StopReceive();
            lock (myData)
            {
                copyofdata = new List<byte[]>(myData);
                myData.Clear();
            }
            StartReceive();
            return copyofdata;
        }


        /// <summary>
        /// Permite establecer una función a la que se llamará una vez que se han recibido datos, o bien el timeout de lectura ha expirado.
        /// </summary>
        /// <param name="funcionDelegada">Función delegada a la que se llamará llamada.</param>
        public void SetReceiveEventHandler(ReceiveEventHandler funcionDelegada)
        {
            reh = funcionDelegada;
        }


        /// <summary>
        /// Reinicia la lectura si está activa la recepción de datos.
        /// </summary>
        /// <param name="sender">Objeto BackGroundWroker que lanzó el evento.</param>
        /// <param name="e">Parámetros pasados al metodo.</param>
        private void bck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            reh();
            if (isConected())
            {
                if (isReciving && !bw_lectura.IsBusy)
                {
                    bw_lectura.RunWorkerAsync();
                }
            }
        }



        /// <summary>
        /// Ayuda a cargar la configuración del robot de un arhcivo de texto.
        /// </summary>
        /// <param name="line">Linea del archivo a analizar.</param>
        private void setConfig(string line)
        {
            string l11 = line.Split('=')[0];
            string l12 = line.Split('=')[1];
            switch (l11)
            {
                case "ip":
                    state1 = (byte)(state1 | 0x01);
                    ip = l12;
                    break;
                case "port":
                    state1 = (byte)(state1 | 0x02);
                    port = l12;
                    break;
                case "timeout":
                    state1 = (byte)(state1 | 0x04);
                    tcpclnt.SendTimeout = Int32.Parse(l12);
                    break;
            }
        }


    }
}
