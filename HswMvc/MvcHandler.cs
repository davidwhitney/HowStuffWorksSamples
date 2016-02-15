using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HswMvc
{
    public class MvcHandler : IHttpHandler
    {
        public bool IsReusable => true;
        private readonly List<Type> _controllers = new List<Type>();

        public MvcHandler()
        {
            foreach (var assemb in AppDomain.CurrentDomain.GetAssemblies())
            {
                var types = assemb.GetTypes();
                _controllers.AddRange(types.Where(x => x.Name.EndsWith("Controller")));
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            var controller = PickController(context);
            var method = PickMethod(context, controller);
            var instance = Activator.CreateInstance(controller);
            var response = method.Invoke(instance, null);

            HttpContext.Current.Response.Write(response);
        }

        private Type PickController(HttpContext context)
        {
            var url = context.Request.Url;

            var controller =
                _controllers.FirstOrDefault(c =>
                    url.PathAndQuery.StartsWith("/" + c.Name));

            if (controller == null)
            {
                controller = _controllers.Single(x => x.Name.StartsWith("HomeController"));
            }

            return controller;
        }

        private MethodInfo PickMethod(HttpContext context, Type controller)
        {
            var url = context.Request.Url;

            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            var publicMethods = controller.GetMethods(bindingFlags);

            var actionMethod = publicMethods.FirstOrDefault(method =>
                url.PathAndQuery.Contains("/" + method.Name));

            if (actionMethod == null)
            {
                actionMethod = publicMethods.Single(x => x.Name == "Index");
            }

            return actionMethod;
        }
    }
}
