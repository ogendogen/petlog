using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Database.Models;

namespace PetLog.Converters
{
    public class VaccinationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HashSet<Vaccination> vaccinations = (HashSet<Vaccination>)value;
            if (vaccinations.Count > 0)
            {
                DateTime dt = vaccinations.OrderByDescending(vacc => vacc.Date).First().Date;
                return dt.ToString();
            }

            return "Brak";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
