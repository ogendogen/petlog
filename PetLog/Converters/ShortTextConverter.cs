using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace PetLog.Converters
{
    /// <summary>
    /// Converts text to shorter version (up to 30 characters)
    /// </summary>
    public class ShortTextConverter : IValueConverter
    {
        /// <summary>
        /// Cuts text to 30 characters if longer
        /// </summary>
        /// <param name="value">Input value - string</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Additional parameter</param>
        /// <param name="culture">Culture information</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value.ToString();
            if (text.Length > 30)
            {
                return $"{text.Substring(0, 30)}...";
            }
            
            return text;
        }

        /// <summary>
        /// Converts back (not used)
        /// </summary>
        /// <param name="value">Input value</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Additional parameter</param>
        /// <param name="culture">Culture information</param>
        /// <returns>NotImplementedException</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
