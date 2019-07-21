﻿using Isop.Server.Models;
using System.Collections.Generic;
using System.Linq;

namespace Isop.Server
{
    #if false
    public class ControllerModule : NancyModule
    {
        public ControllerModule(IIsopServer data)
        {
            Get["/{controller:Controller}"] = _ =>
            {
                Models.Controller controller = data.GetController(_.controller);
                return Negotiate
                   .WithModel(controller)
                   .WithView("Controller.html");
            };

            Get["/{controller:Controller}/{method}"] = _ =>
            {
                Method method = data.GetControllerMethod(_.controller, _.method);
                return Negotiate
                   .WithModel(method)
                   .WithView("ControllerAction.html");
            };

            Post["/{controller:Controller}/{method}"] = _ =>
            {
                try
                {
                    var method = data.GetControllerMethod(_.controller, _.method);
                    IEnumerable<string> result = data.InvokeMethod(method, Request.Form);
                    return new YieldResultResponse(result);
                }
                catch (TypeConversionFailedException e)
                {
                    return Negotiate
                       .WithModel(ToModel(e))
                       .WithView("Errors.html")
                       .WithStatusCode(400);
                }
                catch (MissingArgumentException ex)
                {
                    return Negotiate
                       .WithModel(ToModel(ex))
                       .WithView("Errors.html")
                       .WithStatusCode(400);
                }
            };
        }

        private object ToModel(MissingArgumentException ex)
        {
            return ex.Arguments.Select(arg => new MissingArgument
            {
                Message = "Missing arguments.",
                Argument = arg
            }).ToArray();
        }

        private object ToModel(TypeConversionFailedException e)
        {
            return new[]{
                new TypeConversionFailed
                {
                    Message = "Type conversion failed.",
                    Argument = e.Argument,
                    Value = e.Value,
                    TargetType = e.TargetType.FullName
                }
            };
        }
    }
    #endif
}
