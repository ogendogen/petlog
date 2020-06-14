using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace PetLog.Converters
{
    /// <summary>
    /// Converter checks if object is not null and checks checkbox if not existing (reversed version)
    /// </summary>
    public class ReversedCheckboxConverter : IValueConverter
    {
        /// <summary>
        /// Checks if object exists and converts to bool
        /// </summary>
        /// <param name="value">Input value - any object</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Additional parameter</param>
        /// <param name="culture">Culture information</param>
        /// <returns>False if object exists, true for null</returns>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
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
            throw new NotImplementedException();
        }
    }
}
