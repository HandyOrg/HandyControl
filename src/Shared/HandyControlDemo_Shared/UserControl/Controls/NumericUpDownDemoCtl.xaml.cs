using HandyControl.Data;


namespace HandyControlDemo.UserControl
{
    public partial class NumericUpDownDemoCtl
    {
        public NumericUpDownDemoCtl()
        {
            InitializeComponent();

            NumericUpDownCustomVerify.VerifyFunc = str => double.TryParse(str, out var v)
                ? v % 2 < 1e-06
                    ? OperationResult.Failed(Properties.Langs.Lang.Error)
                    : OperationResult.Success()
                : OperationResult.Failed(Properties.Langs.Lang.Error);
        }
    }
}
