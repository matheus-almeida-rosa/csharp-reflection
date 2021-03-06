﻿using CsharpReflection.Infrastructure.Binding;
using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace CsharpReflection.Infrastructure
{
    public class RequestControllerHandler
    {
        private readonly ActionBinder _actionBinder = new ActionBinder();

        /// <summary>
        /// This method demostrate the use of Activator .net class. Tha Activator class allow us to create instance of another classes in execution time
        /// </summary>
        /// <param name="response"></param>
        /// <param name="path"></param>
        public void Handle(HttpListenerResponse response, string path)
        {
            var pieces = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var controllerName = pieces.First();
            var actionName = pieces[1];

            var controllerFullName = $"CsharpReflection.Controllers.{controllerName}Controller";

            var controllerWrapper = Activator.CreateInstance("CsharpReflection", controllerFullName, new object[0]);
            var controller = controllerWrapper.Unwrap();

            //var methodInfo = controller.GetType().GetMethod(actionName);
            var methodInfo = _actionBinder.GetActionBindInfo(controller, path);

            var actionResult = methodInfo.Invoke(controller) as string;

            var bytesContent = Encoding.UTF8.GetBytes(actionResult);

            response.StatusCode = 200;
            response.ContentLength64 = bytesContent.Length;

            response.OutputStream.Write(bytesContent, 0, bytesContent.Length);
            response.OutputStream.Close();
        }
    }
}
