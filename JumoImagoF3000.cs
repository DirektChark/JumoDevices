using System;
using System.Collections.Generic;
using System.Text;
using ModbusDirekt;

namespace JumoDevices
{
    public class JumoImagoF3000
    {
        public ModbusClient Client { get; }

        public JumoImagoF3000(ModbusDirekt.ModbusClient client)
        {
            this.Client = client;
        }


        public string GetVersion()
        {
            var reg1 = Client.ReadInputRegisters(0, 5).GetString();
            var reg2 = Client.ReadInputRegisters(0x33, 2).GetString();

            return reg1;
        }

        public float ReadFloat(int addr)
        {
            return Client.ReadInputRegisters(addr, 2).GetFloat();
        }


        public float ReadFilteredActualValue(int v)
        {
            return Client.ReadInputRegisters(0x5b+v*2, 2).GetFloat();
        }
        public float ReadModbusActualValue(int v)
        {
            return Client.ReadInputRegisters(0x71 + v*2, 2).GetFloat();
        }

        public float ReadTerminalTemp()
        {
            return Client.ReadInputRegisters(0x6b, 2).GetFloat();
        }

        public float ReadCalculatedHumidity()
        {
            return Client.ReadInputRegisters(0x6d, 2).GetFloat();
        }

        public float ReadActualCore()
        {
            return Client.ReadInputRegisters(0x6f, 2).GetFloat();
        }
        public float ReadTerminalTemp2()
        {
            return Client.ReadInputRegisters(0x81, 2).GetFloat();
        }
        public float ReadHumidity2()
        {
            return Client.ReadInputRegisters(0x83, 2).GetFloat();
        }
        public float ReadActualCore2()
        {
            return Client.ReadInputRegisters(0x85, 2).GetFloat();
        }
        public float ReadActualFValue()
        {
            return Client.ReadInputRegisters(0x87, 2).GetFloat();
        }

        public float ReadActualCValue()
        {
            return Client.ReadInputRegisters(0x89, 2).GetFloat();
        }

        

        /// <summary>
        /// Reads Modbus address 0x8B, float, r/o
        /// (Prgram Source : setpoint chamber delta.)
        /// </summary>
        public float ReadSPChamberDelta()
        {
            return Client.ReadInputRegisters(0x8b, 2).GetFloat();
        }

        /// <summary>
        /// Reads Modbus address 0x8D, float, r/o
        /// (Prgram Source : setpoint humidity indicatin)
        /// </summary>
        public float ReadSPrHInd()
        {
            return Client.ReadInputRegisters(0x8D, 2).GetFloat();
        }

        /// <summary>
        /// Reads Modbus address 0x8F, float, r/o
        /// (Prgram Source : setpoint chamber)
        /// </summary>
        public float ReadSPChamber()
        {
            return Client.ReadInputRegisters(0x8F, 2).GetFloat();
        }

        public float ReadSPrH()
        {
            return Client.ReadInputRegisters(0x91, 2).GetFloat();
        }

        /// <summary>
        /// Reads Modbus address 0x93, float, r/o
        /// (Prgram Source : setpoint core)
        /// </summary>
        public float ReadSPCore()
        {
            return Client.ReadInputRegisters(0x93, 2).GetFloat();
        }


        public string ReadBinaryAlarmText()
        {
            return TrimData(Client.ReadInputRegisters(0x0241, 21).GetString());
        }
        public string ReadProgramName()
        {
            string data = Client.ReadInputRegisters(0x0257, 17).GetString();

            return TrimData(data);
        }
        public string ReadProcessName()
        {
            string data = Client.ReadInputRegisters(0x0260, 17).GetString();
            return TrimData(data);
        }
        public string ReadNextProcessName()
        {
            return TrimData(Client.ReadInputRegisters(0x0269, 17).GetString());
        }

        public string ReadAllProdDataText()
        {
            var sb = new StringBuilder();
            string data = "";
            for(int i = 0; i < 8; i++)
            {
                data = Client.ReadInputRegisters(0x0272 + i * 17, 17).GetString();
                sb.Append(data);

            }

            return sb.ToString();
        }
        private static string TrimData(string data)
        {
            return data.Substring(0, data.IndexOf('\0'));
        }

        /// <summary>
        /// Reads Modbus address 0xAE, word, r/w
        /// (Prgram Source : actual program-no.)
        /// </summary>
        public int ReadProgramNo()
        {
            return (int)Client.ReadInputRegisters(0xAE, 1).GetUint16();
        }

        /// <summary>
        /// Reads Modbus address 0xAF, word, r/o
        /// (Prgram Source : actual section-no.)
        /// </summary>
        public int ReadStepNo()
        {
            return (int)Client.ReadInputRegisters(0xAF, 1).GetUint16();
        }

        /// <summary>
        /// Reads Modbus address 0xB0, word, r/o
        /// (Prgram Source : max. nsection-no.)
        /// </summary>
        public int ReadStepCount()
        {
            return (int)Client.ReadInputRegisters(0xB0, 1).GetUint16();
        }
    }
}
