using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hantleDispenser.Domain
{
    public static class Persistence
    {
        public static string USERS { get { return @"
            [
                {""username"": ""2lOpbOyr0oH68k8jwWK46t/nK1iBbnYl52RZ7vtRdDo="", ""pwd"": ""iM7MSM7BqDwy2nKEF+vy12N9KQT4oQkWRS3xkdyefuY="", ""role"": ""root""},
                {""username"": ""q3zOIiLmJC3JYmwwPSm7jeolLKF5TmDSxYFGoIaonKc="", ""pwd"": ""GwfHtbBHehXkwMeORJqP9Jkt9+GsF9ctmc1LZkEsP/U="", ""role"": ""peon""}
            
            ]
        "; } }
        public static List<string> COM_LIST
        {
            get
            {
                return SerialPort.GetPortNames().ToList();
            }
        }
        //public static List<string> COM_LIST
        //{
        //    get
        //    {
        //        return new List<string> { "COM1","COM2","COM3"};
        //    }
        //}
        public static List<string> DENOMINATIONS_LIST { get { return new List<string> { "$10,000 - $2000", "$20,000 - $10,000 - $2000", "$50,000 - $20,000 - $2000" }; } }
        public static string CopyRight
        {
            get
            {
                return "CopyRight © 2024 Todos los derechos reservados.\r\n\r\nEsta aplicación fue creada con el propósito de proporcionar apoyo y utilidad a sus usuarios. Desarrollada por:\r Donovan Ramirez y Juan Bohórquez.\r\n";
            }
        }

    }
}
