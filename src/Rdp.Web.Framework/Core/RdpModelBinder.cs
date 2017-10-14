using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Rdp.Web.Framework.Core
{
  /*  public class RdpModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext,
            ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.PropertyType == typeof(string) && value == null)
                value = "0";    
            
            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {             var value = base.BindModel(controllerContext, bindingContext);
             if(bindingContext.ModelType == typeof(string) && value == null)
                 value = "0";
             return value;
         }


        protected override void  BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {

            
             base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            return base.GetPropertyValue( controllerContext,  bindingContext,  propertyDescriptor,  propertyBinder);
        }

    }*/
}
