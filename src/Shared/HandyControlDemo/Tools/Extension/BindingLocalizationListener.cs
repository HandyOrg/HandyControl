using System.Windows.Data;

namespace HandyControlDemo.Tools.Extension
{
    /// <summary>
    /// Listener for a culture change when binding is localized
    /// </summary>
    public class BindingLocalizationListener : BaseLocalizationListener
    {
        private BindingExpressionBase BindingExpression { get; set; }

        public void SetBinding(BindingExpressionBase bindingExpression)
        {
            BindingExpression = bindingExpression;
        }

        protected override void OnCultureChanged()
        {
            try
            {
                // Updating the result of a binding expression
                // In this case, the converter is called again for the new culture
                BindingExpression?.UpdateTarget();
            }
            catch
            {
                // ignored
            }
        }
    }
}
