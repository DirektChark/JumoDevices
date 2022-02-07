using ModbusDirekt;
using ModbusDirekt.Modbus;
using System;
using System.Text;

namespace JumoDevices
{
    public class JumoImagoF3000
    {
        public ModbusDirekt.IModbusClient Client { get; }

        public JumoImagoF3000(IModbusClient client)
        {
            Client = client;
        }


        public string GetVersion()
        {
            string reg1 = Client.ReadInputRegisters(0, 5).AsString();
            string reg2 = Client.ReadInputRegisters(0x33, 2).AsString();

            return reg1;
        }

        public float ReadFloat(int addr)
        {
            return Client.ReadInputRegisters(addr, 2).AsFloat();
        }


        public float ReadFilteredActualValue(int v)
        {
            return Client.ReadInputRegisters(0x5b + v * 2, 2).AsFloat();
        }
        public float ReadModbusActualValue(int v)
        {
            return Client.ReadInputRegisters(0x71 + v * 2, 2).AsFloat();
        }

        public float ReadTerminalTemp()
        {
            return Client.ReadInputRegisters(0x6b, 2).AsFloat();
        }

        public float ReadCalculatedHumidity()
        {
            return Client.ReadInputRegisters(0x6d, 2).AsFloat();
        }

        public float ReadActualCore()
        {
            return Client.ReadInputRegisters(0x6f, 2).AsFloat();
        }
        public float ReadTerminalTemp2()
        {
            return Client.ReadInputRegisters(0x81, 2).AsFloat();
        }
        public float ReadHumidity2()
        {
            return Client.ReadInputRegisters(0x83, 2).AsFloat();
        }
        public float ReadActualCore2()
        {
            return Client.ReadInputRegisters(0x85, 2).AsFloat();
        }
        public float ReadActualFValue()
        {
            return Client.ReadInputRegisters(0x87, 2).AsFloat();
        }

        public float ReadActualCValue()
        {
            return Client.ReadInputRegisters(0x89, 2).AsFloat();
        }



        /// <summary>
        /// Reads Modbus address 0x8B, float, r/o
        /// (Prgram Source : setpoint chamber delta.)
        /// </summary>
        public float ReadSPChamberDelta()
        {
            return Client.ReadInputRegisters(0x8b, 2).AsFloat();
        }

        /// <summary>
        /// Reads Modbus address 0x8D, float, r/o
        /// (Prgram Source : setpoint humidity indicatin)
        /// </summary>
        public float ReadSPrHInd()
        {
            return Client.ReadInputRegisters(0x8D, 2).AsFloat();
        }

        /// <summary>
        /// Reads Modbus address 0x8F, float, r/o
        /// (Prgram Source : setpoint chamber)
        /// </summary>
        public float ReadSPChamber()
        {
            return Client.ReadInputRegisters(0x8F, 2).AsFloat();
        }

        public float ReadSPrH()
        {
            return Client.ReadInputRegisters(0x91, 2).AsFloat();
        }

        /// <summary>
        /// Reads Modbus address 0x93, float, r/o
        /// (Prgram Source : setpoint core)
        /// </summary>
        public float ReadSPCore()
        {
            return Client.ReadInputRegisters(0x93, 2).AsFloat();
        }


        public string ReadBinaryAlarmText()
        {
            return TrimData(Client.ReadInputRegisters(0x0241, 21).AsString());
        }
        public string ReadProgramName()
        {
            string data = Client.ReadInputRegisters(0x0257, 17).AsString();

            return TrimData(data);
        }
        public string ReadProcessName()
        {
            string data = Client.ReadInputRegisters(0x0260, 17).AsString();
            return TrimData(data);
        }
        public string ReadNextProcessName()
        {
            return TrimData(Client.ReadInputRegisters(0x0269, 17).AsString());
        }

        public string ReadAllProdDataText()
        {
            var sb = new StringBuilder();
            string data = "";
            for (int i = 0; i < 8; i++)
            {
                data = Client.ReadInputRegisters(0x0272 + i * 17, 17).AsString();
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
            return (int)Client.ReadInputRegisters(0xAE, 1).AsUint16();
        }

        /// <summary>
        /// Reads Modbus address 0xAF, word, r/o
        /// (Prgram Source : actual section-no.)
        /// </summary>
        public int ReadStepNo()
        {
            return (int)Client.ReadInputRegisters(0xAF, 1).AsUint16();
        }

        /// <summary>
        /// Reads Modbus address 0xB0, word, r/o
        /// (Prgram Source : max. nsection-no.)
        /// </summary>
        public int ReadStepCount()
        {
            return (int)Client.ReadInputRegisters(0xB0, 1).AsUint16();
        }


        public void PrintControllerValues(StringBuilder sb)
        {
            var actualFiltered = new InputRegisters[4];
            var sp = new InputRegisters[4];
            var stellgrad = new InputRegisters[4];

            actualFiltered[0] = Client.ReadInputRegisters(0xf7, 4);
            actualFiltered[1] = Client.ReadInputRegisters(0x124, 4);
            actualFiltered[2] = Client.ReadInputRegisters(0x151, 4);
            actualFiltered[3] = Client.ReadInputRegisters(0x17e, 4);

            sp[0] = Client.ReadInputRegisters(0xfb, 4);
            sp[1] = Client.ReadInputRegisters(0x128, 4);
            sp[2] = Client.ReadInputRegisters(0x155, 4);
            sp[3] = Client.ReadInputRegisters(0x182, 4);

            stellgrad[0] = Client.ReadInputRegisters(0xfe, 4);
            stellgrad[1] = Client.ReadInputRegisters(0x12b, 4);
            stellgrad[2] = Client.ReadInputRegisters(0x158, 4);
            stellgrad[3] = Client.ReadInputRegisters(0x185, 4);

            for (int i = 0; i < 4; i++)
            {
                string v1 = actualFiltered[i].AsFloat().ToString();
                string v2 = sp[i].AsFloat().ToString();
                string v3 = stellgrad[i].AsFloat().ToString(); ;

                sb.AppendLine();
                sb.AppendLine($"----  Controller {1 + i}  ----");
                sb.AppendLine($"Actual filtered: {v1}");
                sb.AppendLine($"SP: {v2}");
                sb.AppendLine($"Stellgrad: {v3}");
            }
        }

        public float ReadC1Stellgrad()
        {
            var v = Client.ReadInputRegisters(0xFE, 4);
            return v.AsFloat();
        }

        public int ReadCoolingOutput()
        {
            var v = Client.ReadInputRegisters(0x207, 1);
            return v.AsUint16();
        }

        public float ReadC2Stellgrad()
        {
            var v = Client.ReadInputRegisters(0x12b, 4);
            return v.AsFloat();
        }

        public void PrintMathValues(StringBuilder sb)
        {

            float m1 = Client.ReadInputRegisters(0x1e3, 4).AsFloat();
            float m2 = Client.ReadInputRegisters(0x1ec, 4).AsFloat();
            float m3 = Client.ReadInputRegisters(0x1f5, 4).AsFloat();
            float m4 = Client.ReadInputRegisters(0x1fe, 4).AsFloat();

            sb.AppendLine();
            sb.AppendLine("----  Math  ----");
            sb.AppendLine($"Math 1: {m1}");
            sb.AppendLine($"Math 2: {m2}");
            sb.AppendLine($"Math 3: {m3}");
            sb.AppendLine($"Math 4: {m4}");

        }

        public void PrintLogicsValues(StringBuilder sb)
        {
            var lv = new InputRegisters[8];

            lv[0] = Client.ReadInputRegisters(0x0202, 1);
            lv[1] = Client.ReadInputRegisters(0x0207, 1);
            lv[2] = Client.ReadInputRegisters(0x020c, 1);
            lv[3] = Client.ReadInputRegisters(0x0211, 1);
            lv[4] = Client.ReadInputRegisters(0x0216, 1);
            lv[5] = Client.ReadInputRegisters(0x021b, 1);
            lv[6] = Client.ReadInputRegisters(0x0220, 1);
            lv[7] = Client.ReadInputRegisters(0x0225, 1);

            string[] func = new string[8] { "", "", "", "", "", "", "", "" };
            func[0] = "Relay 10 Larm";
            func[1] = "Relay 13 Cooling";
            func[4] = "Relay 16 Heat3";
            func[5] = "Relay 17 Heat4";
            func[6] = "Relay 15 Heat2";
            

            sb.AppendLine();
            sb.AppendLine("----  Logics  ----");

            for (int i = 0; i < 8; i++)
            {
                sb.AppendLine($"Logics {i + 1} ({func[i]}): {lv[i].AsUint16().ToString().PadLeft(3, ' ')}");// [{lv[i].Data[0]}] [{lv[i].Data[1]}] [{lv[i].Data[2]}] [{lv[i].Data[3]}]");
            }




        }

    }
}
