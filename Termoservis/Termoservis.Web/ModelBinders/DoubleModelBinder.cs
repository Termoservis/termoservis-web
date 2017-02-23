using System.Web.Mvc;

namespace Termoservis.Web.ModelBinders
{
    /// <summary>
    /// The double model binder with support for comma separators.
    /// </summary>
    /// <seealso cref="DefaultModelBinder" />
    public class DoubleModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Binds the model by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>
        /// The bound object.
        /// </returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            double result = 0;
            double.TryParse(value?.AttemptedValue.Replace(",", "."), out result);
            return result;
        }
    }
}