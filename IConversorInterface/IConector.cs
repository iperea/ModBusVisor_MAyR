using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace IConectorInterface
{

    public delegate void ReceiveEventHandler();
    public interface IConector
    {

        /// <summary>
        /// Permite realizar una conexión con la configuración introducida.
        /// </summary>
        /// <param name="conf">Configuración de la conexión.</param>
        void Connect(string conf = null);

        /// <summary>
        /// Cierra la conexión del sockuet.
        /// </summary>
        void Close();

        /// <summary>
        /// Permite realizar el envío de una trama.
        /// </summary>
        /// <param name="trama">Trama a enviar.</param>
        /// <returns>Trama enviada con cabeceras añadiadas por el conector.</returns>
        byte[] Enviar(byte[] trama);
        
        /// <summary>
        /// Inicia la recepción de datos en un segundo plano.
        /// </summary>
        void StartReceive();


        /// <summary>
        /// Inicia la recepción de datos en segundo plano.
        /// </summary>
        void StopReceive();

        /// <summary>
        /// Permite leer los datos recibidos.
        /// </summary>
        /// <returns>Lista de tramas de respuesta recibidas, hasta el momento o null en caso de no haber recibido datos.</returns>
        List<byte[]> getData();

        /// <summary>
        /// Permite saber si el conector está conectado a un servidor.
        /// </summary>
        /// <returns></returns>
        bool isConected();

        /// <summary>
        /// True si el objeto está leyendo del canal y false en caso contrario.
        /// </summary>
        bool isReciving { get; set; }

        /// <summary>
        /// Permite establecer una función a la que se llamará una vez que se han recibido datos, o bien el timeout de lectura ha expirado.
        /// </summary>
        /// <param name="funcionDelegada">Función delegada a la que se llamará llamada.</param>
        void SetReceiveEventHandler(IConectorInterface.ReceiveEventHandler funcionDelegada);
    }
}
