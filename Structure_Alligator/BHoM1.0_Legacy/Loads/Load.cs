﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Loads;
using Grasshopper.Kernel;
using BH.UI.Alligator.Components;
using GHE = BH.Engine.Grasshopper;
using BHE = BH.oM.Structural.Elements;
using BHI = BH.oM.Structural.Interface;
using BH.Engine.Grasshopper.Components;

namespace BH.UI.Alligator.Structural.Loads
{
    class CreateBarGravityLoad : BHoMBaseComponent<GravityLoad>
    {
        public CreateBarGravityLoad() : base("Create Bar Gravity Load", "CreateBarGravityLoad", "Create a BH bar gravity load", "Structure", "Loads") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("35c8ac2b-f99a-4ae8-a6c1-fadfc24de74c");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_Bar_Gravity; }
        }
    }

    class CreateBarPrestressLoad : BHoMBaseComponent<BarPrestressLoad>
    {
        public CreateBarPrestressLoad() : base("Create Bar Prestress Load", "CreateBarPrestressLoad", "Create a BH bar prestress load", "Structure", "Loads") { }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("8470d3f8-13bf-477b-9ec4-88ce437ea5b6");
            }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_Bar_Prestress; }
        }
    }

    public class ExportLoad : GH_Component
    {
        public ExportLoad() : base("Export Load", "ExLoad", "Creates a load", "Structure", "Loads") { }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Application to export bars to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Loads", "L", "BHoM loads to export", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Execute", "R", "Generate loads", GH_ParamAccess.item);

            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "Success", "Return wheather the operation was succuessful or not", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool success = false;

            if (GHE.DataUtils.Run(DA, 2))
            {
                BHI.IElementAdapter app = GHE.DataUtils.GetGenericData<BHI.IElementAdapter>(DA, 0);
                if (app != null)
                {
                    List<ILoad> loads = GHE.DataUtils.GetGenericDataList<ILoad>(DA, 1);
                    loads = loads.Where(x => x != null).ToList();
                    app.SetLoads(loads);
                    success = true;
                }
            }
            DA.SetData(0, success);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("8cd7851d-0869-492a-adcf-49662aec55b1"); }
        }

        /// <summary> Icon (24x24 pixels)</summary>
        protected override System.Drawing.Bitmap Internal_Icon_24x24
        {
            get { return Alligator.Structural.Properties.Resources.BHoM_Bar_Force_Export; }
        }
    }

    
}