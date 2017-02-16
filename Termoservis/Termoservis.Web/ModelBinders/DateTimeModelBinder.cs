using System;
using System.Globalization;
using System.Web.Mvc;

namespace Termoservis.Web.ModelBinders
{
    /// <summary>
    /// The DateTime model binder with custom format support.
    /// </summary>
    public class DateTimeModelBinder : DefaultModelBinder
    {
        private readonly string customFormat;


        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeModelBinder"/> class.
        /// </summary>
        /// <param name="customFormat">The custom format.</param>
        public DateTimeModelBinder(string customFormat)
        {
            this.customFormat = customFormat;
        }


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
            if (string.IsNullOrWhiteSpace(value?.AttemptedValue))
                return null;
            return DateTime.ParseExact(value.AttemptedValue, customFormat, CultureInfo.InvariantCulture);
        }
    }
}
