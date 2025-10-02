using System.Text;

public class MethodParamValueDialogResult
{
    public Type ClassType {  get; internal set; } = typeof(object);
    public HashSet<MethodParamValue> MethodParamValues = new HashSet<MethodParamValue>();
    public DialogResult Result { get; internal set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (MethodParamValue pv in this.MethodParamValues)
        {
            stringBuilder.AppendLine($"[{pv.ParameterType}] {pv.ParameterName} = {pv.Value}");
        }
        return stringBuilder.ToString();
    }
}
