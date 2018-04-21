using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace PortoSchool.Libs
{
    public class RTC_DS3231
    {
        I2cDevice _device;
        bool _initComplete;

        public RTC_DS3231(Action onComplete) : this(0x68, onComplete)
        {
        }

        public RTC_DS3231(int address, Action onComplete = null)
        {
            Init(address, onComplete);
        }

        async void Init(int address, Action onComplete)
        {
            var advancedQuerySyntaxString = I2cDevice.GetDeviceSelector();
            var controllerDeviceIds = await DeviceInformation.FindAllAsync(advancedQuerySyntaxString);
            I2cConnectionSettings connectionSettings = new I2cConnectionSettings(address);
            connectionSettings.BusSpeed = I2cBusSpeed.StandardMode;

            if (controllerDeviceIds.Count==0)
                return;
            _device = await I2cDevice.FromIdAsync(controllerDeviceIds[0].Id, connectionSettings);
            _initComplete = true;
            if (onComplete != null)
            {
                onComplete();
            }
        }

        public DateTime? ReadTime()
        {
            if (!_initComplete)
                return null;
            byte[] readBuffer = new byte[0x13];
            _device.WriteRead(new byte[] { 0x00 }, readBuffer);

            int seconds = BcdToInt(readBuffer[0]);
            int minutes = BcdToInt(readBuffer[1]);
            bool is24HourCock = (readBuffer[2] >> 0x6) != 1;
            int hours;
            if (is24HourCock)
                hours = (readBuffer[2] & 0xF) + ((readBuffer[2] >> 4) & 0x1) * 10 + ((readBuffer[2] >> 0x5) * 20);
            else
                hours = (readBuffer[2] & 0xF) + ((readBuffer[2] >> 4) & 0x1) * 10 + ((readBuffer[2] >> 0x5) * 12);
            int day = BcdToInt(readBuffer[3]);
            int date = BcdToInt(readBuffer[4]);
            int months = BcdToInt((byte)(readBuffer[5] & (byte)0x3f));
            int year = BcdToInt(readBuffer[6]);
            float temperature = (float)BcdToInt(readBuffer[11]) + ((float)(readBuffer[11] >> 0x6)) * 0.25f;
            return new DateTime(2000 + year, months, date, hours, minutes, seconds);
        }

        public float ReadTemperature()
        {
            byte[] buffer = new byte[2];
            _device.WriteRead(new byte[] { 0x11 }, buffer);
            float temperature = (float)buffer[0] + ((float)(buffer[1] >> 6) / 4f);
            return temperature;
        }

        public void WriteTime(DateTime dateTime)
        {
            byte[] buffer = new byte[8];
            int offset = 0;
            buffer[offset++] = 0;
            buffer[offset++] = IntToBcd(dateTime.Second);
            buffer[offset++] = IntToBcd(dateTime.Minute);
            buffer[offset++] = IntToBcd(dateTime.Hour);
            buffer[offset++] = (byte)dateTime.DayOfWeek;
            buffer[offset++] = IntToBcd(dateTime.Day);
            buffer[offset++] = IntToBcd(dateTime.Month);
            buffer[offset++] = IntToBcd(dateTime.Year % 100);
            _device.Write(buffer);
        }

        static int BcdToInt(byte bcd)
        {
            int retVal = (bcd & 0xF) + ((bcd >> 4) * 10);
            return retVal;
        }

        static byte IntToBcd(int v)
        {
            var retVal = (byte)((v % 10) | (v / 10) << 0x4);
            return retVal;
        }
    }
}
