using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Database.Models;

namespace PetLog.Converters
{
    /// <summary>
    /// Converter to check date of last vaccination
    /// </summary>
    public class VaccinationConverter : IValueConverter
    {
        /// <summary>
        /// Converts hashset of vaccinations to datetime of last vaccination or string if there is no vaccinations
        /// </summary>
        /// <param name="value">Input value - hashset of vaccinations</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Additional parameter</param>
        /// <param name="culture">Culture information</param>
        /// <returns>Datetime if any found, string for empty collection</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HashSet<Vaccination> vaccinations = (HashSet<Vaccination>)value;
            if (vaccinations != null && vaccinations.Count > 0)
            {
                DateTime dt = vaccinations.OrderByDescending(vacc => vacc.Date).First().Date;
                return dt.ToString();
            }

            return "Brak";
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
