public class MethodParamValue
{
    public Type ParameterType { get; internal set; } = typeof(object);
    public string ParameterName { get; internal set; } = string.Empty;
    public object? Value { get; internal set; } = null;
    public int Index { get; internal set; }
}
