using Domain.common;
using Domain.Entity.ValueObjects;

namespace Domain.Entity;

public class EmployeeTimeRecord: IEntity<Guid>
{
    public Guid Id { get; set; }
    
    private NonFutureDate _date { get; set; }
    public required DateOnly Date {
        get => _date.Value;
        set => _date = new NonFutureDate(value);
    }
    
    private TimeSpan _startTime;
    public TimeSpan StartTime
    {
        get => _startTime;
        set
        {
            ValidateStartTime(value);
            _startTime = value;
        }
    }    
    private TimeSpan _endTime;
    public TimeSpan EndTime
    {
        get => _endTime;
        set
        {
            ValidateEndTime(value);
            _endTime = value;
        }
    }
    
    private TimeSpan _break;
    public TimeSpan Break
    {
        get => _break;
        set
        {
            ValidateBreak(value);
            _break = value;
        }
    }
    
    private static void ValidateStartTime(TimeSpan value)
    {
        if (value.TotalHours < 0 || value.TotalHours > 24)
        {
            throw new DomainValidationException("StartTime", ErrorCode.BadRequest, "Start time must be between 00:00 and 24:00.");
        }
    }
    
    private void ValidateEndTime(TimeSpan value)
    {
        if (value.TotalHours is < 0 or > 24 || value < _startTime)
        {
            throw new DomainValidationException("EndTime", ErrorCode.BadRequest, "End time must be between 00:00 and 24:00.");
        }
    }
    
    private void ValidateBreak(TimeSpan value)
    {
        if (value.TotalHours is < 0 or > 24 || value > _endTime.Subtract(_startTime))
        {
            throw new DomainValidationException("Break", ErrorCode.BadRequest, "Break time must be between 00:00 and 24:00.");
        }
    }
    
}