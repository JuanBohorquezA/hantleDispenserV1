using hantleDispenser.Domain.Models;
using HantleDispenserAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace hantleDispenser.Domain
{
    public static class SessionManager
    {
        public static User? CurrentUser {  get; private set; }
        public static DenominationsQuantities CurrentProps { get; private set; }
        public static List<CDMS_Response> HantleResponse { get; set; }

        public static void InicializeSession(User user)
        {
            CurrentUser = user;
            
        }
        public static void InicializeHantleSeccion(DenominationsQuantities hantle)
        {
            CurrentProps = hantle;
        }


    }
}
