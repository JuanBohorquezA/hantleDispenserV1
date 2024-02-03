using hantleDispenser.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace hantleDispenser.UserControls
{
    public class AppUserControl: UserControl
    {
        protected Navigator _nav =  Navigator.Instance;

        protected void Goto(UserControl view)
        {
            _nav.NavigateTo(view);
        }
    }
}
