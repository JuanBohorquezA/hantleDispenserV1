using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace hantleDispenser.Domain.UIDictionaries.DinamycResources
{
    public static class RegisterDinamycResources
    {
        public static void AddDinamycResources( this ResourceDictionary dictionary, double maxDimension, double minDimension)
        {
            //Creamos los Recursos dinamicos que usaremos en toda la aplicacion
            dictionary["ButtonWidth"] = (double)Math.Round(maxDimension * 0.4);
            dictionary["MButtonWidth"] = (double)Math.Round(maxDimension * 0.2);
            dictionary["ButtonHeight"] = (double)Math.Round(minDimension * 0.15);
            dictionary["MButtonHeight"] = (double)Math.Round(minDimension * 0.075);
            dictionary["CommonFontSize"] = (double)Math.Round(minDimension * 0.034);
            dictionary["MininFontSize"] = (double)Math.Round(minDimension * 0.025);
            dictionary["RowDefinitionHeight"] = new GridLength((double)Math.Round(minDimension * 0.24), GridUnitType.Pixel);
            dictionary["MarginGrid"] = new Thickness(Math.Round(minDimension * 0.02));
            dictionary["MinMarginGrid"] = new Thickness(Math.Round(minDimension * 0.01));
            dictionary["IconSize"] = (double)Math.Round(minDimension * 0.0497);
            dictionary["ButtonCornerRadius"] = new CornerRadius((double)Math.Round(minDimension * 0.0497));
            dictionary["MidMarginGrid"] = new Thickness(Math.Round(minDimension * 0.298), Math.Round(minDimension * 0.010), Math.Round(minDimension * 0.298), Math.Round(minDimension * 0.010));
            dictionary["MinCornersRadius"] = new CornerRadius(Math.Round(minDimension * 0.005));



            //definiciones para el ancho de los botones de Menu
            dictionary["ButtonWidthMenu"] = (double)Math.Round(maxDimension * 0.2);
            dictionary["ButtonHeightMenu"] = (double)Math.Round(maxDimension * 0.05);

            dictionary["MinButtonWidth"] = (double)Math.Round(minDimension * 0.24);
            dictionary["MidButtonWidth"] = (double)Math.Round(minDimension * 0.44);
            dictionary["MinButtonHeight"] = (double)Math.Round(minDimension * 0.05);
            dictionary["ButtonDimentions"] = (double)Math.Round(minDimension * 0.08);
        }
    }
}
