﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using GHE = Grasshopper_Engine;

namespace Alligator.Base
{
    public class SetProperty : GH_Component
    {
        public SetProperty() : base("SetProperty", "SetProperty", "Set property of a BHoM object", "Alligator", "Base") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("E3C42F6C-15AC-4FBA-8BCC-F3E773B1C1D8");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
            pManager.AddTextParameter("key", "key", "Property name", GH_ParamAccess.item);
            pManager.AddGenericParameter("value", "value", "Property value", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BHoM object", "object", "BHoM object to convert", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object o = GHE.DataUtils.GetGenericData<object>(DA, 0);
            string key = GHE.DataUtils.GetData<string>(DA, 1);
            object value = GHE.DataUtils.GetGenericData<object>(DA, 2);

            object newObject = o;
            System.Reflection.MethodInfo inst = o.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (inst != null)
                newObject = inst.Invoke(o, null);

            System.Reflection.PropertyInfo prop = newObject.GetType().GetProperty(key);
            if (prop == null)
            {
                DA.SetData(0, null);
            }
            else
            {
                prop.SetValue(newObject, value);
                DA.SetData(0, newObject);
            }
                
        }
    }
}