/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using BH.oM.Base;
using BH.Engine.Reflection;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BH.UI.Grasshopper.Base
{
    public class SetProperty : GH_Component
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override System.Drawing.Bitmap Internal_Icon_24x24 { get; } = Properties.Resources.BHoM_SetProperty; 

        public override Guid ComponentGuid { get; } = new Guid("E3C42F6C-15AC-4FBA-8BCC-F3E773B1C1D8"); 

        public override GH_Exposure Exposure { get; } = GH_Exposure.hidden;

        public override bool Obsolete { get; } = true;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public SetProperty() : base("Set Property", "SetProperty", "Returns copy of object with a set property", "Grasshopper", " Engine") { }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
            pManager.AddTextParameter("key", "key", "Property name", GH_ParamAccess.item);
            pManager.AddScriptVariableParameter("value", "value", "Property value", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new BHoMObjectParameter(), "BHoM object", "object", "resulting object", GH_ParamAccess.item);
        }

        /*******************************************/

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Compute.ClearCurrentEvents();

            BHoMObject obj = new BHoMObject();
            string key = "";
            object value = default(object);

            DA.GetData(0, ref obj);
            DA.GetData(1, ref key);

            Type type = typeof(object);
            PropertyInfo prop = obj.GetType().GetProperty(key);
            if (prop != null)
                type = obj.GetType().GetProperty(key).PropertyType;

            if (Params.Input[2].Access == GH_ParamAccess.list) // TODO: We need to take care of the case for list of list (not currently available as an option in the ScriptParam menu)
            {
                Type subType = typeof(object);
                if (type.GetGenericArguments().Count() > 0)
                    subType = type.GetGenericArguments().First();
                MethodInfo generic = DA.GetType().GetMethods().Where(x => x.Name == "GetDataList" && x.GetParameters().First().ParameterType == typeof(int)).First().MakeGenericMethod(subType);
                object[] arguments = new object[] { 2, Activator.CreateInstance(typeof(List<>).MakeGenericType(subType)) };
                generic.Invoke(DA, arguments);
                value = arguments[1];
            }
            else
                DA.GetData(2, ref value);

            while (value is IGH_Goo)
                value = ((IGH_Goo)value).ScriptVariable();

            if (value.GetType().Namespace.StartsWith("Rhino.Geometry"))
                value = BH.Engine.Rhinoceros.Convert.ToBHoM(value as dynamic);

            if (value.GetType() != type)
            {
                ConstructorInfo constructor = type.GetConstructor(new Type[] { value.GetType() });
                if (constructor != null)
                    value = constructor.Invoke(new object[] { value });
            }

            IBHoMObject newObject = obj.GetShallowClone();

            try
            {
                newObject.SetPropertyValue(key, value);
            }
            catch (Exception e)
            {
                if (Params.Input[2].Access == GH_ParamAccess.item)
                    Compute.RecordError(e.ToString() + "\nHave you forgotten to mark the input as a List in its menu?");
                else
                    Compute.RecordError(e.ToString());
            }

            DA.SetData(0, newObject);

            Logging.ShowEvents(this, Query.CurrentEvents());
        }

        /*******************************************/
    }
}
