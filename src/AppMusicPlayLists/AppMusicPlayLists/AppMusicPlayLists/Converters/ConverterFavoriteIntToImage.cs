using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AppMusicPlayLists.Converters
{
    public class ConverterFavoriteIntToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((int)value == 0)
            {
                return "EmptyHeart_32.png";
            }
            else
            {
                return "FavoritedHeart_32.png";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return (bool)value ? 1 : 0;

            if (value.ToString() == "EmptyHeart_32.png")
            {
                return 0;
            }
            else
            {
                return 1; 
            }
        }
    }
}
