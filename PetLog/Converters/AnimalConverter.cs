using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using Database.Models;

namespace PetLog.Converters
{
    /// <summary>
    /// WPF Converter for animal's enum
    /// </summary>
    public class AnimalConverter : IValueConverter
    {
        /// <summary>
        /// Convert original enum in english to polish
        /// </summary>
        /// <param name="value">Input value - AnimalType enum</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Additional parameter</param>
        /// <param name="culture">Culture information</param>
        /// <returns>Translated enum object as string</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string originalEnum = ((AnimalType)value).ToString();

            switch (originalEnum)
            {
                case "Dog":
                    return "Pies";

                case "Cat":
                    return "Kot";

                case "Other":
                    return "Inny";

                default:
                    break;
            }

            return null;
        }

        /// <summary>
        /// Convert back (not used)
        /// </summary>
        /// <param name="value">Input value</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Additional parameter</param>
        /// <param name="culture">Culture information</param>
        /// <returns>NotImplementedException</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new NotImplementedException();
        }
    }
}
