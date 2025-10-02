using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

public static class MethodParamInputDialog
{
    public static MethodParamValueDialogResult Show<T>(IWin32Window? owner, string methodName, string message, string caption) where T : class
    {
        var methodRef = typeof(T).GetMethod("AssignGate");
        if (methodRef == null) throw new ArgumentNullException(nameof(methodRef));
        var parameters = methodRef.GetParameters();

        if (string.IsNullOrEmpty(caption)) caption = nameof(MethodParamInputDialog);

        using (var form = new Form
        {
            Text = caption,
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
            ShowInTaskbar = false,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink
        })
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = parameters.Length + 2,
                Padding = new Padding(10)
            };

            // 🔔 Inject the message at the top
            var messageLabel = new Label
            {
                Text = message,
                AutoSize = true,
                Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 0, 0, 10)
            };
            layout.Controls.Add(messageLabel, 0, 0);
            layout.SetColumnSpan(messageLabel, 2);

            var controls = new Control[parameters.Length];
            MethodParamValueDialogResult r = new MethodParamValueDialogResult();

            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                layout.Controls.Add(new Label { Text = param.Name, AutoSize = true }, 0, i + 1);

                Control inputControl;

                if (param.ParameterType == typeof(int))
                    inputControl = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue };
                else if (param.ParameterType == typeof(string))
                    inputControl = new TextBox();
                else if (param.ParameterType == typeof(bool))
                    inputControl = new CheckBox();
                else
                    throw new NotSupportedException($"Unsupported parameter type: {param.ParameterType}");

                controls[i] = inputControl;
                layout.Controls.Add(inputControl, 1, i + 1);
            }

            var okButton = new Button { Text = MessageBoxButtons.OK.ToString(), DialogResult = DialogResult.OK, AutoSize = true };
            layout.Controls.Add(okButton, 1, parameters.Length + 1);

            form.Controls.Add(layout);
            form.AcceptButton = okButton;

            DialogResult result = DialogResult.Cancel;
            if (owner != null) { form.StartPosition = FormStartPosition.CenterParent; result = form.ShowDialog(owner); }
            else result = form.ShowDialog();

            r.Result = result;

            if (result == DialogResult.OK)
            {
                r.ClassType = methodRef.DeclaringType!;

                for (int i = 0; i < parameters.Length; i++)
                {
                    var control = controls[i];
                    var value = control switch
                    {
                        TextBox tb => tb.Text,
                        NumericUpDown nud => Convert.ChangeType(nud.Value, parameters[i].ParameterType),
                        CheckBox cb => cb.Checked,
                        DateTimePicker dtp => dtp.Value,
                        _ => throw new InvalidOperationException("Unknown control type")
                    };


                    var paramValue = new MethodParamValue
                    {
                        ParameterType = parameters[i].ParameterType,
                        ParameterName = parameters[i].Name!,
                        Value = value,
                        Index = i,
                    };

                    r.MethodParamValues.Add(paramValue);
                }
            }
            return r;
        }
    }

    public static MethodParamValueDialogResult Show<T>(string methodName, string message, string caption) where T : class
    {
        return Show<T>(null, methodName, message, caption);
    }
}
