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
        public static string WithoutPhysicalConnection = "No se logró conectar, Se ha perdido la conexíón fisica con el puerto COM del dispensador.";
        public static string CassettesDoNotMatch = "El número de cassetes agregados especificados NO SON IGUALES A LOS FISICOS.";
        public static string SensorTimeOut = "La respuesta del Sensor a tardado más de lo esperado, valide que todo es OK e intente nuevamente.";
        public static string RejectBoxOpen = "La caja de rechazo está mal puesta, Ingrese adecuadamente la caja de rechazo y asegurese de cerrarla correctamente con la llave.";
        public static string CasseteDisconnected = "El baúl {0} está desconectado, Por favor comuníquese con servicio técnico de E-city.";
        public static string CasseteDismounted = "El baúl {0} está desmontado o mal colocado, Ingrese adecuadamente el baúl";
        public static string CasseteBadLoaded = "El baúl {0} está mal cargado, Asegurese de que no hay ningún billete mal posicionado a la salida del baúl";
        public static string SensorCISOpen = "El compartimiento de la parte posterior del dispensador está abierto.";
        public static string DispendTimeOut = "La respuesta del Baúl a tardado más de los esperado, intentelo nuevamente.   ";
        public static string DispendSuccess = OperationManager.CoinsValue > 0 ? "Devuelta correcta, valor a devolver en monedas: {0}": "Devuelta correcta.";
        public static string LastCassetteWithoutMoney = $"El último baúl se quedó sin billetes, valor a devolver en monedas {OperationManager.CoinsValue}.";
        public static string FailMaxEject = $"No se pudo desatascar, llamar a Julio. Valor a devolver en monedas: {OperationManager.CoinsValue}.";
        public static string StatusUndetermined = $"No se logró determinar el estado del dispensador, ocurrió un error no determinado, valor a devolver en monedas: {OperationManager.CoinsValue}";
        public static string FailEject = "No se logró desatascar, Intentelo nuevamente";
        public static string EjectTimeOut = "No se logró desatascar, Error la respuesta a tardado más de lo esperado, Intento nuevamente.";
        public static string ErrorInitialize = "No se logró inicializar el dispensador: {0}";
        public static string ErrorDispendQuantities = "No se logró dispensar: {0}";
        public static string JAM = "Hay un atasco en el dispensador o algo está obstaculizado el paso de billetes. Revise que no halla obstaculos o polvo excesivo en el recorreido de los billetes.";

    }

}
