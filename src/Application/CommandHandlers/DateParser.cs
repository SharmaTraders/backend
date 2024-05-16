using System.Globalization;
using Tools;

namespace Application.CommandHandlers;

public static class DateParser {

    public static DateOnly ParseDate(string date) {
        bool parsed = DateOnly.TryParseExact(date, Constants.DateFormat,CultureInfo.InvariantCulture,DateTimeStyles.None,out DateOnly parsedDate);
        if (!parsed)
        {
            throw new DomainValidationException("Date", ErrorCode.BadRequest, ErrorMessages.DateFormatInvalid);
        }
        return parsedDate;
    }
    
}