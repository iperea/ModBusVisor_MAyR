using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IIOProtocol
{
    public interface IIOProtocolInterface
    {
        bool CheckResponse(byte[] req, byte[] data);
        byte[] ReadDataResponse(byte[] data);
        byte[] ReadWord(string dir, int count);
        byte[] ReadBit(string dir, int count);
        byte[] WriteBit(string dir, int count, bool [] data);
        byte[] WriteWord(string dir, int count, byte [] data);
        byte[] GetCheckSum(byte[] data);
    }
}
