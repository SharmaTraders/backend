using Domain.common;
using Domain.Entity.ValueObjects;

namespace Domain.Entity
{
    public class EmployeeWorkShift
    {
        public Guid Id { get; set; }
        
        private NonFutureDate _date { get; set; }
        public required DateOnly Date
        {
            get => _date.Value;
            set => _date = new NonFutureDate(value);
        }
        
        private TimeOnly _startTime;
        public required TimeOnly StartTime
        {
            get => _startTime;
            set => _startTime = value;
        }    
        private TimeOnly _endTime;
        public required TimeOnly EndTime
        {
            get => _endTime;
            set
            {
                ValidateEndTime(value);
                _endTime = value;
            }
        }
        
        private int _breakMinutes;
        public int BreakMinutes
        {
            get => _breakMinutes;
            set
            {
                ValidateBreak(value);
                _breakMinutes = value;
            }
        }
        
        private void ValidateEndTime(TimeOnly value)
        {
            if (value < _startTime)
            {
                throw new DomainValidationException("EndTime", ErrorCode.BadRequest, ErrorMessages.EndTimeBeforeStartTime);
            }
        }
        
        private void ValidateBreak(int value)
        {
            if (value is < 0 or > 1440) // 1440 minutes in a day
            {
                throw new DomainValidationException("Break", ErrorCode.BadRequest, ErrorMessages.BreakTimeInvalid);
            }
            
            var totalWorkMinutes = (_endTime.ToTimeSpan() - _startTime.ToTimeSpan()).TotalMinutes;
            if (value > totalWorkMinutes)
            {
                throw new DomainValidationException("Break", ErrorCode.BadRequest, ErrorMessages.BreakTimeMoreThanWorkTime);
            }
        }
    }
}