using handlerDispenser.Domain;
using HantleDispenserAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace hantleDispenser.Domain
{
    public static class OperationManager
    {
        private static CDMS_Handler _handler = CDMS_Handler.Instance;
        private static int[]? _quantities { get; set; }
        private static int _valueToDispense { get; set; } = 0;
        public static int DispensedValue { get; set; } = 0;
        public static int CoinsValue { get; set; } = 0;
        public static Dictionary<int, int> RejectData { get; set; } = new();
        public static Dictionary<int, int> DispensedData { get; set; } = new();
        public static List<CDMS_Response> Response = new();


        public static async Task<List<CDMS_Response>> GoInitialize()
        {
            Response = new();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (!IsConnected())
                {
                    Response.Add( new CDMS_Response { isSuccess = false, UIResponse = HandlerResponse.WithoutConnection });
                    return;
                }

                CleanVariable();

                var SensorResponse = _handler.GetSensor();
                if (SensorResponse.ErrorCode == ErrorCDMS.RuntimeError)
                {
                    Response.Add( new CDMS_Response { isSuccess = false, UIResponse = HandlerResponse.SensorTimeOut });
                    return;
                }

                for (int i = 0; i < CDMS_Handler.Denominations.Split("-").Count(); i++)
                {
                    if (!SensorResponse.SensorInfo.CassetteConnected[i])
                    {
                        Response.Add(new CDMS_Response { isSuccess = false, UIResponse = HandlerResponse.CassettesDoNotMatch });
                        return;
                    }
                }

                var HandlerResponseInit = _handler.Initialize();
                if (!HandlerResponseInit.isSuccess) HandlerResponseInit.UIResponse = string.Format(HandlerResponse.ErrorInitialize, HandlerResponseInit.ErrorDescription);

                Response.Add(HandlerResponseInit);
            });
            return Response;
        }
       
        public static async Task<List<CDMS_Response>> GoDispend(int dispendValue)
        {
            Response = new();
            _valueToDispense = dispendValue;
            await Application.Current.Dispatcher.InvokeAsync(async() =>
            {
                Dispend(dispendValue);

            });
            
            return Response;
        }
        public static async Task<List<CDMS_Response>> GoDispendQuantities(int[] quantities)
        {
            Response = new();
            await Application.Current.Dispatcher.InvokeAsync(async() =>
            {
                if (!IsConnected()) {
                    Response.Add( new CDMS_Response { isSuccess = false, UIResponse = HandlerResponse.WithoutConnection });
                    return;
                }
                var DispenserResponse = _handler.Dispense(quantities);
                if (!DispenserResponse.isSuccess) DispenserResponse.UIResponse = string.Format(HandlerResponse.ErrorDispendQuantities, DispenserResponse.ErrorDescription);


               Response.Add(DispenserResponse);
            });
            return Response;
        }
        public static async Task<List<CDMS_Response>> GoGetSensor()
        {
            Response = new();   
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (!IsConnected())
                {
                    Response.Add(new CDMS_Response { isSuccess = false, UIResponse = HandlerResponse.WithoutConnection });
                    return;
                }

                var GetSensorResponse = _handler.GetSensor();
                if (!GetSensorResponse.isSuccess) GetSensorResponse.UIResponse = HandlerResponse.ErrorSensor;

                Response.Add(GetSensorResponse);
            });
            return Response;

        }
        public static async Task<List<CDMS_Response>> GoEject()
        {
            Response = new();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (!IsConnected())
                {
                    Response.Add(new CDMS_Response { isSuccess = false, UIResponse = HandlerResponse.WithoutConnection });
                    return;
                }
                var EjectResponse = _handler.Eject();
                if (!EjectResponse.isSuccess) EjectResponse.UIResponse = HandlerResponse.FailEject;
                
                Response.Add(EjectResponse);
            });
            return Response;
            
        }
        private static void Dispend(int dispendValue, List<int>? cassetteToIgnore = null)
        {

            if (!IsConnected())
            {
                var HantleR =  new CDMS_Response { isSuccess = false, UIResponse = HandlerResponse.WithoutConnection };
                Response.Add(HantleR);
                return;
            }

            if (cassetteToIgnore == null) cassetteToIgnore = new List<int>();

            _quantities = CalcReturn(dispendValue, cassetteToIgnore);
            var DispendResponse = _handler.Dispense(_quantities);

            if (DispendResponse.ErrorCode == ErrorCDMS.RuntimeError)
            {
                CoinsValue = _valueToDispense - DispensedValue;
                var HantleR = new CDMS_Response { isSuccess = false, UIResponse = HandlerResponse.DispendTimeOut };
                Response.Add(HantleR);
                return;
            }

            List<CDMS_DenomInfo> denomsWithMissing = new();
            int MissingValue = 0;

            foreach (var denomInfo in DispendResponse.DispenseData)
            {
                DispensedValue += denomInfo.Denomination * (denomInfo.DispensedQuantity);

                DispensedData[denomInfo.Denomination] += denomInfo.DispensedQuantity;
                RejectData[denomInfo.Denomination] += denomInfo.OutOfCassetteQuantity - denomInfo.DispensedQuantity;

                if ((denomInfo.RequestedQuantity - denomInfo.DispensedQuantity) != 0)
                {
                    denomsWithMissing.Add(denomInfo);
                }
                MissingValue += denomInfo.Denomination * (denomInfo.RequestedQuantity - denomInfo.DispensedQuantity);
            }

            if (DispendResponse.isSuccess && MissingValue == 0)
            {
                CoinsValue = _valueToDispense - DispensedValue;
                DispendResponse.UIResponse = HandlerResponse.DispendSuccess;
                Response.Add(DispendResponse);
                return;
            }

            //Si se confirma que hay un baúl vacio
            if (DispendResponse.ErrorDescription.EndsWith("JamOrEmpty") && !IsJammed())
            {
                var indexCassetteEmpty = ((int)DispendResponse.ErrorCode) - 61;
                if (indexCassetteEmpty >= (_handler.cassetesValues.Count() - 1))
                {
                    CoinsValue = _valueToDispense - DispensedValue;
                    DispendResponse.UIResponse = HandlerResponse.LastCassetteWithoutMoney;
                    Response.Add(DispendResponse);
                    return;
                }
                cassetteToIgnore.Add(indexCassetteEmpty);

                Response.Add(DispendResponse);
                Dispend(MissingValue, cassetteToIgnore);
                return;
            }
            //Si se presenta un atasco
            if ((DispendResponse.ErrorDescription.EndsWith("Sensor") || DispendResponse.ErrorDescription.EndsWith("Jam") || DispendResponse.ErrorDescription.EndsWith("JamOrEmpty")) && IsJammed())
            {
                if (!TryEject()) //Intenta Ejectar 3 veces
                {
                    CoinsValue = _valueToDispense - DispensedValue;
                    DispendResponse.UIResponse = HandlerResponse.FailMaxEject;
                    Response.Add(DispendResponse);
                    return;
                }

                Response.Add(DispendResponse);
                Dispend(MissingValue, cassetteToIgnore);
                CoinsValue = _valueToDispense - DispensedValue;
                return;
            };

            CoinsValue = _valueToDispense - DispensedValue;

            DispendResponse.UIResponse = HandlerResponse.StatusUndetermined;
            Response.Add(DispendResponse);
            return;
        }

        private static int[] CalcReturn(int valueToDispend, List<int> _cassetteToIgnore)
        {

            int[] _quantities = new int[_handler.cassetesValues.Count()];
            for (int i = 0; i < _handler.cassetesValues.Count(); i++)
            {
                var denominacion = _handler.cassetesValues[i];
                if ((valueToDispend < denominacion) || (_cassetteToIgnore.Contains(i)))
                {
                    _quantities[i] = 0;
                    continue;
                }
                _quantities[i] = (int)(valueToDispend / denominacion);
                valueToDispend -= (_quantities[i] * denominacion);
            }
            return _quantities;
        }
        private static bool TryEject() 
        {
            bool ejectIsSuccess= false;
            int tries = 3;
            do
            {
                Thread.Sleep(800);
                ejectIsSuccess = _handler.Eject().isSuccess && IsJammed();
                tries--;
            } while (!ejectIsSuccess && tries >=0);
            return ejectIsSuccess;  
        }
        private static bool IsConnected()
        {
            try
            {
                _handler.Disconnect();

                if (_handler.Connect()) return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
        private static bool IsJammed()
        {
            var sensors = _handler.GetSensor();

            if (sensors == null || sensors.ErrorCode == ErrorCDMS.RuntimeError) return false;
            if (sensors.SensorInfo == null) return false;

            bool skewJam = false;
            for (int i = 0; i < 8; i++)
            {
                skewJam = skewJam || sensors.SensorInfo.CassetteSkew1[i];
                skewJam = skewJam || sensors.SensorInfo.CassetteSkew2[i];
            }

            if (skewJam) return true;

            return (
                sensors.SensorInfo.ScanStart ||
                sensors.SensorInfo.Gate1 || sensors.SensorInfo.Gate2 ||
                sensors.SensorInfo.Exit ||
                sensors.SensorInfo.RejectIn
            );


        }
        private static void CleanVariable()
        {
            _valueToDispense = 0;
            DispensedValue = 0;
            DispensedData = new();
            RejectData = new();
            foreach (var denom in _handler.cassetesValues)
            {
                DispensedData.Add(denom, 0);
                RejectData.Add(denom, 0);
            }

        }
    
    }


}
