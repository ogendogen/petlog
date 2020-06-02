using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using Database.Models;

namespace PetLog
{
    public class AnimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string originalEnum = ((AnimalType)value).ToString();

            switch(originalEnum)
            {
                case "Dog":
                    return "Pies";

                case "Cat":
                    return "Kot";

                case "Other":
                    return "Inny";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
