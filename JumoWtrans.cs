using System;
using System.Collections.Generic;
using System.Text;
using ModbusDirekt;

namespace JumoDevices
{
    public class JumoWtrans
    {
        public JumoWtrans(ModbusClient client, int unitId)
        {
            this.Client = client;
        }

        public ModbusClient Client { get; }

        public string GetVersion()
        {
            var reg1 = Client.ReadInputRegisters(0, 5).AsString();
            var reg2 = Client.ReadInputRegisters(0x33, 2).AsString();

            return reg1;
        }

        public float ReadRawValue(int channel)
        {
            int valSize = 2;
            int address = 0x67 + (channel - 1) * valSize;
            var reg1 = Client.ReadInputRegisters(address, 2);

            float val = reg1.AsFloat();
            ValidateFloatValue(val);
            return val;
        }

        public uint ReadUpdateTime(int channel)
        {
            var reg1 = Client.ReadInputRegisters(0x87 + (channel-1)*2, 2);
            return reg1.AsUint32();
        }

        public ushort ReadTransmitInterval(int i)
        {
            var reg1 = Client.ReadInputRegisters(0xA7, 1);
            return reg1.AsUint16();
        }

        public float ReadDisplayValue(int channel)
        {
            var addr = 0x00E7 + --channel *2;
            var reg = Client.ReadInputRegisters(addr, 2);
            var val = reg.AsFloat();
            ValidateFloatValue(val);
            return val;
        }

        public AlarmOutputValues ReadAlarmOutput(int channel)
        {
            channel--;
            var reg = Client.ReadInputRegisters(0x0107 +channel, 1);
            int alarmStatus = (int)reg.Data[0];
            return (AlarmOutputValues)alarmStatus;
        }

        static void ValidateFloatValue(float input)
        {
            var val = input / 1e37;
            string error = null;
            switch (val)
            {
                case 1f: error = "Underrange"; break;
                case 2f: error = "Overrange"; break;
                case 3f: error = "No valid input value"; break;
                case 4f:error = "Division by zero.";break;
                case 5f: error = "Math error."; break;
                case 6f: error = "Invalid terminal temperature of thermocouple."; break;
                case 7f: error = "Still no minimum value (drag indicator)."; break;
                case -7f: error = "Still no maximum value (drag indicator)."; break;
                case 8f: error = "Integrator or statistics destroyed."; break;
                case 9f: error = "Radio timeout"; break;
            }
            if(error != null)
            {
                throw new WTransErrorException(error);
            }
        }

        public uint ReadPowerOnTime(int channel)
        {
            var addr = 0x03d5 + --channel * 2;
            var reg = Client.ReadInputRegisters(addr, 2);
            return reg.AsUint32();
        }

        public uint ReadTransmitterID(int channel)
        {
            var addr = 0x03B5 + --channel * 2;
            var reg = Client.ReadHoldingRegisters(addr, 2);
            return reg.AsUint32();
        }
    }

    public enum AlarmOutputValues
    {
        NoAlarms = 0,
        RadioTimeout = 1,
        AlarmMonitoring1 = 2,
        AlarmMonitoring2 = 4,
        LowBattTransmittter = 8
    }

    class WTransErrorException : Exception
    {
        public WTransErrorException(string message) : base(message)
        {
        }
    }
}
