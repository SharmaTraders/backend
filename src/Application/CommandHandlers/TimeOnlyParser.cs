using System.Globalization;
using Tools;

namespace Application.CommandHandlers;

public static class TimeOnlyParser
{
    public static TimeOnly ParseTime(string time)
    {
        bool parsed = TimeOnly.TryParseExact(time, Constants.TimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeOnly parsedTime);
        if (!parsed)
        {
            throw new DomainValidationException("Time", ErrorCode.BadRequest, ErrorMessages.TimeFormatInvalid);
        }
        return parsedTime;
    }
}