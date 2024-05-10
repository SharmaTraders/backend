namespace Domain.common;

public abstract class Enumeration 
{

    public int Value { get; }
    public string DisplayName { get; set; }

    protected Enumeration()
    {
    }

    protected Enumeration(int value, string displayName)
    {
        Value = value;
        DisplayName = displayName;
    }

    public override string ToString()
    {
        return DisplayName;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        bool typeMatches = GetType() == obj.GetType();
        bool valueMatches = Value.Equals(otherValue.Value);
        bool displayNameMatches = DisplayName.Equals(otherValue.DisplayName);

        return typeMatches && valueMatches && displayNameMatches;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode() * DisplayName.GetHashCode();
    }

}