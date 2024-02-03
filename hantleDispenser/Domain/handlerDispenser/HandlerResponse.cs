using ControlzEx.Standard;
using hantleDispenser.Domain;
using HantleDispenserAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace handlerDispenser.Domain
{
    public static class HandlerResponse
    {
        public static string Success = "OK";
        public static string WithoutConnection = "No se logró conectar, valide que el numero de puerto subminitrado sea correcto e intente inicializar.";
        public static string CassettesDoNotMatch = "El número de cassetes agregados especificados NO SON IGUALES A LOS FISICOS.";
        public static string SensorTimeOut = "La respuesta del Sensor a tardado más de lo esperado, valide que todo es OK e intente nuevamente.";
        public static string DispendTimeOut = "";
        public static string DispendSuccess = OperationManager.CoinsValue > 0 ? "Devuelta correcta, valor a devolver en monedas: {0}": "Devuelta correcta.";
        public static string LastCassetteWithoutMoney = $"El último baúl se quedó sin billetes, valor a devolver en monedas {OperationManager.CoinsValue}.";
        public static string FailMaxEject = $"No se pudo desatascar, llamar a Julio. Valor a devolver en monedas: {OperationManager.CoinsValue}.";
        public static string StatusUndetermined = $"No se logró determinar el estado del dispensador, ocurrió un error no determinado, valor a devolver en monedas: {OperationManager.CoinsValue}";
        public static string FailEject = "No se logró desatascar, Intentelo nuevamente";
        public static string ErrorInitialize = "No se logró inicializar el dispensador: {0}";
        public static string ErrorDispendQuantities = "No se logró dispensar: {0}";
        public static string ErrorSensor = "Ocurrió un Error a la hora de obtener los sensores: {0}";

    }

}
