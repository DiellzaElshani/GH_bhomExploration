﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using BHB = BH.oM.Base;

namespace Alligator.Base
{
    public class FromJSON : GH_Component
    {
        public FromJSON() : base("FromJSON", "FromJSON", "Create a BH.oM object from a JSON string", "Alligator", "Base") { }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Properties.Resources.BH.oM_FromJSON; }
        }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("45912883-EE6F-49F4-BEBA-4A123EC2370C");
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("json", "json", "json representation of the BH.oM object", GH_ParamAccess.item);
            pManager.AddTextParameter("password", "password", "password to decrypt data", GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BH.oM object", "object", "Resulting BH.oM object", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string json = ""; // Utils.GetGenericData<string>(DA, 0);
            string password = "";
            DA.GetData<string>(0, ref json);
            DA.GetData<string>(1, ref password);
            DA.SetDataList(0, BHB.BH.oMJSON.ReadPackage(json, password));
        }
    }
}
