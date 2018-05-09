using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IConectorInterface;
using System.Timers;

namespace SCADAWinForms
{
    class ListaPeticiones
    {

        Dictionary<byte[], StatePeticion> listaPeticiones;

        IConector conector;

        Timer tm_envio;

        int reenvio;
        int count;

        public ListaPeticiones(IConector con)
        {
            //Inicialización de la lista de peticiones
            listaPeticiones = new Dictionary<byte[], StatePeticion>();
            conector = con;

            //Inicialización del timer
            tm_envio = new Timer();
            //Configuración del timer
            tm_envio.Interval = 50;                    //intervalo de tiempo (cada 50 ms se hace un tick)
            tm_envio.Elapsed += new ElapsedEventHandler(Envio); 
               // new EventHandler(Envio); //Se ajusta para que se llame a la función Envio.

            //Establece una función que será llamada cuando lleguen datos al conector.
            conector.SetReceiveEventHandler(RecepcionDeDatos);

        }

        public void setInterval(int v)
        {
            tm_envio.Interval = v;
        }

        public void Start()
        {
            tm_envio.Start();
        }


        public void Stop()
        {
            tm_envio.Stop();
        }

        /// <summary>
        /// Añade una petición a nuestra lista de peticiones.
        /// </summary>
        /// <param name="peticion"></param>
        public void Add(StatePeticion peticion){

            //Se añade la petición a la lista StatePetción(trama,  modelo de datos, función que procesará los datos recibidos,
            //servidor de protocolo al que pertenece la trama)
            listaPeticiones.Add(peticion.Request, peticion);

        }


        //-------------------------------------------
        //
        //       Envío y recepción de datos
        //
        //-------------------------------------------

        /// <summary>
        /// Delegado que se lanza cuando el Conector ha recibido datos.
        /// </summary>
        private void RecepcionDeDatos()
        {
            if (conector != null)
            {
                //Se leen 
                List<byte[]> data = conector.getData();

                if (data.Count > 0)
                {
                    //Recorre todas las tramas recibidas.
                    foreach (byte[] response in data)
                    {
                        bool atendida = false;
                        //Comprueba si la respuesta corresponde a alguna de las peticiones preconfiguradas y 
                        //en caso afirmativo las procesa.

                        //Esto es importante cuando se está recorriendo una lista Hay que usar un iterador
                        //sino puede que si en otro lado del codigo cambiamos la lista trabajemos con elementos 
                        //distintos en instantes distintos.
                        StatePeticion s;
                        for (int i = 0; i < listaPeticiones.Count && !atendida; i++)
                        {
                            s = listaPeticiones.ElementAt(i).Value;
                            atendida = s.Processs(response);
                            if (atendida && s.isVolatile)
                            {
                                //Para no salirse de rango es importante solo eliminar elementos en un 
                                //lugar del código. ¿En que lugar del código? En el que tenga más sentido darle esta responsabilidad.
                                //En este caso, aquí ya se sabe que se ha recibido una respuesta por lo tanto se debe borrar aquí.
                                listaPeticiones.Remove(s.Request);
                            }
                        }

                    }
                }

            }
        }


        /// <summary>
        /// Función para el envío de datos, antes de enviar comprueba que no se esté esperando una petición.
        /// Normalmente es un timer quien lanza esta función, pero también se podría lanzar manualmente.
        /// </summary>
        /// <param name="sender">Objeto que llama a la función (se puede pasar como valor null o this)</param>
        /// <param name="e">Arguemntos (no se tratan)</param>
        private void Envio(object sender, EventArgs e)
        {
            try
            {
                //Se detiene el timer para que no se llame al evento hasta que no se haya procesado
                tm_envio.Stop();

                ///Gestiona un protocolo de parada y espera. Hasta que no se ha recivido una respuesta 
                ///no procesa la siguiente. Ojo! no tiene en cuenta posibles fallos de comunicación.
                ///(no utiliza un timeout).
                ///Tras 300 intentos se pasa a la siguiente trama.
                reenvio++;
                if (reenvio < 300 && listaPeticiones.Count > 0 && listaPeticiones.ElementAt(count % listaPeticiones.Count).Value.WaitResponse == false)
                {
                    count++;
                    StatePeticion peticionActual = listaPeticiones.ElementAt(count % listaPeticiones.Count).Value;
                    byte[] trama = peticionActual.Request;
                    
                    //Se envía el mensaje a traves el conector.
                    conector.Enviar(trama);
                    peticionActual.Set();

                    //Inicia la recepción de datos en el conector por si no estba listo para recibir.
                    conector.StartReceive();
                }
                //Tras 300 intentos se considera que la trama no se ha contestado o no ser ha recibido por parte del servidor
                else if (reenvio > 300)
                {
                    listaPeticiones.ElementAt(count % listaPeticiones.Count).Value.Reset();
                    reenvio = 0;
                    count++;
                }
                //Se reanuda el timer.
                tm_envio.Start();

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error..... " + ex.Message);
            }
            finally
            {
                tm_envio.Start();
            }
        }

    }
}
