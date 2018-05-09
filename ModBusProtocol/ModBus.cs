using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ModBusProtocol
{
    public class ModBus
    {

        public const byte RCM = 0x01;
        public const byte RDM = 0x02;
        public const byte RRM = 0x03;
        public const byte RIM = 0x04;
        public const byte WCS = 0x05;
        public const byte WRS = 0x06;
        public const byte WCM = 0x0F;
        public const byte WRM = 0x10;


        /// <summary>
        /// Comando Modbus 0x01 Leer una ó multiples Bobinas.
        /// </summary>
        /// <param name="dir">Direccion de memoria inicial formato 0000-FFFF valor hexadecimal representado en formato texto.</param>
        /// <param name="count">Cantidad de entradas a leer.</param>
        public byte[] ReadCBit(string dir, int count)
        {
            return CreateSream(RCM, dir, null, count);
        }

        /// <summary>
        /// Comando Modbus 0x02 Leer una ó multiples Entradas digitales.
        /// </summary>
        /// <param name="dir">Direccion de memoria inicial formato 0000-FFFF valor hexadecimal representado en formato texto.</param>
        /// <param name="count">Cantidad de entradas a leer.</param>
        public byte[] ReadIBit(string dir, int count)
        {
            return CreateSream(RDM, dir, null, count);
        }


        /// <summary>
        /// Comando Modbus 0x04 Leer una ó multiples Entrada Analogica.
        /// </summary>
        /// <param name="dir">Direccion de memoria inicial formato 0000-FFFF valor hexadecimal representado en formato texto.</param>
        /// <param name="count">Cantidad de entradas a leer.</param>
        /// <returns>Frame Modbus que envía dicho comando.</returns>
        public byte[] ReadIWord(string dir, int count)
        {
            return CreateSream(RIM, dir, null, count);
        }

        /// <summary>
        /// Comando Modbus 0x03 Leer uno ó multiples Registros.
        /// </summary>
        /// <param name="dir">Direccion de memoria inicial formato 0000-FFFF valor hexadecimal representado en formato texto.</param>
        /// <param name="count">Cantidad de entradas a leer.</param>
        public byte[] ReadRWord(string dir, int count)
        {
            return CreateSream(RRM, dir, null, count);
        }


        /// <summary>
        /// Comando Modbus 0x05 Escribir una Bobina.
        /// </summary>
        /// <param name="dir">Direccion de memoria inicial formato 0000-FFFF valor hexadecimal representado en formato texto.</param>
        /// <param name="count">Cantidad de entradas a leer.</param>
        public byte[] WriteBit(string dir, int count, bool[] data)
        {
            byte[] datab = null;
            datab = new byte[2];
            datab[0] = (byte)((data[0] == true) ? 0xFF : 0x00);
            datab[1] = (byte)((data[0] == true) ? 0x00 : 0x00);
            return CreateSream(WCS, dir, datab, count);
        }


        /// <summary>
        /// Comando Modbus 0x06 Escribir un único Registro.
        /// </summary>
        /// <param name="dir">Direccion de memoria inicial formato 0000-FFFF valor hexadecimal representado en formato texto.</param>
        /// <param name="count">Cantidad de entradas a leer.</param>
        public byte[] WriteSWord(string dir, int count, byte[] data)
        {
            return CreateSream(WRS, dir, data, count);
        }


        /// <summary>
        /// Comando Modbus 0x0F Escribir multiples Bobinas.
        /// </summary>
        /// <param name="dir">Direccion de memoria inicial formato 0000-FFFF valor hexadecimal representado en formato texto.</param>
        /// <param name="count">Cantidad de entradas a leer.</param>
        public byte[] WriteMBits(string dir, int count, bool[] data)
        {
            byte[] datab = null;
            count = 0;
            if (count > 0)
            {
                count = (count >> 3) + 1;
            }
            datab = new byte[count];
            for (int i = 0; i < data.Length; i = i++)
            {
                datab[(i >> 3)] = (byte)(datab[(i >> 3)] | (byte)((data[i] == true) ? (0x01 << (i & 0x7)) : 0x00));
            }

            return CreateSream(WCM, dir, datab, count);
        }


        /// <summary>
        /// Comando Modbus 0x10 Escribir multiples Registros.
        /// </summary>
        /// <param name="dir">Direccion de memoria inicial formato 0000-FFFF valor hexadecimal representado en formato texto.</param>
        /// <param name="count">Cantidad de entradas a leer.</param>
        public byte[] WriteMWord(string dir, int count, byte[] data)
        {
            return CreateSream(WRM, dir, data, count);
        }

        /// <summary>
        /// Permite extraer los datos de una respuesta.
        /// </summary>
        /// <param name="data">Trama de respuesta recibida.</param>
        /// <returns>Devuel los datos contenidos en la respuesta a unta trama de lectura, 
        /// ó null en caso de que no se trate de una respuesta a una petición de lectura.</returns>
        public byte[] ReadDataResponse(byte[] data)
        {
            byte f = data[0];
            byte[] res = null;
            MemoryStream dataResp = new MemoryStream();
            switch (f)
            {
                case RCM:
                case RDM:
                case RIM:
                case RRM:
                    //Se extraen los datos. El primer byte es el codigo de función y el segundo es el nrq.
                    //con la longitud del frame ya conocemos el nrq por lo que no necesitamos procesarlo.
                    //En otros lenguajes si necesitariamos procesarlo.
                    for (int i = 2; i < data.Length; i++)
                    {
                        dataResp.WriteByte(data[i]);
                    }
                    res = dataResp.ToArray();
                    break;
                case WCS:
                case WRS:
                case WCM:
                case WRM:
                    //Se extraen los datos. El primer byte es el codigo de función y el segundo es el nrq.
                    //con la longitud del frame ya conocemos el nrq por lo que no necesitamos procesarlo.
                    //En otros lenguajes si necesitariamos procesarlo.
                    for (int i = 1; i < 3; i++)
                    {
                        dataResp.WriteByte(data[i]);
                    }
                    res = dataResp.ToArray();
                    break;


            }
            return res;
        }

        /// <summary>
        /// Creación del Stream de bytes correspondiente a una función de lectura/escritura Modbus
        /// </summary>
        /// <param name="funcion">Codigo de Función Modbus .</param>
        /// <param name="dir">Dirección de memoria en formato 0000-FFFF</param>
        /// <param name="data">Datos a escribir en la trama, null en caso de no ser necesario.</param>
        /// <param name="count">Cantidad de bytes que quedan en la trama, este parámetro debe estar precalculado.</param>
        /// <returns></returns>
        public byte[] CreateSream(byte funcion, string dir, byte[] data, int count)
        {
            //Esta función está diseñada para construir un stream de bytes que corresponde con una trama de Lectura/Escritura en Modbus
            //Pero está pensada de una forma en que es facil de ampliar a nuevos tipos de trama.
            MemoryStream ADU = new MemoryStream();

            //Funcion
            ADU.WriteByte(funcion);

            //Dirección de memoria inicial
            //Lo tienen todas las tramas de Lectura/Escritura
            switch (funcion)
            {
                case RCM:
                case RDM:
                case RIM:
                case RRM:
                case WRS:
                case WCS:
                case WCM:
                    for (int i = 0; i < dir.Length; i=i+2)
                    {
                        ADU.WriteByte(Byte.Parse(dir.Substring(i, 2), System.Globalization.NumberStyles.HexNumber));
                    }
                    break;
            }


            //Datos a escribir o cantidad de datos a leer
            switch (funcion)
            {
                case RCM:
                case RDM:
                case RIM:
                case RRM:
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("{0:x4}", count);
                    //Datos o cantidad de registros a leer
                    ADU.WriteByte(Byte.Parse(sb.ToString().Substring(0, 2), System.Globalization.NumberStyles.HexNumber));
                    ADU.WriteByte(Byte.Parse(sb.ToString().Substring(2, 2), System.Globalization.NumberStyles.HexNumber));
                    break;

                case WRS:
                case WCS:
                    //Datos a escribir
                    ADU.Write(data, 0, data.Length);
                    break;

                case WCM:
                    //Datos o cantidad de registros a escribir
                    //División por 8
                    if (count > 0)
                    {
                        count = (count >> 3) + 1;
                    }
                    break;
            }

            //Por último se añade el final para las tramas de escritura de multiples registros
            switch (funcion)
            {
                case WCM:
                case WRM:
                    
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("{0:x2}", count);

                    //Datos o cantidad de registros a escribir
                    ADU.WriteByte(Byte.Parse(sb.ToString()));

                    //Cantidad de bytes 
                    int Nrq = count * 2;
                    sb = new StringBuilder();
                    sb.AppendFormat("{0:x2}", Nrq);

                    ADU.WriteByte(Byte.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber));

                    //Datos a escribir
                    ADU.Write(data, 0, data.Length);
                    break;
            }

            return ADU.ToArray();
        }
    }
}
