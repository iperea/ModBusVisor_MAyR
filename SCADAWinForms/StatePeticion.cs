using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIOProtocol;
using DataModel;

namespace SCADAWinForms
{

    /// <summary>
    /// Tipo de delegado creado para procesar las respuestas del sistema a las peticiones.
    /// </summary>
    /// <param name="m">Modelo que hay que actualizar.</param>
    /// <param name="resp">Datos recividos en la respuesta.</param>
    public delegate void CustomProcessRequestHandler(Modelo m, byte[] resp);


    public class StatePeticion
    {

        /// <summary>
        /// Peticiçopn representada por este objeto.
        /// </summary>
        public byte[] Request { get; set; }

        /// <summary>
        /// Delegado que procesará la respuesta a la petición representada por este objeto.
        /// </summary>
        CustomProcessRequestHandler delegadoProcesarRespuesta;

        /// <summary>
        /// Representa si se esta esperando una respuesta o no. True si se ha enviado una petición y aún no ha lleagado.
        /// </summary>
        public bool WaitResponse { get; set; }

        /// <summary>
        /// Si la petición es volatil se desencola al procesar su respuesta.
        /// </summary>
        public bool isVolatile { get; set; }


        /// <summary>
        /// Modelo de datos al que se aplica la petición.
        /// </summary>
        private Modelo modelo;

        /// <summary>
        /// Protocolo que implementa esta petición.
        /// </summary>
        IIOProtocolInterface prot;

        /// <summary>
        /// Constructor, perimte crear un objeto de estado de petición.
        /// </summary>
        /// <param name="req">Petición que representará.</param>
        /// <param name="m">Modelo de datos que deben actualizar las peticiones represetnadas en este objeto.</param>
        /// <param name="hand">Manejador que actualizará los datos del modelo a partir de los datos recibidos en una petición.</param>
        /// <param name="p">Protocolo que se emplea en la petición.</param>
        public StatePeticion(byte[] req, Modelo m, CustomProcessRequestHandler hand, IIOProtocolInterface p, bool isvolatile=false)
        {
            Request = req;
            WaitResponse = false;
            modelo = m;
            delegadoProcesarRespuesta = hand;
            prot = p;
            isVolatile = isvolatile;
        }

        /// <summary>
        /// Establece a verdadero la propiedad de esprando respuesta (establece la espera).
        /// </summary>
        public void Set()
        {
            WaitResponse = true;
        }

        /// <summary>
        /// Establece a falseo la propiedad de esprando respuesta (resetea la espera).
        /// </summary>
        public void Reset()
        {
            WaitResponse = false;
        }

        /// <summary>
        /// Establece el delegado que actualizará el modelo a partir de los datos recibidos en la respuesta a esta petición.
        /// </summary>
        /// <param name="hand">Función que actualiza los datos del modelo.</param>
        public void setDelegate(CustomProcessRequestHandler hand)
        {
            delegadoProcesarRespuesta = hand;
        }

        /// <summary>
        /// Función que procesa una respuesta y comprueba si corresponde a esta petición. En caso de que si corresponda extrae los
        /// datos recividos en ella y llama a la función delegada creada por el usuario (almacenada en la variable delegadoProcesarRespuesta)
        /// de manerea que es la función del usuario la que actualiza el modelo según sea necesario.
        /// </summary>
        /// <param name="response">Respuesta recibida del sistema.</param>
        /// <returns>True en caso de que la respuesta perteneciese a esta petición y se haya procesado, false en caso contrario.</returns>
        public bool Processs(byte [] response)
        {
            if (prot.CheckResponse(Request, response))
            {
                byte [] data=prot.ReadDataResponse(response);
                if (delegadoProcesarRespuesta != null) { 
                    delegadoProcesarRespuesta(modelo, data);
                }
                Reset();
                return true;
            }
            return false;
        }

    }
}
