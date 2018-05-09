using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IIOProtocol;
using ModBusProtocol;

namespace IProtocolModBusTCP
{
    

    public class Protocol_MB_TCP : IIOProtocolInterface
    {

        ModBus mb=new ModBus();

        UInt16 Transid = 0x0000;

        /// <summary>
        /// Permite leer palabras de memoria, en el caso de Modbus son de 16 bits.
        /// </summary>
        /// <param name="dir">Formato: DDRRRRT DD es la dirección del esclavo, RRRR es la dirección de memoría del registro a leer
        /// y T es el tipo de palabra (R si es Registro o I si es una entrada analogia)</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] ReadWord(string dir, int count)
        {
            byte[] f;
            if (dir.Substring(6, 1).Equals("R"))
            {
                f = mb.ReadRWord(dir.Substring(2, 4), count);
            }
            else
            {
                f = mb.ReadIWord(dir.Substring(2, 4), count);
            }
            return CreateSream(dir.Substring(0, 2), f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir">Formato: DDRRRRT donde:  DD es la dirección del esclavo, RRRR es la dirección de memoría del registro a leer 
        /// (recuerda que un byte se representa con dos carácteres) y T es el tipo de palabra (C si es Coil (bobina) o D si es una entrada digital)</param>
        /// <param name="count"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] ReadBit(string dir, int count)
        {
            byte[] f;
            if (dir.Substring(6, 1).Equals("C"))
            {
                f = mb.ReadCBit(dir.Substring(2, 4), count);
            }
            else
            {
                f = mb.ReadIBit(dir.Substring(2, 4), count);
            }
            return CreateSream(dir.Substring(0, 2), f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] WriteBit(string dir, int count, bool[] data)
        {
            byte[] f;
            if (count > 1)
            {
                f = mb.WriteMBits(dir.Substring(2, 4), count, data);
            }
            else
            {
                f = mb.WriteBit(dir.Substring(2, 4), count, data);
            }

            return CreateSream(dir.Substring(0, 2), f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] WriteWord(string dir, int count, byte[] data)
        {
            byte[] f;
            if (count > 1)
            {
                f = mb.WriteMWord(dir.Substring(2, 4), count, data);
            }
            else
            {
                f = mb.WriteSWord(dir.Substring(2, 4), count, data);
            }
            return CreateSream(dir.Substring(0, 2), f);
        }

        /// <summary>
        /// Comprueba que la respuesta pertenezca a la petición req, y que su checksum sea correcto.
        /// </summary>
        /// <param name="req">Petición original.</param>
        /// <param name="data">Respuesta recibida.</param>
        /// <returns>True si la respuesta pertence a la petición req, false en caso contrario.</returns>
        public bool CheckResponse(byte[] req, byte[] data)
        {
            //Se verifica que corresponda el id de trama y el protocolo.
            bool dev = true;
            for (int i = 0; i < 4; i++)
            {
                //Si se cumple esta condición es que no coincide alguno de los bytes de trama o protocolo.
                if (req[i] != data[i])
                {
                    dev = false;
                }
            }
            return dev;
        }

        /// <summary>
        /// Permite extraer los datos de una respuesta.
        /// </summary>
        /// <param name="data">Trama de respuesta recibida.</param>
        /// <returns>Devuel los datos contenidos en la respuesta a unta trama de lectura, 
        /// ó null en caso de que no se trate de una respuesta a una petición de lectura.</returns>
        public byte[] ReadDataResponse(byte[] data)
        {

            MemoryStream dataResp = new MemoryStream();

            //Se extrae el frame ModBus eliminando cabecera MDA
            //Esta es la parte que depende de ModBusTCP
            for (int i = 7; i < data.Length; i++)
            {
                dataResp.WriteByte(data[i]);
            }

            //Se extraen los datos (Esto es lo que depende solo del protocolo Modbus de nivel de aplicación).
            byte[] res = mb.ReadDataResponse(dataResp.ToArray());

            return res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        private byte[] CreateSream(string dir, byte[] frame)
        {

            UInt16 idt;
            //Se bloquea el acceso a otro hilo en el mismo instante
            //para que dos peticiones no salgan con la misma respuesta
            lock (this)
            {
                idt = Transid;
                Transid++;
            }

            //Creando cabecera MBA
            byte[] adu = new byte[frame.Length + 7];

            //ID de transacción (2 bytes)
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:x4}", idt);

            adu[0] = Byte.Parse(sb.ToString().Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            adu[1] = Byte.Parse(sb.ToString().Substring(2, 2), System.Globalization.NumberStyles.HexNumber);


            //Protocolo (2 bytes)
            adu[2] = 0x00;//Protocolo Modbus
            adu[3] = 0x00;//Protocolo Modbus

            //Longitud del mensaje
            sb = new StringBuilder();
            sb.AppendFormat("{0:x4}", frame.Length+1); //se suma uno por el byte de id de esclavo

            adu[4] = Byte.Parse(sb.ToString().Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            adu[5] = Byte.Parse(sb.ToString().Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            
            //ID de Esclavo (1 bytes)
            adu[6] = Byte.Parse(dir);

            for (int i = 0; i < (frame.Length); i++)
            {
                adu[i + 7] = frame[i];
            }

            return adu;
        }


        /// <summary>
        /// Función que genera el CheckSum del dato, en este caso un CRC de 16 bits para ModBus.
        /// </summary>
        /// <param name="data">Cadena de datos a la que hay que hacer el CheckaSum</param>
        /// <returns>CheckSum calculado.</returns>
        public byte[] GetCheckSum(byte[] data)
        {
            throw new NotImplementedException("El mecanismo ModBusTCP no utiliza codigo re detección de errores");
        }
    }
}
