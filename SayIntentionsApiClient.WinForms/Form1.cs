using System.Text;

namespace SayIntentionsApiClient.WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        { 
            var r = MethodParamInputDialog.Show<SayIntentionsApiClient>(this, "AssignGate", "Set the fields", "SI Gate Assignment dialog");
            if (r.Result == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder(); 
                foreach(MethodParamValue pv in r.MethodParamValues)
                {
                    sb.AppendLine($"[{pv.ParameterType}] {pv.ParameterName} = {pv.Value}");
                }
                MessageBox.Show(sb.ToString());
            }
            InitializeComponent();
        }
    }
}
