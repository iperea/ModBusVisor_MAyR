using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IIOProtocol;
using System.IO;
using ModBusProtocol;

namespace IProtocolModBusRTU
{

    public class Protocol_MB_RTU : IIOProtocolInterface
    {



        ModBus mb = new ModBus();

        /// <summary>
        /// Permite  leer palabras de memoria, en el caso de Modbus son de 16 bits.
        /// </summary>
        /// <param name="dir">Formato: DDRRRRT DD es la dirección del esclavo, RRRR es la dirección de memoría del registro a leer
        /// y T es el tipo de palabra (R si es Registro o I si es una entrada analogia)</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] ReadWord(string dir, int count)
        {
            byte [] f;
            if(dir.Substring(6,1).Equals("R")){
                f=mb.ReadRWord(dir.Substring(2, 4), count);
            }else{
                f=mb.ReadIWord(dir.Substring(2, 4), count);
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
            byte [] f;
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
        public byte[] WriteBit(string dir, int count, bool [] data)
        {
            byte [] f;
            if (count>1)
            {
                f = mb.WriteMBits(dir.Substring(2, 4), count,data);
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
        public byte[] WriteWord(string dir, int count, byte [] data)
        {
            byte [] f;
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
        /// <param name="req"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool CheckResponse(byte [] req, byte[] data)
        {
            //Se crea una cadena sin chekcsum para verificar que el checksum es correcto
            bool dev=true;
            byte[] d2 = new byte[data.Length - 2];
            for (int i = 0; i < d2.Length; i++)
            {
                d2[i] = data[i];
                if (i < 2)
                {
                    //Si se cumple esta condición quiere decir que no coincide o la dirección o el codigo de función.
                    if (d2[i] != data[i])
                    {
                        dev = false;
                    }
                }
            }
            if (dev)
            {
                byte[] chs = GetCheckSum(d2);
                if (chs[0] == data[data.Length - 2] && chs[1] == data[data.Length - 1])
                {
                    dev = true;
                }
                else
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
        public byte[] ReadDataResponse(byte [] data)
        {
            MemoryStream dataResp = new MemoryStream();

            //Se extrae el frame ModBus eliminando cabecera y checksum
            //Esta es la parte que depende de RTU
            for(int i=2;i<data.Length-2;i++){
                dataResp.WriteByte(data[i]);
            }

            //Se extraen los datos (Esto es lo que depende solo del protocolo Modbus de nivel de aplicación).
            byte [] res=mb.ReadDataResponse(dataResp.ToArray());

            return res;
        }

        /// <summary>
        /// Premite crear una trama Modbus RTU.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        private byte[] CreateSream( string dir, byte [] frame)
        {            
            MemoryStream ADU=new MemoryStream();

            //Direccion del esclavo
            ADU.WriteByte(Byte.Parse(dir.Substring(0, 2), System.Globalization.NumberStyles.HexNumber));

            //Y se añade al frame ModBusRTU
            ADU.Write(frame,0,frame.Length);

            //Se le añade por último el CheckSum
            byte []chs=GetCheckSum(ADU.ToArray());
            ADU.Write(chs, 0, chs.Length);
            return ADU.ToArray();
        }

        /// <summary>
        /// Función que genera el CheckSum del dato, en este caso un CRC de 16 bits para ModBus.
        /// </summary>
        /// <param name="data">Cadena de datos a la que hay que hacer el CheckaSum</param>
        /// <returns>CheckSum calculado.</returns>
        public byte[] GetCheckSum(byte[] data)
        {
            int usDataLen = data.Length;
            byte uchCRCHi = 0xFF; // high byte of CRC initialized 
            byte uchCRCLo = 0xFF; // low byte of CRC initialized 
            uint uIndex; // will index into CRC lookup table 
            uint i = 0;

            while (usDataLen > 0) 
            {
                usDataLen--;
                uIndex = (uint)(uchCRCHi ^ (data[i]));
                uchCRCHi = (byte)(uchCRCLo ^ auchCRCHi[uIndex]);
                uchCRCLo = auchCRCLo[uIndex];
                i++;
            }
            Byte[] ret = new Byte[2];
            ret[0] = uchCRCHi;
            ret[1] = uchCRCLo;
            return ret; 
        }


        // Table of CRC values for low–order byte 
        static byte[] auchCRCLo = new byte[256] {
            0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7, 0x05, 0xC5, 0xC4,
            0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
            0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD,
            0x1D, 0x1C, 0xDC, 0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
            0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32, 0x36, 0xF6, 0xF7,
            0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
            0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE,
            0x2E, 0x2F, 0xEF, 0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
            0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1, 0x63, 0xA3, 0xA2,
            0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
            0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB,
            0x7B, 0x7A, 0xBA, 0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
            0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0, 0x50, 0x90, 0x91,
            0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
            0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88,
            0x48, 0x49, 0x89, 0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
            0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83, 0x41, 0x81, 0x80,
            0x40
        };

        // Table of CRC values for high–order byte 
        static byte[] auchCRCHi = new byte[256]{
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
            0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
            0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
            0x40
        };
    }
}
