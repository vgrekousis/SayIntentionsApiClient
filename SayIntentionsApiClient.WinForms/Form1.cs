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
                foreach(ParamValue pv in r.ParamValues)
                {
                    sb.AppendLine($"[{pv.ParamType}] {pv.ParamName} = {pv.Value}");
                }
                MessageBox.Show(sb.ToString());
            }
            InitializeComponent();
        }
    }
}
